// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using SoftinuxBase.Infrastructure;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Barebone
{
    public class ExtensionMetadata : IExtensionMetadata
    {
        /// <inheritdoc />
        public Assembly CurrentAssembly => Assembly.GetExecutingAssembly();

        /// <inheritdoc />
        public string CurrentAssemblyPath => CurrentAssembly.Location;

        /// <inheritdoc />
        public Type Permissions => null;

        /// <inheritdoc />
        public string Name => CurrentAssembly.GetName().Name;

        /// <inheritdoc />
        public string Url => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyTitleAttribute))?.ToString();

        /// <inheritdoc />
        public string Version => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyVersionAttribute))?.ToString();

        /// <inheritdoc />
        public string Authors => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyCompanyAttribute))?.ToString();

        /// <inheritdoc />
        public string Description => Attribute.GetCustomAttribute(CurrentAssembly, typeof(AssemblyDescriptionAttribute))?.ToString();

        /// <inheritdoc />
        public IEnumerable<StyleSheet> StyleSheets => new[]
        {
            // -- CSS Fonts
            new StyleSheet("/node_modules.wfk_opensans.opensans.css", 100),
            new StyleSheet("/node_modules.normalize.css.normalize.css", 200),
            new StyleSheet("/node_modules.font_awesome.css.all.min.css", 300),

            // -- Font Awesome

            // -- Admin LTE
            new StyleSheet("/node_modules.admin_lte.dist.css.adminlte.min.css", 410),

            // -- Select2.org
            new StyleSheet("/node_modules.select2.dist.css.select2.min.css", 411),
            new StyleSheet("/node_modules._ttskch.select2_bootstrap4_theme.dist.select2-bootstrap4.min.css", 412),

            // -- Toastr
            new StyleSheet("/node_modules.toastr.build.toastr.min.css", 417),

            // --
            new StyleSheet("/Styles.barebone.css", 600),
            new StyleSheet("/css/Styles.css", 700)
        };

        /// <inheritdoc />
        public IEnumerable<Script> Scripts => new[]
        {
            new Script("/node_modules.jquery.dist.jquery.min.js", 100),
            new Script("/node_modules.popper.dist.js.popper.min.js", 200),
            new Script("/node_modules.bootstrap.dist.js.bootstrap.min.js", 220),
            new Script("/node_modules.jquery_validation.dist.jquery.validate.min.js", 300),
            new Script("/node_modules.jquery_validation_unobtrusive.dist.jquery.validate.unobtrusive.js", 400),
            new Script("/node_modules.js_cookie.dist.cookie.js", 500),

            // -- Admin LTE
            new Script("/node_modules.inputmask.dist.jquery.inputmask.min.js", 600),
            new Script("/node_modules.admin_lte.dist.js.adminlte.min.js", 660),

            // -- Select2
            new Script("/node_modules.select2.dist.js.select2.full.min.js", 661),

            // -- Roastr
            new Script("/node_modules.toastr.build.toastr.min.js", 662),

            // -- Codemirror
            // new Script("/node_modules.codemirror.state.index.js",662),
            // new Script("/node_modules.codemirror.view.index.js",663),
            // new Script("/node_modules.codemirror.language.index.js",664),
            // new Script("/node_modules.codemirror.search.index.js",665),
            // new Script("/node_modules.codemirror.commands.index.js",666),
            // new Script("/node_modules.codemirror.lint.index.js",667),
            // new Script("/node_modules.codemirror.autocomplete.index.js",668),

            // --
            new Script($"/Scripts.barebone{FileExtensionPrefix}.js", 700, Script.JsType.IsModule)
        };

        /// <inheritdoc />
        public IEnumerable<MenuGroup> MenuGroups => null;

        /// <inheritdoc />
        public string FileExtensionPrefix
        {
            get
            {
#if DEBUG
                return string.Empty;
#else
                return ".min";
#endif
            }
        }
    }
}