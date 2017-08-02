using System.Collections.Generic;
using Barebone.ViewModels.Shared.StyleSheet;

namespace Barebone.ViewModels.Shared.StyleSheets
{
    public class StyleSheetsViewModel
    {
        public IEnumerable<StyleSheetViewModel> StyleSheets { get; set; }
    }
}