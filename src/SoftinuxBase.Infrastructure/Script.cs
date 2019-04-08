// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Infrastructure
{
    public class Script
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class.
        /// </summary>
        /// <param name="url_">set url to load script.</param>
        /// <param name="position_">set position of script in the list of scripts included in the page.</param>
        public Script(string url_, int position_)
        {
            Url = url_;
            Position = position_;
        }

        /// <summary>
        /// Gets url of script included in the page.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets position of the script in the list of scripts included in the page.
        /// </summary>
        public int Position { get; }
    }
}