using Infrastructure;

namespace Barebone.ViewModels.Shared.MenuItem
{
    public class MenuItemViewModelFactory : ViewModelFactoryBase
    {
        public MenuItemViewModelFactory(IRequestHandler requestHandler)
            : base(requestHandler)
        {
        }
        public MenuItemViewModel Create(Infrastructure.MenuItem menuItem_)
        {
            return new MenuItemViewModel()
            {
                Url = menuItem_.Url,
                Name = menuItem_.Name,
                Position = menuItem_.Position
            };
        }
    }
}