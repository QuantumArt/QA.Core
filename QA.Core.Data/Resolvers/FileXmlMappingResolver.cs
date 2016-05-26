using System;
using System.Configuration;
using System.Data.Linq.Mapping;
using System.IO;
using System.Web;

namespace QA.Core.Data.Resolvers
{
    /// <summary>
    /// Получение маппингов из файлов
    /// </summary>
    public class FileXmlMappingResolver : IXmlMappingResolver
    {
        /// <summary>
        ////Маппинг Stage
        /// </summary>
        protected XmlMappingSource _mappingStageSource;

        /// <summary>
        /// Маппинг Live
        /// </summary>
        protected XmlMappingSource _mappingLiveSource;

        /// <summary>
        /// Конструктор
        /// </summary>
        public FileXmlMappingResolver(
            string stagePath, string livePath)
        {
            Throws.IfArgumentNullOrEmpty(stagePath, "stagePath");
            Throws.IfArgumentNullOrEmpty(livePath, "livePath");

            _mappingStageSource = GetFile(stagePath);

            _mappingLiveSource = GetFile(livePath);
        }

        /// <summary>
        /// Возвращает файл маппинга
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        protected virtual XmlMappingSource GetFile(
            string path)
        {
            string realPath = string.Empty;

            try
            {
                if (path.StartsWith("\\"))
                {
                    path = path.Remove(0, 1);
                }

                realPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    (HttpContext.Current == null ? "" : "bin\\"),
                    path.Replace("/", "\\"));

                Throws.IfFileNotExists(realPath);
            }
            catch
            {
                realPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                    path.Replace("/", "\\"));
            }

            Throws.IfFileNotExists(realPath);

            return XmlMappingSource.FromXml(
                File.ReadAllText(realPath));
        }

        /// <summary>
        /// Получить маппинг с учетом режима доступа к данным live/stage
        /// </summary>
        /// <param name="isStage"></param>
        /// <returns></returns>
        public virtual XmlMappingSource GetMapping(bool isStage)
        {
            return isStage ? _mappingStageSource : _mappingLiveSource;
        }

        /// <summary>
        /// Получить маппинг с учетом конфигурации
        /// </summary>
        /// <returns></returns>
        public virtual XmlMappingSource GetCurrentMapping()
        {
            return GetMapping(IsStageMode);
        }

        protected const string IsStageSiteModeKey = "Mode.IsStage";

        private bool? _isStageMode;
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
