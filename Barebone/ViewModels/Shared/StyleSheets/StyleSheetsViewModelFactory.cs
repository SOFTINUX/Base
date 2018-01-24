// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Infrastructure;
using Infrastructure.Interfaces;
using Barebone.ViewModels.Shared.StyleSheets;

namespace Barebone.ViewModels.Shared.StyleSheet
{
    public class StyleSheetsViewModelFactory
    {
        public StyleSheetsViewModel Create()
        {
            List<Infrastructure.StyleSheet> styleSheets = new List<Infrastructure.StyleSheet>();

            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
                styleSheets.AddRange(extensionMetadata.StyleSheets);

            return new StyleSheetsViewModel()
            {
                StyleSheets = styleSheets.OrderBy(ss => ss.Position).Select(ss => new StyleSheetViewModelFactory().Create(ss))
            };
        }
    }
}