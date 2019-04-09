// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace SoftinuxBase.Infrastructure.Util
{
    public class EfLoggerProvider : ILoggerProvider
    {
        private static readonly object LogFileWriteLock = new object();

        /// <summary>
        /// Gets the path of log file to write. Extracted from assembly executing location.
        /// </summary>
        public static string LogFilePath =>
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "entityframework.log");

        /// <summary>
        /// creater logger.
        /// </summary>
        /// <param name="categoryName_">TODO: param not used.</param>
        /// <returns>EfLogger instance object.</returns>
        public ILogger CreateLogger(string categoryName_)
        {
            return new EfLogger();
        }

        /// <summary>
        /// destroy EfLogger.
        /// </summary>
        public void Dispose()
        {
        }

        private class EfLogger : ILogger
        {
            /// <summary>
            /// Gets if level is enabled.
            /// TODO: finalize this method. Param is not used.
            /// </summary>
            /// <param name="logLevel_">loglevel value (from enum).</param>
            /// <returns>always true.</returns>
            public bool IsEnabled(LogLevel logLevel_)
            {
                return true;
            }

            /// <summary>
            /// write log in log file.
            /// </summary>
            /// <typeparam name="TState"></typeparam>
            /// <param name="logLevel_"></param>
            /// <param name="eventId_"></param>
            /// <param name="state_"></param>
            /// <param name="exception_"></param>
            /// <param name="formatter_"></param>
            public void Log<TState>(LogLevel logLevel_, EventId eventId_, TState state_, Exception exception_, Func<TState, Exception, string> formatter_)
            {
                lock (LogFileWriteLock)
                {
                    File.AppendAllText(LogFilePath, formatter_(state_, exception_));
                }
            }

            /// <summary>
            /// destroy EfLogger.
            /// </summary>
            /// <typeparam name="TState"></typeparam>
            /// <param name="state_">TODO: param is not used.</param>
            /// <returns>null.</returns>
            public IDisposable BeginScope<TState>(TState state_)
            {
                return null;
            }
        }
    }
}
