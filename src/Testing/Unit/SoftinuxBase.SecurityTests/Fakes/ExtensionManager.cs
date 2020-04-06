// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Reflection;
using ExtCore.Infrastructure;

namespace SoftinuxBase.SecurityTests.Fakes
{
    public static class ExtensionManager
    {
        public static void Setup()
        {
            ExtCore.Infrastructure.ExtensionManager.SetAssemblies(new List<Assembly>
            {
                Assembly.LoadFrom("SoftinuxBase.Security.dll"),
                Assembly.LoadFrom("SampleExtension1.dll"),
                Assembly.LoadFrom("SampleExtension2.dll")
            });
        }
    }
}