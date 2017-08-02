 using Infrastructure;

namespace Barebone.ViewModels.Shared.StyleSheet
{
    public class StyleSheetViewModelFactory
    {
        public StyleSheetViewModel Create (Infrastructure.StyleSheet styleSheet_)
        {
            return new StyleSheetViewModel()
            {
                Url = styleSheet_.Url
            };
        }
    }
}