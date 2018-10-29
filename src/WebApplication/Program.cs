// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SoftinuxBase.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args_) =>
            WebHost.CreateDefaultBuilder(args_)
                .UseStartup<Startup>()
                // Add the two lines below for SoftinuxBase
                .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "..", "wwwroot"))
                .CaptureStartupErrors(true);
    }
}