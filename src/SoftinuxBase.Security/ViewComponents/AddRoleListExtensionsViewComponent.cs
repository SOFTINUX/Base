// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

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
