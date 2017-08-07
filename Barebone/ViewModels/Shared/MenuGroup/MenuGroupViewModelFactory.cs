using System.Collections.Generic;
using Barebone.ViewModels.Shared.MenuItem;

namespace Barebone.ViewModels.Shared.MenuGroup
{
    public class MenuGroupViewModelFactory : ViewModelFactoryBase
    {

        public MenuGroupViewModelFactory(IRequestHandler requestHandler)
            : base(requestHandler)
        {
        }

        public MenuGroupViewModel Create(Infrastructure.MenuGroup menuGroup_)
        {
            return new MenuGroupViewModel()
            {
                Name = menuGroup_.Name,
                Position = menuGroup_.Position
            };
        }
    }
}