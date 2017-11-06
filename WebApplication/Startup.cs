using System.IO;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.Data.EntityFramework.Sqlite;
using ExtCore.WebApplication.Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security;
using Serilog;
using Serilog.Events;

namespace WebApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private string _extensionsPath;

        public Startup(IConfiguration configuration_, IHostingEnvironment hostingEnvironment_)
        {
            Configuration = configuration_;
            _extensionsPath = hostingEnvironment_.ContentRootPath + Configuration["Extensions:Path"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile("log-{Date}.txt")
                .CreateLogger();

#if DEBUG
            Log.Information("#######################################################");
            Log.Information("webroot path: " + hostingEnvironment_.WebRootPath + "\n" + "Content Root path: " + hostingEnvironment_.ContentRootPath);
            Log.Information("#######################################################");
            /* var temp = "webroot path: " + hostingEnvironment_.WebRootPath + "\n" + "Content Root path: " + hostingEnvironment_.ContentRootPath;
            System.Console.WriteLine("#######################################################");
            System.Console.WriteLine(temp);
            System.Console.WriteLine("#######################################################"); */
        }
#endif
        public void ConfigureServices(IServiceCollection services_)
        {
            services_.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            // Note: AddScoped : for services based on EF (once per request),
            // other values : AddTransient (stateless), AddSingleton (avoids to implement singleton pattern ourselves)

            services_.AddSingleton<IConfiguration>(Configuration);
            services_.Configure<StorageContextOptions>(options_ =>
                {
                    options_.ConnectionString = Configuration["ConnectionStrings:Default"];
                }
            );

            services_.Configure<BaseApplicationConfiguration>(Configuration.GetSection("Corporate"));

            // Register database-specific storage context implementation.
            // Necessary for IStorage service registration to fully work (see AddAuthorizationPolicies).
            services_.AddScoped<IStorageContext, StorageContext>();
            services_.AddScoped<IStorage, Storage>();

            services_.AddExtCore(_extensionsPath);

            services_.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            //services_.AddScoped<BaseLogger>();

        }

        public void Configure(IApplicationBuilder applicationBuilder_, IHostingEnvironment hostingEnvironment_)
        {
            if (hostingEnvironment_.IsDevelopment())
            {
                applicationBuilder_.UseDeveloperExceptionPage();
                //applicationBuilder.UseDatabaseErrorPage();
                //applicationBuilder.UseBrowserLink();
            }
            applicationBuilder_.UseExtCore();

            System.Console.WriteLine("PID= " + System.Diagnostics.Process.GetCurrentProcess().Id);
        }
    }
}