using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QA.Core.Replacing
{
    /// <summary>
    /// Управление заменами, указанными с помощью атрибутов CultureDependent и DependentValue.
    /// Данный объект следует кешировать.
    /// </summary>
    [Obsolete("Use SwapReplacementProcessor instead.")]
    public class ReplacementProcessor : IReplacementProcessor
    {
        private readonly Dictionary<string, AttributeInfo> _targetProperties;
        private readonly Dictionary<string, AttributeInfo> _nestedProcessors;
        private readonly Dictionary<Type, IReplacementProcessor> _processed;
        private Dictionary<string, Dictionary<string, AttributeInfo>> _valueProperties;
        private bool _hasNested;

        /// <summary>
        /// Тип объекта
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="targetType"></param>
        public ReplacementProcessor(Type targetType)
            : this()
        {
            TargetType = targetType;
            BuildUp(targetType, this, _processed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="processors"></param>
        private ReplacementProcessor(Type targetType, Dictionary<Type, IReplacementProcessor> processors)
            : this()
        {
            TargetType = targetType;
            BuildUp(targetType, this, processors);
        }

        private ReplacementProcessor()
        {
            _targetProperties = new Dictionary<string, AttributeInfo>();
            _valueProperties = new Dictionary<string, Dictionary<string, AttributeInfo>>();
            _processed = new Dictionary<Type, IReplacementProcessor>();
            _nestedProcessors = new Dictionary<string, AttributeInfo>();
        }

        private static void BuildUp(Type targetType, ReplacementProcessor processor, Dictionary<Type, IReplacementProcessor> processors)
        {
            if (processors.ContainsKey(targetType))
            {
                return;
            }

            processors.Add(targetType, processor);

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

                IPropertyAccessor accesor = new FastPropertyAccessor(targetType, propertyInfo.Name);
                var propertyType = propertyInfo.PropertyType;
                if (targetAttribute != null && propertyInfo.CanWrite)
                {
                    processor._targetProperties.Add(propertyInfo.Name, new AttributeInfo(propertyInfo.Name, accesor, targetAttribute, propertyType));
                }

                if (valueAttribute != null && propertyInfo.CanRead)
                {
                    valueProperties.Add(new AttributeInfo(propertyInfo.Name, accesor, valueAttribute, propertyType));
                }

                if (memberAttribute != null)
                {
                    IReplacementProcessor proc = null;
                    processor._hasNested = true;
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
                                    proc = new ReplacementProcessor(genericParameters[0], processors);
                                }
                            }
                            else if (propertyType.IsArray)
                            {
                                var elementType = propertyType.GetElementType();
                                if (!processors.TryGetValue(elementType, out proc))
                                {
                                    proc = new ReplacementProcessor(elementType, processors);
                                }
                            }
                        }
                        else
                        {
                            // Тип не является коллекцией
                            if (!processors.TryGetValue(propertyType, out proc))
                            {
                                proc = new ReplacementProcessor(propertyType, processors);
                            }
                        }
                    }

                    if (proc == null)
                    {
                        throw new InvalidOperationException(string.Format("The type '{0}' cannot be used as DependentMember.", propertyType));
                    }

                    processor._nestedProcessors.Add(propertyInfo.Name, new AttributeInfo(propertyInfo.Name, accesor, proc, propertyType));
                }
            }

            // проверим, что типы совместимые
            // TODO: сделать проверку
            foreach (var property in processor._targetProperties)
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

            processor._valueProperties = valueProperties
                .Where(x => !processor._targetProperties.ContainsKey(x.Name))
                    .GroupBy(k => k.ValueAttribute.CultureName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.GroupBy(e => e.ValueAttribute.TargetPropertyName)
                            .ToDictionary(
                                g2 => g2.Key,
                                g2 => g2.Single())
                    );
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
                return obj;

            Dictionary<string, AttributeInfo> current = null;
            if (_valueProperties.TryGetValue(cultureKey, out current))
            {
                foreach (var property in _targetProperties)
                {
                    AttributeInfo info = null;
                    if (current.TryGetValue(property.Key, out info))
                    {
                        // теперь можно заменять
                        var value = info.Accesor.GetValue(obj);
                        property.Value.Accesor.SetValue(obj, value);
                    }
                }
            }
            if (_hasNested)
            {
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

            public AttributeInfo(string p, IPropertyAccessor accesor, DependentValueAttribute valueAttribute, Type propertyType)
                : this(p, accesor, propertyType)
            {
                // TODO: Complete member initialization
                _valueAttribute = valueAttribute;
            }

            public AttributeInfo(string p, IPropertyAccessor accesor, CultureDependentAttribute targetAttribute, Type propertyType)
                : this(p, accesor, propertyType)
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
