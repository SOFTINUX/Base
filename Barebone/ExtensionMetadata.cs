using System.Collections.Generic;
using Infrastructure;

namespace Barebone
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[]
        {
            new StyleSheet("/node_modules.wfk_opensans.opensans.css", 100),
            new StyleSheet("/node_modules.normalize.css.normalize.css", 200),
            new StyleSheet("/node_modules.bootstrap.dist.css.bootstrap.min.css", 300),
            new StyleSheet("/node_modules.font_awesome.css.font-awesome.min.css", 400),
            new StyleSheet("/node_modules.font_awesome.fonts.fontawesome.otf", 410),
            new StyleSheet("/node_modules.font_awesome.fonts.fontawesome.woff", 420),
            new StyleSheet("/node_modules.font_awesome.fonts.fontawesome.woff2", 430),
            new StyleSheet("/node_modules.font_awesome.fonts.fontawesome.ttf", 440),
            new StyleSheet("/Styles.barebone.css",500)
        };

        public IEnumerable<Script> Scripts => new Script[]
        {
            new Script("/node_modules.jquery.dist.jquery.slim.min.js",100),
            new Script("/node_modules.popper.js.dist.umd.popper.min.js",200),
            new Script("/node_modules.bootstrap.dist.js.bootstrap.min.js",300),
            new Script("/node_modules.jquery_validation.dist.jquery.validate.min.js",400),
            new Script("/node_modules.jquery_validation_unobtrusive.jquery.validate.unobtrusive.js",500),
            new Script("/node_modules.js_cookie.src.js.cookie.js",600),
            new Script("/Scripts.barebone.js",700),
        };

        public IEnumerable<MenuGroup> MenuGroups => null;

    }
}
