using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace QA.Core.Replacing
{
    /// <summary>
    /// Управление заменами, указанными с помощью атрибутов CultureDependent и DependentValue.
    /// В два раза быстрее по сравнению с ReplacementProcessor.
    /// Данный объект следует кешировать.
    /// </summary>
    public class SwapReplacementProcessor : IReplacementProcessor
    {
        private readonly Dictionary<string, AttributeInfo> _nestedProcessors;
        private readonly Dictionary<Type, IReplacementProcessor> _processorLookup;
        private readonly Dictionary<string, Action<object>> _swapAllOperations;
        private bool _hasNested;


        /// <summary>
        /// Тип объекта
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="targetType"></param>
        public SwapReplacementProcessor(Type targetType)
            : this()
        {
            TargetType = targetType;
            BuildUp(targetType, this, _processorLookup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="processorLookup"></param>
        private SwapReplacementProcessor(Type targetType, Dictionary<Type, IReplacementProcessor> processorLookup)
            : this()
        {
            TargetType = targetType;
            BuildUp(targetType, this, processorLookup);
        }

        private SwapReplacementProcessor()
        {

            _processorLookup = new Dictionary<Type, IReplacementProcessor>();
            _nestedProcessors = new Dictionary<string, AttributeInfo>();
            _swapAllOperations = new Dictionary<string, Action<object>>();
        }

        /// <summary>
        /// Произвести замену свойств объекта
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        public object Process(object obj, string cultureKey)
        {
            return ProcessInternal(obj, cultureKey);
        }

        /// <summary>
        ///  Производит замену свойств объекта
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object Process(object obj)
        {
            return ProcessInternal(obj, CultureInfo.CurrentUICulture.Name.ToLower());
        }

        private object ProcessInternal(object obj, string cultureKey)
        {
            if (obj == null)
                return null;

            Action<object> current = null;
            if (_swapAllOperations.TryGetValue(cultureKey, out current))
            {
                // производим все замены свойств данного объекта
                current(obj);
            }

            if (_hasNested)
            {
                // производим замены дочерних объектов
                foreach (var proc in _nestedProcessors)
                {
                    AttributeInfo info = proc.Value;
                    var value = info.Accesor.GetValue(obj);
                    var np = info.NestedProcessor;
                    if (value != null)
                    {
                        if (!info.IsCollection)
                        {
                            np.Process(value, cultureKey);
                        }
                        else
                        {
                            foreach (var item in (IEnumerable)value)
                            {
                                np.Process(item, cultureKey);
                            }
                        }
                    }
                }
            }
            return obj;
        }

        #region Initialization

        private static void BuildUp(Type targetType, SwapReplacementProcessor currentProcessor, Dictionary<Type, IReplacementProcessor> processorLookup)
        {
            if (processorLookup.ContainsKey(targetType))
            {
                return;
            }

            processorLookup.Add(targetType, currentProcessor);
            var targetLookup = new Dictionary<string, AttributeInfo>();
            var valueProperties = new List<AttributeInfo>();

            var propertyInfos = targetType.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var targetAttribute = propertyInfo.GetCustomAttributes(typeof(CultureDependentAttribute), true)
                    .Cast<CultureDependentAttribute>()
                    .FirstOrDefault();

                var valueAttribute = propertyInfo.GetCustomAttributes(typeof(DependentValueAttribute), true)
                   .Cast<DependentValueAttribute>()
                   .FirstOrDefault();

                var memberAttribute = propertyInfo.GetCustomAttributes(typeof(CultureDependentMemberAttribute), true)
                  .Cast<CultureDependentMemberAttribute>()
                  .FirstOrDefault();

                if (targetAttribute == null && valueAttribute == null && memberAttribute == null)
                {
                    continue;
                }

                var propertyType = propertyInfo.PropertyType;
                if (targetAttribute != null && propertyInfo.CanWrite)
                {
                    targetLookup.Add(propertyInfo.Name, new AttributeInfo(propertyInfo.Name, targetAttribute, propertyType));
                }

                if (valueAttribute != null && propertyInfo.CanRead)
                {
                    valueProperties.Add(new AttributeInfo(propertyInfo.Name, valueAttribute, propertyType));
                }

                if (memberAttribute != null)
                {
                    InitializeNestedProcessor(targetType, currentProcessor, processorLookup, propertyInfo, propertyType);
                }
            }

            // проверки
            foreach (var property in targetLookup)
            {
                if (valueProperties
                    .GroupBy(k => k.ValueAttribute.TargetPropertyName)
                    .Where(g => g.Key == property.Key)
                    .SelectMany(x => x)
                    .Select(x => x.PropertyType)
                    .Any(x => x != property.Value.PropertyType))
                {
                    throw new InvalidOperationException(string.Format("Target and Value properties should nave the same type. Name: {0}, Expected: {1}",
                        property.Key,
                        property.Value.PropertyType));
                }
            }

            var sourceLookup = valueProperties
                .Where(x => !targetLookup.ContainsKey(x.Name))
                    .GroupBy(k => k.ValueAttribute.CultureName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.GroupBy(e => e.ValueAttribute.TargetPropertyName)
                            .ToDictionary(
                                g2 => g2.Key,
                                g2 => g2.Single())
                    );

            // генерация обменных операций
            ParameterExpression obj = Expression.Parameter(typeof(object), "obj");
            List<Expression> all = new List<Expression>();

            foreach (var culture in sourceLookup)
            {
                foreach (var property in targetLookup)
                {
                    AttributeInfo info = null;

                    if (culture.Value.TryGetValue(property.Key, out info))
                    {
                        var getMethodInfo = currentProcessor.TargetType.GetProperty(info.Name).GetGetMethod();
                        var setMethodInfo = currentProcessor.TargetType.GetProperty(property.Value.Name).GetSetMethod();

                        // теперь можно заменять
                        all.Add(Expression.Call(
                            Expression.Convert(obj, currentProcessor.TargetType), // приводим object к типу объекта
                            setMethodInfo, // сеттер
                            Expression.Call(
                                Expression.Convert(obj, currentProcessor.TargetType),
                                getMethodInfo // геттер
                                )
                            ));

                        //var body = Expression.Assign(Expression.Property(Expression.Convert(obj, currentProcessor.TargetType), property.Value.Name),
                        //    Expression.Property(Expression.Convert(obj, currentProcessor.TargetType), info.Name));
                        //all.Add(body);
                    }
                }

                var expr = Expression.Lambda<Action<object>>(
                    Expression.Block(all),
                    obj);

                currentProcessor._swapAllOperations.Add(culture.Key, expr.Compile());
            }
        }

        private static void InitializeNestedProcessor(Type targetType, SwapReplacementProcessor processor, Dictionary<Type, IReplacementProcessor> processors, System.Reflection.PropertyInfo propertyInfo, Type propertyType)
        {
            processor._hasNested = true;
            IReplacementProcessor proc = null;

            // для данного свойства создаем (или получаем из словаря) еще один процессор
            if (!processors.TryGetValue(propertyType, out proc))
            {
                // обработка коллекций
                if (typeof(IEnumerable).IsAssignableFrom(propertyType))
                {
                    if (propertyType.IsGenericType)
                    {
                        var genericParameters = propertyType.GetGenericArguments();
                        if (genericParameters.Length != 1)
                        {
                            throw new InvalidOperationException(string.Format("The type '{0}' cannot be used as DependentMember. Too much generic parameters.", propertyType));
                        }
                        if (!processors.TryGetValue(genericParameters[0], out proc))
                        {
                            proc = new SwapReplacementProcessor(genericParameters[0], processors);
                        }
                    }
                    else if (propertyType.IsArray)
                    {
                        var elementType = propertyType.GetElementType();
                        if (!processors.TryGetValue(elementType, out proc))
                        {
                            proc = new SwapReplacementProcessor(elementType, processors);
                        }
                    }
                }
                else
                {
                    // Тип не является коллекцией
                    if (!processors.TryGetValue(propertyType, out proc))
                    {
                        proc = new SwapReplacementProcessor(propertyType, processors);
                    }
                }
            }

            if (proc == null)
            {
                throw new InvalidOperationException(string.Format("The type '{0}' cannot be used as DependentMember.", propertyType));
            }

            processor._nestedProcessors.Add(propertyInfo.Name,
                new AttributeInfo(propertyInfo.Name,
                    new FastPropertyAccessor(targetType, propertyInfo.Name),
                    proc, propertyType));
        }


        #endregion

        #region Nested type
        class AttributeInfo
        {
            private string _name;
            private IPropertyAccessor _accesor;
            private DependentValueAttribute _valueAttribute;
            private Type _propertyType;
            private CultureDependentAttribute _targetAttribute;
            IReplacementProcessor _nestedProcessor;
            private bool _isCollection;
            private Type _elementType;

            public Type ElementType
            {
                get { return _elementType; }
                set { _elementType = value; }
            }

            public bool IsCollection
            {
                get { return _isCollection; }
                set { _isCollection = value; }
            }

            public IReplacementProcessor NestedProcessor
            {
                get { return _nestedProcessor; }
                set { _nestedProcessor = value; }
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public IPropertyAccessor Accesor
            {
                get { return _accesor; }
                set { _accesor = value; }
            }

            public DependentValueAttribute ValueAttribute
            {
                get { return _valueAttribute; }
                set { _valueAttribute = value; }
            }

            public Type PropertyType
            {
                get { return _propertyType; }
                set { _propertyType = value; }
            }

            public CultureDependentAttribute TargetAttribute
            {
                get { return _targetAttribute; }
                set { _targetAttribute = value; }
            }

            private AttributeInfo(string p, IPropertyAccessor accesor, Type propertyType)
            {
                _propertyType = propertyType;
                _name = p;
                _accesor = accesor;
                _isCollection = (typeof(IEnumerable)).IsAssignableFrom(propertyType);
                if (propertyType.IsGenericType)
                {
                    _elementType = propertyType.GetGenericArguments().FirstOrDefault();
                }
                else if (propertyType.IsArray)
                {
                    _elementType = propertyType.GetElementType();
                }
            }

            public AttributeInfo(string p, DependentValueAttribute valueAttribute, Type propertyType)
                : this(p, (IPropertyAccessor)null, propertyType)
            {
                // TODO: Complete member initialization
                _valueAttribute = valueAttribute;
            }

            public AttributeInfo(string p, CultureDependentAttribute targetAttribute, Type propertyType)
                : this(p, (IPropertyAccessor)null, propertyType)
            {
                // TODO: Complete member initialization
                this._targetAttribute = targetAttribute;
            }

            public AttributeInfo(string p, IPropertyAccessor accesor, IReplacementProcessor nestedProcessor, Type propertyType)
                : this(p, accesor, propertyType)
            {
                // TODO: Complete member initialization
                this._nestedProcessor = nestedProcessor;
            }
        }
        #endregion
    }
}
