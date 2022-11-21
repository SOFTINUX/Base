// Copyright © 2017-2022 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.IO;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SoftinuxBase.Security.Data.EntityFramework;
using SoftinuxBase.WebApplication;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Staging,

    // Look for static files in webroot
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "wwwroot")
});

IConfiguration configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment hostingEnvironment = builder.Environment;

var extensionsPath = hostingEnvironment.ContentRootPath + configuration["Extensions:Path"].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

// Add services to the container.

// Note: AddScoped : for services based on EF (once per request),
// other values : AddTransient (stateless), AddSingleton (avoids to implement singleton pattern ourselves)
builder.Services.AddSingleton(configuration);

// SoftinuxBase/db context
builder.Services.AddSoftinuxBase<ApplicationStorageContext>(builder.Configuration, extensionsPath);

// Which database provider to use : Sqlite
builder.Services.AddDbContext<ApplicationStorageContext>(options_ =>
{
    options_.UseSqlite(builder.Configuration.GetConnectionString("Default"), sqliteOptions_ => sqliteOptions_.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
});

// Register database-specific storage context implementation.
builder.Services.AddScoped<IStorageContext, ApplicationStorageContext>();

builder.Services.AddControllers();

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Softinux Base API", Version = "v1" });
    foreach (SoftinuxBase.Infrastructure.Interfaces.IExtensionMetadata extensionMetadata in ExtCore.Infrastructure.ExtensionManager.GetInstances<SoftinuxBase.Infrastructure.Interfaces.IExtensionMetadata>())
    {
        c.IncludeXmlComments($"{extensionMetadata.CurrentAssemblyPath.Replace(@".dll", string.Empty)}.xml");
    }
});

// Wait 30 seconds for graceful shutdown.
builder.Host.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));

var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
var antiForgery = app.Services.GetService<IAntiforgery>();

loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSoftinuxBase(hostingEnvironment, loggerFactory, configuration, antiForgery);

app.Run();