using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        public static IWebHost BuildWebHost(string[] args_) =>
            WebHost.CreateDefaultBuilder(args_)
                .UseStartup<Startup>()
                .CaptureStartupErrors(true)
                .Build();
    }
}