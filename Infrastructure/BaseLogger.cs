using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure
{
    public class BaseLogger
    {
        private static ILoggerFactory _Factory = null;
        private static IConfiguration _Configuration = null;

        public static void ConfigureLogger(ILoggerFactory factory_, IConfiguration configuration_)
        {
            _Configuration = configuration_;
            factory_.AddConsole(configuration_.GetSection("Logging"));
            factory_.AddDebug(LogLevel.None);
        }

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                    _Configuration = _Configuration.GetSection("Logging");
                    ConfigureLogger(_Factory, _Configuration);
                }
                return _Factory;
            }
            set { _Factory = value; }
        }

        public static Microsoft.Extensions.Logging.ILogger CreateLogger() => LoggerFactory.CreateLogger("test");
    }
}