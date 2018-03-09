// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure.Enums
{
    public class ClaimType
    {
        public const string Permission = "Permission";
        // TODO créer une claim par groupe et une par rôle (le type existe dans ClaimsTypes de MS)
        // (de la même façon que c'est fait pour les permissions (par notre ClaimsManager)).
        // A faire dans ClaimsManager.
        public const string Group = "Group";
    }
}
