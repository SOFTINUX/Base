using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }

        public void ConfigureServices(IServiceCollection services_)
        {
            
            services_.AddExtCore(_extensionsPath);
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