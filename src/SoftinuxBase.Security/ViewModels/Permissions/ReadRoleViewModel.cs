using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    public class ReadRoleViewModel
    {
        public IdentityRole<string> Role { get; set; }
        public List<string> Extensions {get; set;}
    }
}