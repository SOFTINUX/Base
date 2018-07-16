// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;


namespace Security.ViewModels.Permissions
{
    /// <summary>
    /// The posted data when adding a new role.
    /// </summary>
    public class SaveNewRoleViewModel
    {
        public string Role {get; set;}

        public List<string> Extensions {get; set;}

        public string Permission {get; set;}
    }
}