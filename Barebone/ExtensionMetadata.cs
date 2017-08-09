using System.Collections.Generic;
using Infrastructure;

namespace Barebone
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new StyleSheet[]
        {
            new StyleSheet("//fonts.googleapis.com/css?family=Open+Sans", 100),
            new StyleSheet("//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css", 200),
            new StyleSheet("//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css", 210),
            new StyleSheet("style.barebone.css",400)
        };

        public IEnumerable<Script> Scripts => new Script[]
        {
            new Script("//ajax.aspnetcdn.com/ajax/jquery/jquery-1.11.3.min.js",100),
            new Script("//ajax.aspnetcdn.com/ajax/jquery.validate/1.14.0/jquery.validate.min.js",200),
            new Script("//ajax.aspnetcdn.com/ajax/jquery.validation.unobtrusive/3.2.6/jquery.validate.unobtrusive.min.js",300),
            new Script("//cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js",400),
            new Script("//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js",500),
            new Script("script.barebone.js",600),
        };

        public IEnumerable<MenuGroup> MenuGroups => null;
    }
}
