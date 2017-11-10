// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Barebone.ViewModels.Shared.StyleSheet;

namespace Barebone.ViewModels.Shared.StyleSheets
{
    public class StyleSheetsViewModel
    {
        public IEnumerable<StyleSheetViewModel> StyleSheets { get; set; }
    }
}