using System;
using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SqlHelper
    {
        private IStorageContext _context;

        public SqlHelper(IStorageContext context_)
        {
            _context = context_;
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
        public string ExecuteSqlFile(string filePath_)
        {
            if (!File.Exists(filePath_))
            {
                return $"File {filePath_} not found";
            }

            try
            {
                ((StorageContextBase)_context).Database.ExecuteSqlCommand(File.ReadAllText(filePath_));
            }
            catch (Exception e)
            {
                return $"Error executing SQL: {e.Message} - {e.StackTrace}";
            }
            return null;
        }

    }
}
