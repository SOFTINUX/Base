using System.Security.Claims;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Security.Data.Entities;

namespace Security
{
    /// <summary>
    /// Overriding WIF's UserClaimsPrincipalFactory allows to add custom claims to the WIF's current user.
    /// </summary>
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        private readonly IStorage _storage;
        private readonly ILoggerFactory _loggerFactory;

        public ClaimsPrincipalFactory(
            UserManager<User> userManager_,
            RoleManager<Role> roleManager_,
            IOptions<IdentityOptions> optionsAccessor_,
            IStorage storage_,
            ILoggerFactory loggerFactory_)
            : base(userManager_, roleManager_, optionsAccessor_)
        {
            _storage = storage_;
            _loggerFactory = loggerFactory_;

        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user_)
        {
            var principal = await base.CreateAsync(user_);

            // FIXME fix the linq query using the correct table names
            // InvalidOperationException: Cannot create a DbSet for 'UserRole' because this type is not included in the model for the context.
            //new UserManager(null, _loggerFactory, _storage).AddClaims(user_, (ClaimsIdentity)principal.Identity);

            return principal;
        }
    }
}
