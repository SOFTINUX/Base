// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Infrastructure
{
    public class CorporateConfiguration
    {
        /// <summary>
        /// Gets or sets corporation name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets corporation logo.
        /// </summary>
        public string BrandLogo { get; set; }

        /// <summary>
        /// Gets or sets login background picture.
        /// </summary>
        public string LoginBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can self register.
        /// </summary>
        public bool RegisterNewUsers { get; set; }
    }
}