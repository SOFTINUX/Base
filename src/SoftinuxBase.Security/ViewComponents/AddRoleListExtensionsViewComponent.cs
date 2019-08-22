using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SoftinuxBase.Barebone.ViewComponents;

namespace SoftinuxBase.Security.ViewComponents
{
    public class AddRoleListExtensionsViewComponent : ViewComponentBase
    {
        public AddRoleListExtensionsViewComponent(IStorage storage_) : base(storage_)
        {
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View("_AddRoleListExtensions"));
        }
    }
}
