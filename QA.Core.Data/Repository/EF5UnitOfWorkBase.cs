using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace QA.Core.Data.Repository
{
    public class EF5UnitOfWorkBase<T> : UnitOfWork<T> where T : DbContext
    {
        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        public EF5UnitOfWorkBase(string connectionString)
        {
            _connectionStringName = connectionString;
        }

        protected string _connectionStringName;

        public override string ConnectionName
        {
            get
            {
                return this._connectionStringName;
            }
        }

        /// <summary>
        /// Текущая транзакция
        /// </summary>
        protected DbTransaction _transaction;

        /// <summary>
        /// Открывает транзакцию
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public override IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            RollbackTransaction();

            // TODO: smart open
            if (Context.Database.Connection.State != ConnectionState.Open)
            {
                Context.Database.Connection.Open();
            }

            _transaction = Context.Database.Connection.BeginTransaction(
                isolationLevel);

            return _transaction;
        }

        /// <summary>
        /// Применяет изменения
        /// </summary>
        public override void Commit()
        {
            Context.SaveChanges();

            CommitTransaction();
        }

        public override void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }
        
        /// <summary>
        /// Включить изменения в транзакцию
        /// </summary>
        /// <param name="tran"></param>
        public override void AttachTransaction(IDbTransaction tran)
        {
            
        }

        /// <summary>
        /// Создает контекст
        /// </summary>
        /// <returns></returns>
        protected override T CreateContext()
        {
            var connString = ConfigurationManager
                .ConnectionStrings[_connectionStringName].ConnectionString;
            return (T)Activator.CreateInstance(typeof(T), connString);
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
        ~EF5UnitOfWorkBase() 
        {
            Dispose(false);
        }

        /// <summary>
        /// Обновляет данные
        /// </summary>
        public override void Refresh()
        {
            //TODO:
        }

        /// <summary>
        /// Отменяет транзакцию
        /// </summary>
        public override void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            CloseConnection();
        }

        /// <summary>
        /// Закрывает подключение к БД
        /// </summary>
        protected virtual void CloseConnection()
        {
            if (Context.Database.Connection.State != ConnectionState.Closed)
            {
                Context.Database.Connection.Close();
            }
        }
    }
}
