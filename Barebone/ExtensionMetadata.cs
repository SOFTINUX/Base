using System.Collections.Generic;
using Infrastructure;

namespace Barebone
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[]
        {
            new StyleSheet("//fonts.googleapis.com/css?family=Open+Sans", 100),
            new StyleSheet("/bower_components.bootstrap.dist.css.bootstrap.min.css", 200),
            new StyleSheet("/bower_components.bootstrap.dist.css.bootstrap-theme.min.css", 210),
            new StyleSheet("/Styles.barebone.css",400)
        };

        public IEnumerable<Script> Scripts => new Script[]
        {
            new Script("//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.2.0.min.js",100),
            new Script("//ajax.aspnetcdn.com/ajax/jquery.validate/1.16.0/jquery.validate.min.js",200),
            new Script("//ajax.aspnetcdn.com/ajax/jquery.validation.unobtrusive/3.2.6/jquery.validate.unobtrusive.min.js",300),
            new Script("//cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js",400),
            new Script("/bower_components.bootstrap.dist.js.bootstrap.min.js",500),
            new Script("/Scripts.barebone.js",600),
        };

        public IEnumerable<MenuGroup> MenuGroups => null;
    }
}
