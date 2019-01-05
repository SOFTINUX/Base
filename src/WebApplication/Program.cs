// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args_)
        {
            CreateWebHostBuilder(args_).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args_) =>
            WebHost.CreateDefaultBuilder(args_)
                .UseStartup<Startup>()
                .UseKestrel(c_ => c_.AddServerHeader = false) // Remove the server headers from the kestrel server, by using the UseKestrel extension method.
                // Add the two lines below for SoftinuxBase
                .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "..", "wwwroot"))
                .CaptureStartupErrors(true);
    }
}