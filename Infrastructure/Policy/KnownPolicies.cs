// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;

namespace Infrastructure.Policy
{
    /// <summary>
    /// A simple in-memory storage of names of policies we registered to system.
    /// </summary>
    public static class KnownPolicies
    {
        private static readonly HashSet<string> _set = new HashSet<string>();

        public static void Add(string policyName_)
        {
            _set.Add(policyName_);
        }

        public static bool Contains(string policyName_)
        {
            return _set.Contains(policyName_);
        }
    }
}
