// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ExtCore.Infrastructure;

namespace SoftinuxBase.Tests.Common
{
    public class Utilities
    {
        public static void LoadExtensions()
        {
            List<Assembly> loadedAssemblies = new List<Assembly>();

            // loop through all dll files in directory
            foreach (FileInfo file in new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).GetFiles("*.dll"))
            {
                try
                {
                    loadedAssemblies.Add(Assembly.Load(AssemblyName.GetAssemblyName(file.FullName)));
                }
                catch (Exception)
                {
                    Console.WriteLine("Error loading assembly from file: " + file.FullName);
                }
            }

            ExtensionManager.SetAssemblies(loadedAssemblies);
        }
    }
}
