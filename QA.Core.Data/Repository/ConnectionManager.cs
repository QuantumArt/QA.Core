﻿using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Менеджер подключений к БД
    /// </summary>
    public class ConnectionManager
    {
        private Dictionary<string, IDbConnection> _connections =
            new Dictionary<string, IDbConnection>();

        private Dictionary<string, IDbTransaction> _transactions =
            new Dictionary<string, IDbTransaction>();

        /// <summary>
        /// Возвращает подключение к БД по имени
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public IDbConnection GetConnection(string connectionName)
        {
            Throws.IfArgumentNullOrEmpty(connectionName, _ => connectionName);

            //TODO: connection storage
            if (_connections.ContainsKey(connectionName))
            {
                return _connections[connectionName];
            }

            var conn = new SqlConnection(
                ConfigurationManager
                    .ConnectionStrings[connectionName]
                    .ConnectionString);

            _connections.Add(connectionName, conn);

            return conn;
        }

        //TODO: ?
        //public IDbTransaction BeginTransaction(IUnitOfWork unitOfWork)
        //{
        //    Throws.IfArgumentNull(unitOfWork, _ => unitOfWork);
        //    Throws.IfArgumentNullOrEmpty(unitOfWork.ConnectionName, _ => unitOfWork.ConnectionName);

        //    // TODO: smart open
        //    if (_connections[unitOfWork.ConnectionName].State != ConnectionState.Open)
        //    {
        //        _connections[unitOfWork.ConnectionName].Open();
        //    }

        //    IDbTransaction tran;
        //    if (_transactions.ContainsKey(unitOfWork.ConnectionName))
        //    {
        //        tran = _transactions[unitOfWork.ConnectionName];
        //    }
        //    else
        //    {
        //        tran = _connections[unitOfWork.ConnectionName].BeginTransaction();

        //        if (!_transactions.ContainsKey(unitOfWork.ConnectionName))
        //        {
        //            _transactions.Add(unitOfWork.ConnectionName, tran);
        //        }
        //    }

        //    if (unitOfWork.Context is DataContext)
        //    {
        //        (unitOfWork.Context as DataContext).Transaction = tran as DbTransaction;
        //    }
            
        //    //TODO: EF

        //    return tran;
        //}

        //public void CommitTransaction(IUnitOfWork unitOfWork)
        //{
        //    Throws.IfArgumentNull(unitOfWork, _ => unitOfWork);
        //    Throws.IfArgumentNullOrEmpty(unitOfWork.ConnectionName, _ => unitOfWork.ConnectionName);

        //    _transactions[unitOfWork.ConnectionName].Commit();

        //    if (_transactions.ContainsKey(unitOfWork.ConnectionName))
        //    {
        //        _transactions.Remove(unitOfWork.ConnectionName);
        //    }

        //    if (unitOfWork.Context is DataContext)
        //    {
        //        (unitOfWork.Context as DataContext).Transaction = null;
        //    }
        //}

        //public void RollbackTransaction(IUnitOfWork unitOfWork)
        //{
        //    Throws.IfArgumentNull(unitOfWork, _ => unitOfWork);
        //    Throws.IfArgumentNullOrEmpty(unitOfWork.ConnectionName, _ => unitOfWork.ConnectionName);

        //    _transactions[unitOfWork.ConnectionName].Rollback();

        //    if (_transactions.ContainsKey(unitOfWork.ConnectionName))
        //    {
        //        _transactions.Remove(unitOfWork.ConnectionName);
        //    }

        //    if (unitOfWork.Context is DataContext)
        //    {
        //        (unitOfWork.Context as DataContext).Transaction = null;
        //    }
        //}
    }
}
