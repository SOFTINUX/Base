// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using SoftinuxBase.Security.DataLayer;

namespace SoftinuxBase.Security.DataAuthorize
{
    public class CalcDataKey
    {
        private readonly ApplicationStorageContext _context;

        public CalcDataKey(ApplicationStorageContext context_)
        {
            _context = context_;
        }

        /// <summary>
        /// This looks for a DataKey for the current user, which can be missing
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns>The found data key, or random guid string to stop it matching anything</returns>
        public string CalcDataKeyForUser(string userId_)
        {
            // TODO CHECK THIS CODE BECAUSE IS NOT NEEDED ANYMORE
            /*
            return _context.DataAccess.Where(x => x.UserId == userId_)
                .Select(x => x.LinkedTenant.DataKey).SingleOrDefault()
                   //If no data key then set to random guid to stop it matching anything
                   ?? Guid.NewGuid().ToString("N");
            */
            throw new NotImplementedException();
        }
    }
}