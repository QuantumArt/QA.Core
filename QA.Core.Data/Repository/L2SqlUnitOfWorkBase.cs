using System;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
#pragma warning disable 1591


namespace QA.Core.Data.Repository
{
    public partial class L2SqlUnitOfWorkBase<T> : UnitOfWork<T> where T : DataContext
    {
        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public L2SqlUnitOfWorkBase(string connectionString) :
            this(connectionString, null)
        {
        }

        [Obsolete("Use ConnectionString")]
        protected string _connectionStringName;


        protected string ConnectionString;

        protected IXmlMappingResolver _mappingSource;
        protected string _siteName;

        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="mappingSource">Источник маппинга</param>
        public L2SqlUnitOfWorkBase(string connectionString, IXmlMappingResolver mappingSource)
        {
            ConnectionString = connectionString;
            _mappingSource = mappingSource;

            OnCreated();
        }

        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="mappingSource">Источник маппинга</param>
        /// <param name="siteName">Имя сайта</param>
        public L2SqlUnitOfWorkBase(string connectionString, string siteName, IXmlMappingResolver mappingSource)
        {
            ConnectionString = connectionString;
            _mappingSource = mappingSource;
            _siteName = siteName;

            OnCreated();
        }

        public override string ConnectionName
        {
            get
            {
                return this.ConnectionString;
            }
        }

        protected virtual void OnCreated()
        {
        }

        /// <summary>
        /// Сохраняем изменения. Применяет транзакицю.
        /// </summary>
        public override void Commit()
        {
            if (IsContextCreated)
            {
                Context.SubmitChanges();
                //CommitTransaction();
            }
        }

        /// <summary>
        /// Создает контекст
        /// </summary>
        /// <returns></returns>
        protected override T CreateContext()
        {
            return (T)Activator.CreateInstance(typeof(T),
                ConnectionString,
                _mappingSource);
        }

        /// <summary>
        /// Освобождает ресурсы
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Освобождает ресурсы
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RollbackTransaction();
            }
        }

        /// <summary>
        /// Освобождает ресурсы
        /// </summary>
        ~L2SqlUnitOfWorkBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Обновляет данные из БД
        /// </summary>
        public override void Refresh()
        {
            if (IsContextCreated)
            {
                Context.Refresh(
                    RefreshMode.OverwriteCurrentValues,
                    Context.GetChangeSet().Updates);

                Context.Refresh(
                    RefreshMode.OverwriteCurrentValues,
                    Context.GetChangeSet().Inserts);

                Context.Refresh(
                    RefreshMode.OverwriteCurrentValues,
                    Context.GetChangeSet().Deletes);
            }
        }

        /// <summary>
        /// Текущая транзакция
        /// </summary>
        protected DbTransaction transaction;

        /// <summary>
        /// Открывает новую транзакцию. При наличии транзакции откатывает ее.
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public override IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            RollbackTransaction();

            // TODO: smart open
            if (Context.Connection.State != ConnectionState.Open)
            {
                Context.Connection.Open();
            }

            transaction = Context.Connection.BeginTransaction(
                isolationLevel);
            Context.Transaction = transaction;

            return transaction;
        }

        /// <summary>
        /// Включить изменения в транзакцию
        /// </summary>
        /// <param name="tran"></param>
        public override void AttachTransaction(IDbTransaction tran)
        {
            Context.Transaction = tran as DbTransaction;
        }

        /// <summary>
        /// Применяет текущую транзакцию.
        /// </summary>
        public override void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                Context.Transaction = null;
                transaction = null;
            }

            CloseConnection();
        }

        /// <summary>
        /// Откатывает текущую транзакцию
        /// </summary>
        public override void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                Context.Transaction = null;
                transaction = null;
            }

            CloseConnection();
        }

        /// <summary>
        /// Закрывает подключение
        /// </summary>
        protected virtual void CloseConnection()
        {
            if (Context.Connection.State != ConnectionState.Closed)
            {
                Context.Connection.Close();
            }
        }
    }
}
