
namespace Barebone.ViewModels.Shared.MenuItem
{
    public class MenuitemViewModelFactory
    {
        public MenuItemViewModel Create(Infrastructure.MenuItem menuItem_)
        {
            return new MenuItemViewModel()
            {
                Url = menuItem_.Url,
                Name = menuItem_.Name
            };
        }
    }
}