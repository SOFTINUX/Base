// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Infrastructure;

namespace Barebone.ViewModels.Shared.StyleSheet
{
    public class StyleSheetViewModelFactory
    {
        public StyleSheetViewModel Create (Infrastructure.StyleSheet styleSheet_)
        {
            return new StyleSheetViewModel()
            {
                Url = styleSheet_.Url
            };
        }
    }
}