using System;
using System.Linq;
using ExtCore.Data.EntityFramework;
using Infrastructure;
using Security.Data.Entities;
using Security.Data.EntityFramework;

namespace Security
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        /// <summary>
        /// Performs database base data inserts if no data
        /// </summary>
        /// <param name="context_"></param>
        public void CheckAndInitialize(IRequestHandler context_)
        {
            // 1. credential type
            bool mustInit = CheckInsertCredentialType(context_);

            if (!mustInit)
                return;

            // 2. user
            // TODO

            // 3. credential

            // 4. permission level

            // 5. role

            // 6. group

            // 7. user-role

            // 8. group-user

            // 9. permission

            // 10. user-permission (none)

            // 11. role-permission

            // 12. group-permission (none)

            context_.Storage.Save();
            
        }

        private void InsertUser(IRequestHandler context_)
        {
            User user = new User {DisplayName = "Administrator", FirstName = "Admin", LastName = "Admin"};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context_"></param>
        /// <returns>False if database is already populated</returns>
        private bool CheckInsertCredentialType(IRequestHandler context_)
        {
            CredentialTypeRepository repo = context_.Storage.GetRepository<CredentialTypeRepository>();

            var test = repo.All().FirstOrDefault();

            if (repo.All().Any())
                return false;

            CredentialType entity = new CredentialType {Code = "email", Label = "E-mail and password"};
            repo.Create(entity);
            return true;
        }
    }
}
 