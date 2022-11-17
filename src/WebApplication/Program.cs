// Copyright © 2017-2022 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.IO;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SoftinuxBase.Security.Data.EntityFramework;
using SoftinuxBase.WebApplication;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment hostingEnvironment_ = builder.Environment;

var extensionsPath = hostingEnvironment_.ContentRootPath + configuration["Extensions:Path"].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

// Add services to the container.
builder.Services.AddSoftinuxBase<ApplicationStorageContext>(builder.Configuration, extensionsPath);
builder.Services.AddDbContext<ApplicationStorageContext>(options_ =>
{
    options_.UseSqlite(builder.Configuration.GetConnectionString("Default"), sqliteOptions_ => sqliteOptions_.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
});
builder.Services.AddControllers();
builder.Services.AddScoped<IStorageContext, ApplicationStorageContext>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Softinux Base API", Version = "v1" });
    foreach (SoftinuxBase.Infrastructure.Interfaces.IExtensionMetadata extensionMetadata in ExtCore.Infrastructure.ExtensionManager.GetInstances<SoftinuxBase.Infrastructure.Interfaces.IExtensionMetadata>())
    {
        c.IncludeXmlComments($"{extensionMetadata.CurrentAssemblyPath.Replace(@".dll", string.Empty)}.xml");
    }
});

var app = builder.Build();

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
//app.UseSoftinuxBase(hostingEnvironment_, loggerFactory_, configuration_, antiForgery_);

app.Run();