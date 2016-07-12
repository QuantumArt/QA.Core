using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Core.Data.Resolvers
{
    public abstract class ResolverBase : IXmlMappingResolver
    {
        private bool? _isStageMode;

        protected const string IsStageSiteModeKey = "Mode.IsStage";

        public abstract XmlMappingSource GetCurrentMapping();

        public abstract public XmlMappingSource GetMapping(bool isStage);

        /// <summary>
        /// Возвращает имя SQL-таблицы по типам контекста и класса контента
        /// </summary>
        /// <param name="contentType">Тип контента</param>
        /// <param name="contextType">Тип контекста</param>
        /// <returns>имя таблицы</returns>
        public string GetTableName(Type contextType, Type contentType)
        {
            if (contextType == null) throw new ArgumentNullException(nameof(contextType));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));

            var model = GetCurrentMapping()
                 .GetModel(contextType);

            if (model == null)
            {
                throw new InvalidOperationException($"The type {contextType} is not a valid QP Data Context or current mapping cannot be applied for the type supplied.");
            }

            var metaTable = model.GetTable(contentType);

            if (metaTable == null)
            {
                throw new InvalidOperationException($"The type {contentType} does not belong to qp data context.");
            }

            return metaTable.TableName;
        }

        /// <summary>
        /// Возвращает имя SQL-таблицы по типам контекста и класса контента
        /// </summary>
        /// <typeparam name="TContext">Тип контекста</typeparam>
        /// <typeparam name="TContent">Тип контента</typeparam>
        /// <returns>имя таблицы</returns>
        public string GetTableName<TContext, TContent>()
        {
            return GetTableName(typeof(TContext), typeof(TContent));
        }

        /// <summary>
        /// Режим доступа к данным live/stage
        /// </summary>
        public bool IsStageMode
        {
            get
            {
                if (_isStageMode == null)
                {
                    bool result = false;
                    if (bool.TryParse(ConfigurationManager.AppSettings[IsStageSiteModeKey] ?? string.Empty, out result))
                    {
                        return result;
                    }

                    return false;
                }

                return _isStageMode.Value;
            }

            set
            {
                _isStageMode = value;
            }
        }
    }
}
