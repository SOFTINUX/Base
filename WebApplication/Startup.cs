using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
  public class Startup
  {
    private IConfigurationRoot configurationRoot;
    private string extensionsPath;

    public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
    {
      IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(hostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

      IConfigurationRoot configurationRoot = configurationBuilder.Build();

      this.extensionsPath = hostingEnvironment.ContentRootPath + configurationRoot["Extensions:Path"];

      loggerFactory.AddConsole();
      loggerFactory.AddDebug();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddExtCore(this.extensionsPath);
    }

    public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment)
    {
      if (hostingEnvironment.IsDevelopment())
      {
          applicationBuilder.UseDeveloperExceptionPage();
          //applicationBuilder.UseDatabaseErrorPage();
          //applicationBuilder.UseBrowserLink();
      }
      applicationBuilder.UseExtCore();

      System.Console.WriteLine("PID= " + System.Diagnostics.Process.GetCurrentProcess().Id);
    }
  }
}