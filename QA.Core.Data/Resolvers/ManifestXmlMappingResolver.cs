using System;
using System.Data.Linq.Mapping;

namespace QA.Core.Data.Resolvers
{
    /// <summary>
    /// Реализует механизм получения маппингов из текущей сборки
    /// </summary>
    public abstract class ManifestXmlMappingResolver<TContext> : ResolverBase
    {
        /// <summary>
        /// Шаблон сообщения об ошибке
        /// </summary>
        protected const string NotFoundErrorFormat = "The resource is not found. Location: {0}, Searched for key: {1}";

        /// <summary>
        /// Маппинг Stage
        /// </summary>
        protected XmlMappingSource _mappingStageSource;

        /// <summary>
        /// Маппинг Live
        /// </summary>
        protected XmlMappingSource _mappingLiveSource;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ManifestXmlMappingResolver()
        {
            var assembly = typeof(TContext).Assembly;
            var path = GetManifestResourcePath(true);
            using (var stream = assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    throw new InvalidProgramException(string.Format(NotFoundErrorFormat, assembly.FullName, path));
                }

                _mappingStageSource = XmlMappingSource.FromStream(stream);
            }

            path = GetManifestResourcePath(false);

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    throw new InvalidProgramException(string.Format(NotFoundErrorFormat, assembly.FullName, path));
                }

                _mappingLiveSource = XmlMappingSource.FromStream(stream);
            }
        }

        /// <summary>
        /// Получить маппинг с учетом режима доступа к данным live/stage
        /// </summary>
        /// <param name="isStage"></param>
        /// <returns></returns>
        public override XmlMappingSource GetMapping(bool isStage)
        {
            return isStage ? _mappingStageSource : _mappingLiveSource;
        }

        /// <summary>
        /// Получить маппинг с учетом конфигурации
        /// </summary>
        /// <returns></returns>
        public override XmlMappingSource GetCurrentMapping()
        {
            return GetMapping(IsStageMode);
        }

        
        /// <summary>
        /// Получение полного пути к ресурсу.
        /// Например, "QA.Core.Site.Data.QP.Mapping.ContentContext_Stage.map"
        /// </summary>
        /// <param name="isStage"></param>
        /// <returns></returns>
        protected abstract string GetManifestResourcePath(bool isStage);
    }
}
