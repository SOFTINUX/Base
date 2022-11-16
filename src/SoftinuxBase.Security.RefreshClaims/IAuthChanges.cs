// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace SoftinuxBase.Security.RefreshClaims
{
    public interface IAuthChanges
    {
        /// <summary>
        /// This returns true if there is no ticksToCompare or the ticksToCompare is earlier than the AuthLastUpdated time
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="ticksToCompareString"></param>
        /// <param name="timeStore">Link to the DbContext that managers the cache store</param>
        /// <returns></returns>
        bool IsOutOfDateOrMissing(string cacheKey, string ticksToCompareString, ITimeStore timeStore);

        /// <summary>
        /// This adds or updates the TimeStore entry with the cacheKey with the cachedValue (datetime as ticks) 
        /// </summary>
        /// <param name="timeStore"></param>
        void AddOrUpdate(ITimeStore timeStore);
    }
}