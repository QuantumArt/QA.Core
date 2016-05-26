﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Quantumart.QPublishing;
using Quantumart.QPublishing.Database;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Реализация подключения к БД Qp
    /// </summary>
    public class QpDbConnector : IQpDbConnector
    {
        /// <summary>
        /// Подключения к БД Qp
        /// </summary>
        public DBConnector DbConnector { get; private set; }

        /// <summary>
        /// Конструирует объект
        /// </summary>
        public QpDbConnector()
        {
            DbConnector = new DBConnector();
        }

        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="connectionName">Название подключения</param>
        public QpDbConnector(string connectionName)
        {
            string connection = ConfigurationManager.ConnectionStrings[
                connectionName].ConnectionString;
            DbConnector = new DBConnector(connection);
        }

        /// <summary>
        /// Возвращает данные контента
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="totalRecords">Общее количество строк</param>
        /// <returns></returns>
        public DataTable GetContentData(ContentDataQueryObject query, ref long totalRecords)
        {
            return DbConnector.GetContentData(query, ref totalRecords);
        }

        /// <summary>
        /// Возвращает данные контента
        /// </summary>
        /// <param name="command">Команда к БД</param>
        /// <returns></returns>
        public DataTable GetRealData(SqlCommand command)
        {
            return DbConnector.GetRealData(command);
        }

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string InstanceConnectionString
        {
            get
            {
                return DbConnector.InstanceConnectionString;
            }
        }

        /// <summary>
        /// Вовзращает имя контента
        /// </summary>
        /// <param name="contentId">Идентификатор контента</param>
        /// <returns></returns>
        public string GetContentName(int contentId)
        {
            return DbConnector.GetContentName(contentId);
        }

        /// <summary>
        /// Возвращает идентификаторы связанных записей
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="itemId">Ид. записи, для которой необходимо получить данные</param>
        /// <returns></returns>
        public string GetContentItemLinkIDs(string fieldName, int itemId)
        {
            return DbConnector.GetContentItemLinkIDs(fieldName, itemId);
        }

        /// <summary>
        /// Удаляет элемент из БД
        /// </summary>
        /// <param name="contentItemId"></param>
        public void DeleteContentItem(int contentItemId)
        {
            DbConnector.DeleteContentItem(contentItemId);
        }

        /// <summary>
        /// Возвращает идентификаторы связанных записей
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="values">Ид'ы записей, для которой необходимо получить данные</param>
        /// <returns></returns>
        public string GetContentItemLinkIDs(string fieldName, string values)
        {
            return DbConnector.GetContentItemLinkIDs(fieldName, values);
        }

        /// <summary>
        /// Реальный коннектор
        /// </summary>
        public object Connector
        {
            get { return DbConnector; }
        }
    }
}
