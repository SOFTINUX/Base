// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Infrastructure
{
    public class StyleSheet
    {
        public string Url {get; set;}
        public int Position {get;}

        public StyleSheet(string url_, int position_)
        {
            Url = url_;
            Position = position_;
        }
    }
}