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
        /// <param name="jsType_">set if script is a js module.</param>
        public Script(string url_, int position_, JsType jsType_ = JsType.JsNormal)
        {
            Url = url_;
            Position = position_;
            JsIsModule = jsType_;
        }

        /// <summary>
        /// Enum for js script type.
        /// </summary>
        public enum JsType
        {
            /// <summary>
            /// Is module javascript file
            /// </summary>
            IsModule = 0,

            /// <summary>
            /// Is nomodule javascript file
            /// </summary>
            NoModule = 1,

            /// <summary>
            /// Is normal javascript file
            /// </summary>
            JsNormal = 2
        }

        /// <summary>
        /// Gets url of script included in the page.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets a value indicating whether get if script is imported as module.
        /// </summary>
        public JsType JsIsModule { get; private set; }

        /// <summary>
        /// Gets position of the script in the list of scripts included in the page.
        /// </summary>
        public int Position { get; }
    }
}