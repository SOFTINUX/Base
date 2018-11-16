// Copyright © 2017 SOFTINUX. All rights reserved.
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

        public static string LogFilePath =>
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "entityframework.log");

        public ILogger CreateLogger(string categoryName_)
        {
            return new EfLogger();
        }

        public void Dispose()
        { }

        private class EfLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel_)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel_, EventId eventId_, TState state_, Exception exception_, Func<TState, Exception, string> formatter_)
            {
                lock (LogFileWriteLock)
                {
                    File.AppendAllText(LogFilePath, formatter_(state_, exception_));
                }
            }

            public IDisposable BeginScope<TState>(TState state_)
            {
                return null;
            }
        }
    }
}
