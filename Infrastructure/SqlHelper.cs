// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using System.IO.Compression;
using ExtCore.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace Infrastructure
{
    public class SqlHelper
    {
        private readonly IStorage _storage;
        private readonly ILogger _logger;
        private string _providerCode;

        private string _connexionString;

        public SqlHelper(IStorage storage_, ILoggerFactory loggerFactory_)
        {
            _storage = storage_;
            _logger = loggerFactory_.CreateLogger(GetType().FullName);
            _connexionString = ((DbContext) _storage.StorageContext).Database.GetDbConnection().ConnectionString;
            GetProviderName();
        }

        /// <summary>
        /// Execute SQL code from an embedded resource SQL file.
        /// </summary>
        /// <param name="resourcePath_"></param>
        /// <returns></returns>
        public string ExecuteSqlResource(string resourcePath_)
        {
            // TODO
            return "not implemented";
        }

        /// <summary>
        /// Execute SQL code from a plain SQL file.
        /// </summary>
        /// <param name="filePath_"></param>
        /// <returns>Any error information, else null when no error happened</returns>
        public string ExecuteSqlFileWithTransaction(string filePath_)
        {
            if (!File.Exists(filePath_))
            {
                return $"File {filePath_} not found";
            }

            try
            {
                _logger.LogInformation("####### START CHINOOK SEED #######");
                ((DbContext)_storage.StorageContext).Database.BeginTransaction();
                ((DbContext)_storage.StorageContext).Database.ExecuteSqlCommand(File.ReadAllText(filePath_));
                ((DbContext)_storage.StorageContext).Database.CommitTransaction();
                _logger.LogInformation("####### END CHINOOK SEED #######");
            }
            catch (Exception e)
            {
                return $"Error executing SQL: {e.Message} - {e.StackTrace}";
            }
            return null;
        }

        /// <summary>
        /// Get the Entity Framework provider
        /// </summary>
        private void GetProviderName()
        {
            // list of provider: https://docs.microsoft.com/en-us/ef/core/providers/
            switch (((DbContext) _storage.StorageContext).Database.ProviderName)
            {
                case "Microsoft.EntityFrameworkCore.Sqlite" :
                    _providerCode = "SQLITE";
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer" :
                    _providerCode = "MSSQL";
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL" :
                    _providerCode = "POSTGRESQL";
                    break;
                case "Pomelo.EntityFrameworkCore.MySql" :
                    _providerCode = "MYSQL";
                    break;
                case "EntityFrameworkCore.SqlServerCompact40" :
                    _providerCode = "SQLCOMPACT4";
                    break;
                case "EntityFrameworkCore.SqlServerCompact35" :
                    _providerCode = "SQLCOMPACT35";
                    break;
                case "MySql.Data.EntityFrameworkCore" :
                    _providerCode = "MYSQL";
                    break;
                case "EntityFrameworkCore.Jet" :
                    _providerCode = "MSACCESS";
                    break;
                default:
                    throw new Exception("Unsuported provider: " + ((DbContext) _storage.StorageContext).Database.ProviderName);
            }
        }

        /// <summary>
        /// Test database connection
        /// </summary>
        /// <param name="connecString_"></param>
        /// <returns></returns>
        private string TestDbConnection(string connecString_)
        {
            if (string.Equals(_providerCode, "SQLITE", StringComparison.OrdinalIgnoreCase))
            {
                return TestSqliteConnexion(_providerCode);
            }

            throw new Exception("Default return value test db connexion not yet implemented");
        }

        /// <summary>
        /// Test sqlite database connection
        /// </summary>
        /// <param name="providerCode_"></param>
        /// <returns></returns>
        private string TestSqliteConnexion(string providerCode_)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(providerCode_);

                connection.Open();
                connection.Close();
                return "true";
            }
            catch
            {
                return "false";
            }
        }
    }
}
