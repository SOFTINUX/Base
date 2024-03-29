﻿// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using ExtCore.Data.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace SoftinuxBase.Infrastructure
{
    public class SqlHelper
    {
        /// <summary>
        /// Type of Sql Provider.
        /// </summary>
        public enum ProviderCode
        {
            /// <summary>
            /// SqLite.
            /// </summary>
            Sqlite,

            /// <summary>
            /// MS Sql.
            /// </summary>
            Mssql,

            /// <summary>
            /// PostgreSql.
            /// </summary>
            Postgresql,

            /// <summary>
            /// MySql or MariaDB.
            /// </summary>
            MysqlMariadb,

            /// <summary>
            /// Oracle.
            /// </summary>
            MysqlOracle,

            /// <summary>
            /// MsSql Server Compact version 4.0.
            /// </summary>
            Sqlcompact4,

            /// <summary>
            /// MsSql Server Compact version 3.5.
            /// </summary>
            Sqlcompact35,

            /// <summary>
            /// MS Access.
            /// </summary>
            Msaccess,
        }

        /// <summary>
        /// Storage interface provided by services container.
        /// </summary>
        private readonly IStorage _storage;

        /// <summary>
        /// Logger factory interface provided by services container.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Type of database provider.
        /// </summary>
        private ProviderCode _providerCode;

        /// <summary>
        /// Connection string to database.
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Execute SQL code from an embedded resource SQL file.
        /// TODO make this code and replace at good place.
        /// </summary>
        /// <param name="resourcePath_">internal ressource path.</param>
        /// <returns>Return a <see cref="string"/> result of sql execution.</returns>
        public string ExecuteSqlResource(string resourcePath_) => throw new Exception("Not yet implemented");

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlHelper"/> class.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="loggerFactory_">Logger factory interface provided by services container.</param>
        public SqlHelper(IStorage storage_, ILoggerFactory loggerFactory_)
        {
            _storage = storage_;
            _logger = loggerFactory_.CreateLogger(GetType().FullName);
            _connectionString = ((DbContext)_storage.StorageContext).Database.GetDbConnection().ConnectionString;
            GetProvider();
        }

        /// <summary>
        /// Gets the Entity Framework provider.
        /// </summary>
        /// <returns>Return a <see cref="ProviderCode" />.</returns>
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
        /// Execute sql script string.
        /// </summary>
        /// <param name="sqlScript_">sql script to execute.</param>
        /// <returns>Return a <see cref="ExecuteSqlScript"/> result of sql script execution>.</returns>
        public string ExecuteSqlScript(string sqlScript_)
        {
            if (string.IsNullOrWhiteSpace(sqlScript_) || string.IsNullOrEmpty(sqlScript_))
            {
                return "your sql script is empty";
            }

            throw new Exception("Not yet implemented");
        }

        /// <summary>
        /// Execute SQL code from a plain SQL file.
        /// </summary>
        /// <param name="filePath_">Path of the file to execute.</param>
        /// <returns>Return a <see cref="ExecuteSqlFileWithTransaction"/>. Any error information, else null when no error happened.</returns>
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
                var affectedRows = ((DbContext)_storage.StorageContext).Database.ExecuteSqlRaw(File.ReadAllText(filePath_));
                ((DbContext)_storage.StorageContext).Database.CommitTransaction();
                _logger.LogInformation($"####### Affected row(s) : {affectedRows} #######");
                _logger.LogInformation("####### ExecuteSqlFileWithTransaction - end transaction #######");
            }
            catch (Exception e)
            {
                return $"Error executing SQL: {e.Message} - {e.StackTrace}";
            }

            return null;
        }

        /// <summary>
        /// Test sqlite database connection.
        /// </summary>
        /// <param name="connectionString_">the connection string to sqlite.</param>
        /// <returns>Return a <see cref="bool"/>. True if can open and close connection, else false.</returns>
        private static bool TestSqliteConnection(string connectionString_)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(connectionString_);

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
        /// <param name="connectionString_">the connection string to MsSql database server.</param>
        /// <returns>Return a <see cref="bool"/>. True if can open and close connection, else false.</returns>
        private static bool TestMsSqlConnection(string connectionString_)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString_);

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
        /// <param name="connectionString_">the connection string to PostgreSql database server.</param>
        /// <returns>Return a <see cref="bool"/>. True if can open and close connection, else false.</returns>
        private static bool TestPostgresqlConnection(string connectionString_)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString_);

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
        /// Test database connection on SqLite, MsSql, PostgreSql.
        /// </summary>
        /// <param name="connecString_">the connection string to database server.</param>
        /// <returns>Return a <see cref="bool"/>. True if call return true, else false.</returns>
        private bool TestDbConnection(string connecString_)
        {
            switch (_providerCode)
            {
                case ProviderCode.Sqlite:
                    return TestSqliteConnection(connecString_);
                case ProviderCode.Mssql:
                    return TestMsSqlConnection(connecString_);
                case ProviderCode.Postgresql:
                    return TestPostgresqlConnection(connecString_);
                default:
                    throw new Exception("Database provider not yet implemented");
            }
        }
    }
}
