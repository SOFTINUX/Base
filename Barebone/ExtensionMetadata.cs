// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Barebone
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        public IEnumerable<StyleSheet> StyleSheets => new[]
        {
            new StyleSheet("/node_modules.wfk_opensans.opensans.css", 100),
            new StyleSheet("/node_modules.normalize.css.normalize.css", 200),
            new StyleSheet("/node_modules.bootstrap.dist.css.bootstrap.min.css", 300),
            new StyleSheet("/node_modules.bootstrap.dist.css.bootstrap.min.css.map", 310),
            new StyleSheet("/node_modules.font_awesome.css.font-awesome.min.css", 400),
            // -- Admin LTE
            new StyleSheet("/node_modules.admin_lte.dist.css.AdminLTE.min.css", 410),
            new StyleSheet("/node_modules.admin_lte.dist.css.adminlte.min.css.map", 411),
            new StyleSheet("/node_modules.admin_lte.dist.css.skins.skin-blue.min.css", 412),
            new StyleSheet("/node_modules.ionicons.dist.css.ionicons.min.css", 413),
            new StyleSheet("/node_modules.ionicons.dist.css.ionicons.min.css.map", 414),
            new StyleSheet("/node_modules.icheck.skins.square.blue.css", 415),
            new StyleSheet("/node_modules.icheck.skins.minimal.blue.css", 416),
            new StyleSheet("/node_modules.icheck.skins.flat.blue.css", 417),
            new StyleSheet("/node_modules.icheck.skins.line.blue.css", 418),
            // -- Toastr
            new StyleSheet("/node_modules.toastr.build.toastr.min.css", 417),
            // --
            new StyleSheet("/Styles.barebone.css",500),
            new StyleSheet("/css/Styles.css",600)
        };

        public IEnumerable<Script> Scripts => new[]
        {
            new Script("/node_modules.jquery.dist.jquery.min.js",100),
            new Script("/node_modules.bootstrap.dist.js.bootstrap.min.js",200),
            new Script("/node_modules.jquery_validation.dist.jquery.validate.min.js",300),
            new Script("/node_modules.jquery_validation_unobtrusive.dist.jquery.validate.unobtrusive.js",400),
            new Script("/node_modules.js_cookie.src.js.cookie.js",500),
            // -- Admin LTE
            new Script("/node_modules.inputmask.dist.min.jquery.inputmask.bundle.min.js",600),
            new Script("/node_modules.icheck.icheck.js",650),
            new Script("/node_modules.admin_lte.dist.js.adminlte.min.js",660),
            // -- Roastr
            new Script("/node_modules.toastr.build.toastr.min.js",661),
            // --
            new Script("/Scripts.barebone.js",700),
        };

        public IEnumerable<MenuGroup> MenuGroups => null;

    }
}
