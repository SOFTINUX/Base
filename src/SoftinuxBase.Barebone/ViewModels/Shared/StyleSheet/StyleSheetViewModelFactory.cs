// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Barebone.ViewModels.Shared.StyleSheet
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