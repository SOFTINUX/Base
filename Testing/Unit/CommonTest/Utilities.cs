using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ExtCore.Infrastructure;

namespace CommonTest
{
    public class Utilities
    {

        public static void LoadExtensions() {
            List<Assembly> loadedAssemblies = new List<Assembly>();

            foreach (FileInfo file in new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).GetFiles("*.dll")) //loop through all dll files in directory	
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
