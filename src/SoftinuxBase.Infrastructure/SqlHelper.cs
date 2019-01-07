// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Data.SqlClient;
using System.IO;
using ExtCore.Data.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace SoftinuxBase.Infrastructure
{
    public class SqlHelper
    {
        public enum ProviderCode
        {
            Sqlite,
            Mssql,
            Postgresql,
            MysqlMariadb,
            MysqlOracle,
            Sqlcompact4,
            Sqlcompact35,
            Msaccess,
        }

        private readonly IStorage _storage;
        private readonly ILogger _logger;
        private ProviderCode _providerCode;

        private string _connexionString;

        public SqlHelper(IStorage storage_, ILoggerFactory loggerFactory_)
        {
            _storage = storage_;
            _logger = loggerFactory_.CreateLogger(GetType().FullName);
            _connexionString = ((DbContext)_storage.StorageContext).Database.GetDbConnection().ConnectionString;
            GetProvider();
        }

        /// <summary>
        /// Execute SQL code from an embedded resource SQL file.
        /// </summary>
        /// <param name="resourcePath_"></param>
        /// <returns></returns>
        public string ExecuteSqlResource(string resourcePath_) =>

            // TODO
            throw new Exception("Not yet implemented");

        /// <summary>
        /// Execute sql script form internal editor.
        /// </summary>
        /// <param name="sqlScript_"></param>
        /// <returns></returns>
        public string ExecuteSqlScript(string sqlScript_)
        {
            if (string.IsNullOrWhiteSpace(sqlScript_) || string.IsNullOrEmpty(sqlScript_))
            {
                return "you sql script is empty";
            }

            throw new Exception("Not yet implemented");
        }

        /// <summary>
        /// Execute SQL code from a plain SQL file.
        /// </summary>
        /// <param name="filePath_"></param>
        /// <returns>Any error information, else null when no error happened.</returns>
        public string ExecuteSqlFileWithTransaction(string filePath_)
        {
            if (!File.Exists(filePath_))
            {
                return $"File {filePath_} not found";
            }

            try
            {
                _logger.LogInformation("####### ExecuteSqlFileWithTransaction - begin transaction #######");
                ((DbContext)_storage.StorageContext).Database.BeginTransaction();
                ((DbContext)_storage.StorageContext).Database.ExecuteSqlCommand(File.ReadAllText(filePath_));
                ((DbContext)_storage.StorageContext).Database.CommitTransaction();
                _logger.LogInformation("####### ExecuteSqlFileWithTransaction - end transaction #######");
            }
            catch (Exception e)
            {
                return $"Error executing SQL: {e.Message} - {e.StackTrace}";
            }

            return null;
        }

        /// <summary>
        /// Get the Entity Framework provider.
        /// </summary>
        /// <returns>ProviderCode.</returns>
        public ProviderCode GetProvider()
        {
            // list of provider: https://docs.microsoft.com/en-us/ef/core/providers/
            switch (((DbContext)_storage.StorageContext).Database.ProviderName)
            {
                case "Microsoft.EntityFrameworkCore.Sqlite":
                    _providerCode = ProviderCode.Sqlite;
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    _providerCode = ProviderCode.Mssql;
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    _providerCode = ProviderCode.Postgresql;
                    break;
                case "Pomelo.EntityFrameworkCore.MySql":
                    _providerCode = ProviderCode.MysqlMariadb;
                    break;
                case "EntityFrameworkCore.SqlServerCompact40":
                    _providerCode = ProviderCode.Sqlcompact4;
                    break;
                case "EntityFrameworkCore.SqlServerCompact35":
                    _providerCode = ProviderCode.Sqlcompact35;
                    break;
                case "MySql.Data.EntityFrameworkCore":
                    _providerCode = ProviderCode.MysqlOracle;
                    break;
                case "EntityFrameworkCore.Jet":
                    _providerCode = ProviderCode.Msaccess;
                    break;
                default:
                    throw new Exception("Unsuported provider: " + ((DbContext)_storage.StorageContext).Database.ProviderName);
            }

            return _providerCode;
        }

        /// <summary>
        /// Test database connection.
        /// </summary>
        /// <param name="connecString_"></param>
        /// <returns>True if call return true, else false.</returns>
        private bool TestDbConnection(string connecString_)
        {
            switch (_providerCode)
            {
                case ProviderCode.Sqlite:
                    return TestSqliteConnexion(connecString_);
                case ProviderCode.Mssql:
                    return TestMsSqlConnexion(connecString_);
                case ProviderCode.Postgresql:
                    return TestPostgresqlConnexion(connecString_);
                default:
                    throw new Exception("Database provider not yet implemented");
            }
        }

        /// <summary>
        /// Test sqlite database connection.
        /// </summary>
        /// <param name="connexionString_"></param>
        /// <returns>True if can open and close connexion, else false.</returns>
        private static bool TestSqliteConnexion(string connexionString_)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(connexionString_);

                connection.Open();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Test mssql database connection.
        /// </summary>
        /// <param name="connexionString_"></param>
        /// <returns>True if can open and close connexion, else false.</returns>
        private static bool TestMsSqlConnexion(string connexionString_)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connexionString_);

                connection.Open();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Test PostgreSql database connection.
        /// </summary>
        /// <param name="connexionString_"></param>
        /// <returns>True if can open and close connexion, else false.</returns>
        private static bool TestPostgresqlConnexion(string connexionString_)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connexionString_);

                connection.Open();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
