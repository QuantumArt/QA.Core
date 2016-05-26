using System.Collections.Generic;
using System.Linq;
using Quantumart.QPublishing;
using Quantumart.QPublishing.Database;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Менеджер метаданных QP
    /// </summary>
    public class QPMetadataManager : IQPMetadataManager
    {
        #region Properties

        /// <summary>
        /// Подключение к QP
        /// </summary>
        public DBConnector DbConnection { get; private set; }

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString { get { return DbConnection == null ? string.Empty:  DbConnection.InstanceConnectionString; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public QPMetadataManager(
            string connectionString)
        {
            Throws.IfArgumentNullOrEmpty(connectionString, _ => connectionString);

            DbConnection = new DBConnector(connectionString);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает список атрибутов контента
        /// </summary>
        /// <param name="siteName">Имя сайта</param>
        /// <param name="contentName">Имя контента</param>
        /// <returns></returns>
        public virtual List<ContentAttribute> GetContentAttributes(
            string siteName,
            string contentName)
        {
            Throws.IfArgumentNullOrEmpty(siteName, _ => siteName);
            Throws.IfArgumentNullOrEmpty(contentName, _ => contentName);

            return GetContentAttributes(GetContentId(
                siteName, contentName));
        }

        /// <summary>
        /// Возвращает список атрибутов контента
        /// </summary>
        /// <param name="contentId">Идентификатор контента</param>
        /// <returns></returns>
        public virtual List<ContentAttribute> GetContentAttributes(
            int contentId)
        {
            Throws.IfArgumentNot(contentId > 0, _ => contentId);

            return DbConnection.GetContentAttributeObjects(contentId).ToList();
        }

        /// <summary>
        /// Возвращает атрибут контента
        /// </summary>
        /// <param name="siteName">Имя сайта</param>
        /// <param name="contentName">Имя контента</param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns></returns>
        public virtual ContentAttribute GetContentAttribute(
            string siteName,
            string contentName,
            string fieldName)
        {
            Throws.IfArgumentNullOrEmpty(siteName, _ => siteName);
            Throws.IfArgumentNullOrEmpty(contentName, _ => contentName);
            Throws.IfArgumentNullOrEmpty(fieldName, _ => fieldName);

            int fieldId = DbConnection.GetAttributeIdByNetName(
                GetContentId(
                    siteName,
                    contentName), fieldName);

            return DbConnection.GetContentAttributeObject(fieldId);
        }

        /// <summary>
        /// Возвращает идентификатор контента
        /// </summary>
        /// <param name="siteName">Имя сайта</param>
        /// <param name="contentName">Имя контента</param>
        /// <returns></returns>
        public virtual int GetContentId(
            string siteName,
            string contentName)
        {
            Throws.IfArgumentNullOrEmpty(siteName, _ => siteName);
            Throws.IfArgumentNullOrEmpty(contentName, _ => contentName);

            int contentId = DbConnection.GetContentId(
                GetSiteId(siteName), contentName);

            return contentId;
        }

        /// <summary>
        /// Возвращает имя контента
        /// </summary>
        /// <param name="contentId">Идентификатор контента</param>
        /// <returns></returns>
        public virtual string GetContentName(
            int contentId)
        {
            Throws.IfArgumentNot(contentId > 0, _ => contentId);

            string contentName = DbConnection.GetContentName(contentId);

            return contentName;
        }

        /// <summary>
        /// Возвращает идентификатор сайта
        /// </summary>
        /// <param name="siteName">Название сайта</param>
        /// <returns></returns>
        public virtual int GetSiteId(string siteName)
        {
            Throws.IfArgumentNullOrEmpty(siteName, _ => siteName);

            int siteId = DbConnection.GetSiteId(siteName);
            return siteId;
        }

        /// <summary>
        /// Возвращает имя сайта
        /// </summary>
        /// <param name="siteId">Ид. сайта</param>
        /// <returns></returns>
        public virtual string GetSiteName(int siteId)
        {
            Throws.IfArgumentNot(siteId > 0, _ => siteId);

            string siteName = DbConnection.GetSiteName(siteId);
            return siteName;
        }

        #endregion
    }
}
