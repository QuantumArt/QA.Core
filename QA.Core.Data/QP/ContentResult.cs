using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Quantumart.QPublishing;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Результат выборки из QP
    /// </summary>
    public class ContentResult
    {
        #region Properties

        /// <summary>
        /// Основной контент
        /// </summary>
        public DataTable PrimaryContent { get; internal set; }

        /// <summary>
        /// Список контентов
        /// </summary>
        public Dictionary<string, DataTable> Contents { get; private set; }

        /// <summary>
        /// Запрос к QP
        /// </summary>
        internal ContentDataQueryObject Query { get; set; }

        /// <summary>
        /// Подключение к QP
        /// </summary>
        internal IQpDbConnector DbConnection { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает контент по имени
        /// </summary>
        /// <param name="contentName"></param>
        /// <returns></returns>
        public DataTable GetContent(
            string contentName)
        {
            Throws.IfArgumentNullOrEmpty(contentName, "contentName");

            if (Contents == null || Contents.Count == 0)
            {
                return null;
            }

            if (Contents.ContainsKey(contentName))
            {
                return Contents[contentName];
            }

            return null;
        }

        /// <summary>
        /// Добавляет контент в результат
        /// </summary>
        /// <param name="contentName">Название контента</param>
        /// <param name="content">Содержимое контента</param>
        internal void AddContent(
            string contentName, DataTable content)
        {
            if (Contents == null)
            {
                Contents = new Dictionary<string, DataTable>();
            }

            if (!Contents.ContainsKey(contentName))
            {
                Contents.Add(contentName, content);
            }
        }

        /// <summary>
        /// Возвращает запись контента
        /// </summary>
        /// <param name="contentName">Название контента</param>
        /// <param name="id">Идентификтаор записи</param>
        /// <returns></returns>
        public DataRow GetContentRowById(string contentName, int id)
        {
            if (!Contents.ContainsKey(contentName))
            {
                return null;
            }

            return Contents[contentName].Select("CONTENT_ITEM_ID = " + id).FirstOrDefault();
        }

        /// <summary>
        /// Возвращает записи, связанные с указанной записью
        /// </summary>
        /// <param name="fieldName">Имя reference поля</param>
        /// <param name="id">Идентификатор записи, для которой нужно выбрать связанные записи</param>
        /// <returns></returns>
        public List<DataRow> GetReferenceRows(string fieldName, int id)
        {
            if (Contents == null || Contents.Count == 0)
            {
                return null;
            }

            if (!PrimaryContent.Columns.Contains(fieldName))
            {
                return null;
            }

            throw new NotImplementedException();

            var metadata = new QPMetadataManager(/*Query.ConnectionString*/null);

            var attr = metadata.GetContentAttribute(
                Query.SiteName,
                Query.ContentName,
                fieldName);

            string referenceIds = string.Empty;

            if (attr.LinkId == null)
            {
                var item = PrimaryContent.Select("CONTENT_ITEM_ID = " + id).FirstOrDefault();
                if (item != null && item.Table.Columns.Contains(fieldName))
                {
                    referenceIds = item[fieldName].ToString();
                }
            }
            else
            {
                referenceIds = DbConnection.GetContentItemLinkIDs(attr.Name, id);
            }

            if (!string.IsNullOrEmpty(referenceIds))
            {
                string contentName = metadata.GetContentName(attr.RelatedContentId.Value);

                return Contents[contentName].Select("CONTENT_ITEM_ID in (" + referenceIds + ")").ToList();
            }

            return new List<DataRow>();
        }

        #endregion
    }
}
