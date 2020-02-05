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
using Microsoft.OpenApi.Models;
using Serilog;
using SoftinuxBase.Security.Data.EntityFramework;
using SoftinuxBase.WebApplication;

namespace WebApplication
{
    public class Startup
    {
        private readonly string _extensionsPath;

        public Startup(IConfiguration configuration_, IHostingEnvironment hostingEnvironment_)
        {
            Configuration = configuration_;
            _extensionsPath = hostingEnvironment_.ContentRootPath + Configuration["Extensions:Path"].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        private IConfiguration Configuration { get; }

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
                options_.UseSqlite(Configuration["ConnectionStrings:Default"], o_ => o_.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            });

            // Register database-specific storage context implementation.
            services_.AddScoped<IStorageContext, ApplicationStorageContext>();

#if DEBUG
            // Register the Swagger generator, defining 1 or more Swagger documents
            services_.AddSwaggerGen(c_ =>
            {
                c_.SwaggerDoc("v1", new OpenApiInfo { Title = "Softinux Base API", Version = "v1" });
                foreach (SoftinuxBase.Infrastructure.Interfaces.IExtensionMetadata extensionMetadata in ExtCore.Infrastructure.ExtensionManager.GetInstances<SoftinuxBase.Infrastructure.Interfaces.IExtensionMetadata>())
                {
                    c_.IncludeXmlComments($"{extensionMetadata.CurrentAssemblyPath.Replace(@".dll", string.Empty)}.xml");
                }
            });
#endif

            services_.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder applicationBuilder_, IHostingEnvironment hostingEnvironment_, ILoggerFactory loggerFactory_, IConfiguration configuration_, IAntiforgery antiForgery_)
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
        }
    }
}