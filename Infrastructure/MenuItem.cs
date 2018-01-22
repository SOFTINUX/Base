// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure
{
    public class MenuItem
    {
        public string Url {get; set;}
        public string Name {get; set;}
        public uint Position {get;}

        /// <summary>
        /// The fa-xxx class to render the associated icon
        /// </summary>
        public string FontAwesomeClass {get;}

        //TODO here place properties for menu access permissions
        //public IEnumerable<string> PermissionCodes { get; set; }

        //public MenuItem(string url_, string name_, int position_, IEnumerable<string> permissionCodes_)
        public MenuItem(string url_, string name_, uint position_, string fontAwesomeClass_ = "fa-circle-o")
        {
            Url = url_;
            Name = name_;
            Position = position_;
            FontAwesomeClass = fontAwesomeClass_;
            // PermissionCodes = permissionCodes_;
        }
    }

}