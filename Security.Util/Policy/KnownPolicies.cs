using System.Collections.Generic;

namespace Security.Util.Policy
{
    /// <summary>
    /// A simple in-memory storage of names of policies we registered to system.
    /// </summary>
    public static class KnownPolicies
    {
        private static HashSet<string> _set = new HashSet<string>();

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
