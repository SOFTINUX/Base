using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Data.Entities;

namespace Security.Data.EntityFramework
{
    public class ApplicationStorageContext : IdentityDbContext<User, Role, int>, IStorageContext
    {
        //public ApplicationStorageContext(IOptions<StorageContextOptions> options_) : base(options_)
        //{
        //}
    }
}
