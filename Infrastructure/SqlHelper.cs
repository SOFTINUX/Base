// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using ExtCore.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class SqlHelper
    {
        private readonly IStorage _storage;
        private readonly ILogger _logger;
        private string _provider;

        public SqlHelper(IStorage storage_, ILoggerFactory loggerFactory_)
        {
            _storage = storage_;
            _logger = loggerFactory_.CreateLogger(GetType().FullName);
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

        private string GetProviderName()
        {
            // list of provider: https://docs.microsoft.com/en-us/ef/core/providers/
            switch (((DbContext) _storage.StorageContext).Database.ProviderName)
            {
                case "Microsoft.EntityFrameworkCore.Sqlite" :
                    _provider = "SQLITE";
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer" :
                    _provider = "MSSQL";
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL" :
                    _provider = "POSTGRESQL";
                    break;
                case "Pomelo.EntityFrameworkCore.MySql" :
                    _provider = "MYSQL";
                    break;
                case "EntityFrameworkCore.SqlServerCompact40" :
                    _provider = "SQLCOMPACT4";
                    break;
                case "EntityFrameworkCore.SqlServerCompact35" :
                    _provider = "SQLCOMPACT35";
                    break;
                case "MySql.Data.EntityFrameworkCore" :
                    _provider = "MYSQL";
                    break;
                case "EntityFrameworkCore.Jet" :
                    _provider = "MSACCESS";
                    break;
                default:
                    throw new Exception("Unsuported provider: " + ((DbContext) _storage.StorageContext).Database.ProviderName);
            }
            return _provider;
        }

        private void TestDbConnection(string provider_, string connecString_)
        {
            GetProviderName();
        }
    }
}
