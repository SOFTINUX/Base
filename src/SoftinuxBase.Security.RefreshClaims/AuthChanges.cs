// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Test")]

namespace SoftinuxBase.Security.RefreshClaims
{
    public class AuthChanges : IAuthChanges
    {
        /// <summary>
        /// This returns true if ticksToCompareString is null, or if its value is lower than the value in the TimeStore
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="ticksToCompareString"></param>
        /// <param name="timeStore"></param>
        /// <returns></returns>
        public bool IsOutOfDateOrMissing(string cacheKey, string ticksToCompareString, ITimeStore timeStore)
        {
            if (ticksToCompareString == null)
                //if there is no time claim then you do need to reset the claims
                return true;

            var ticksToCompare = long.Parse(ticksToCompareString);
            return IsOutOfDate(cacheKey, ticksToCompare, timeStore);
        }

        private bool IsOutOfDate(string cacheKey, long ticksToCompare, ITimeStore timeStore)
        {
            var cachedTicks = timeStore.GetValueFromStore(cacheKey);
            if (cachedTicks != null)
            {
                return ticksToCompare < cachedTicks;
            }

            // seed the database with a cache value for the key
            this.AddOrUpdate(timeStore);
            return true;

        }

        public void AddOrUpdate(ITimeStore timeStore)
        {
            timeStore.AddUpdateValue(AuthChangesConsts.FeatureCacheKey, DateTime.UtcNow.Ticks);
        }
    }
}
