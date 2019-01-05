// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.IO;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SoftinuxBase.WebApplication;
using SoftinuxLogger;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApplication
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly string _extensionsPath;

        public Startup(IConfiguration configuration_, IHostingEnvironment hostingEnvironment_)
        {
            Configuration = configuration_;
            _extensionsPath = hostingEnvironment_.ContentRootPath + Configuration["Extensions:Path"].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        public void ConfigureServices(IServiceCollection services_)
        {
            // Note: AddScoped : for services based on EF (once per request),
            // other values : AddTransient (stateless), AddSingleton (avoids to implement singleton pattern ourselves)

            services_.AddSingleton(Configuration);

            // SoftinuxBase/db context
            services_.AddSoftinuxBase<ApplicationStorageContext>(Configuration, _extensionsPath);

            // Which database provider to use : Sqlite
            services_.AddDbContext<ApplicationStorageContext>(options_ =>
            {
                options_.UseSqlite(Configuration["ConnectionStrings:Default"]);
            });

            // Register database-specific storage context implementation.
            services_.AddScoped<IStorageContext, ApplicationStorageContext>();

            // Logging
            services_.AddSoftinuxLogger();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services_.AddSwaggerGen(c_ =>
            {
                c_.SwaggerDoc("v1", new Info { Title = "Softinux Base API", Version = "v1" });
            });

            services_.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder applicationBuilder_, IHostingEnvironment hostingEnvironment_, ILoggerFactory loggerFactory_,
         IConfiguration configuration_, IAntiforgery antiForgery_)
        {
#if DEBUG
            Log.Information("#######################################################");
            Log.Information("webroot path: " + hostingEnvironment_.WebRootPath + "\n" + "Content Root path: " + hostingEnvironment_.ContentRootPath);
            Log.Information("#######################################################");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            applicationBuilder_.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            applicationBuilder_.UseSwaggerUI(c_ =>
            {
                c_.SwaggerEndpoint("/swagger/v1/swagger.json", "Softinux Base API V1");
            });
#endif

            applicationBuilder_.UseSoftinuxBase(hostingEnvironment_, loggerFactory_, configuration_, antiForgery_);

            // Logging
            applicationBuilder_.UseSoftinuxLogger(loggerFactory_, configuration_);
        }
    }
}