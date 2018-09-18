// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SoftinuxBase.Infrastructure
{
    public class Logging
    {
        private static ILoggerFactory _factory;
        private static IConfiguration _configuration;

        /// <summary>
        /// Configure a factory that was created either by .NET Core in Startup.cs or by a call to Logging.LoggerFactory to get a standalone singleton.
        /// </summary>
        /// <param name="factory_"></param>
        /// <param name="configuration_"></param>
        public static void ConfigureLogger(ILoggerFactory factory_, IConfiguration configuration_)
        {
            _configuration = configuration_;
            // standard console configuration
            factory_.AddConsole(configuration_.GetSection("Logging"));
            // standard log level configuration
            factory_.AddDebug(LogLevel.Debug);
            // serilog configuration that uses appsettings.json (not app settings in web.config because this is for .net 4.5, not .net standard/core).
            factory_.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration_).CreateLogger(), true);
        }

        /// <summary>
        /// LoggerFactory of the application. When set, it means that we're associating logger factory created by .NET Core in Startup.cs with this class.
        /// Else it's a classical singleton.
        /// </summary>
        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_factory != null)
                    return _factory;
                _factory = new LoggerFactory();
                ConfigureLogger(_factory, _configuration);
                return _factory;
            }
            set { _factory = value; }
        }
    }
}