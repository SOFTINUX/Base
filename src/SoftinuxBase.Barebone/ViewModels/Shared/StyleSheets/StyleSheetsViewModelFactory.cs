// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Infrastructure;
using SoftinuxBase.Barebone.ViewModels.Shared.StyleSheet;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Barebone.ViewModels.Shared.StyleSheets
{
    public class StyleSheetsViewModelFactory
    {
        public StyleSheetsViewModel Create()
        {
            List<Infrastructure.StyleSheet> styleSheets = new List<Infrastructure.StyleSheet>();

            foreach (IExtensionMetadata extensionMetadata in ExtensionManager.GetInstances<IExtensionMetadata>())
                if(extensionMetadata.StyleSheets != null)
                    styleSheets.AddRange(extensionMetadata.StyleSheets);

            return new StyleSheetsViewModel()
            {
                StyleSheets = styleSheets.OrderBy(s_ => s_.Position).Select(s_ => new StyleSheetViewModelFactory().Create(s_))
            };
        }
    }
}