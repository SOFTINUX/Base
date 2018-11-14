using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Entities;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    public class RoleObject
    {
        public RoleObject(IdentityRole<string> role_)
        {
            RoleNormalizedName = role_.NormalizedName;
            ConcurrencyStamp = role_.ConcurrencyStamp;
        }

        public RoleObject(IdentityRole<string> role_, List<User> users_)
        {
            RoleNormalizedName = role_.NormalizedName;
            ConcurrencyStamp = role_.ConcurrencyStamp;
            Users = users_;
        }

        public RoleObject(IdentityRole<string> role_, List<string> extensions_)
        {
            RoleNormalizedName = role_.NormalizedName;
            ConcurrencyStamp = role_.ConcurrencyStamp;
            Extensions = extensions_;
        }

        public RoleObject(IdentityRole<string> role_, List<string> extensions_, List<User> users_)
        {
            RoleNormalizedName = role_.NormalizedName;
            ConcurrencyStamp = role_.ConcurrencyStamp;
            Extensions = extensions_;
            Users = users_;
        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleNormalizedName { get; }
        public string ConcurrencyStamp { get; }
        public List<Permission> Permissions { get; set; }
        public List<string> Extensions { get; set; }
        public List<Security.Data.Entities.User> Users { get; }
    }
}
