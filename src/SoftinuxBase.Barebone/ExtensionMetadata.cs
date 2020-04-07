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
        bool IExtensionMetadata.IsAvailableForPermissions => true;

        /// <inheritdoc />
        public IEnumerable<StyleSheet> StyleSheets => new[]
        {
            // -- CSS Fonts
            new StyleSheet("/node_modules.wfk_opensans.opensans.css", 100),
            new StyleSheet("/node_modules.normalize.css.normalize.css", 200),
            new StyleSheet("/node_modules.font_awesome.css.all.min.css", 30),

            // -- Font Awesome

            // -- Admin LTE
            new StyleSheet("/node_modules.admin_lte.dist.css.adminlte.min.css", 410),
            new StyleSheet("/node_modules.select2.dist.css.select2.min.css", 411),
            new StyleSheet("/node_modules.select2_theme_bootstrap4.dist.select2-bootstrap.min.css", 412),

            // -- Toastr
            new StyleSheet("/node_modules.toastr.build.toastr.min.css", 417),

            // -- Codemirror
            // new StyleSheet("/node_modules.codemirror.lib.codemirror.css", 418),
            // new StyleSheet("/node_modules.codemirror.addon.hint.show-hint.css", 418),
            // new StyleSheet("/node_modules.codemirror.addon.display.fullscreen.css", 418),
            // new StyleSheet("/node_modules.codemirror.addon.search.matchesonscrollbar.css", 419),
            // new StyleSheet("/node_modules.codemirror.addon.dialog.dialog.css", 419),
            // new StyleSheet("/node_modules.codemirror.theme.dracula.css", 420),
            // new StyleSheet("/node_modules.codemirror.theme.eclipse.css", 421),
            // new StyleSheet("/node_modules.codemirror.theme.idea.css", 422),
            // new StyleSheet("/node_modules.codemirror.theme.lesser-dark.css", 423),
            // new StyleSheet("/node_modules.codemirror.theme.material.css", 424),
            // new StyleSheet("/node_modules.codemirror.theme.monokai.css", 425),
            // new StyleSheet("/node_modules.codemirror.theme.base16-dark.css", 426),
            // new StyleSheet("/node_modules.codemirror.theme.base16-light.css", 427),
            // --
            new StyleSheet("/Styles.barebone.css", 600),
            new StyleSheet("/css/Styles.css", 700)
        };

        /// <inheritdoc />
        public IEnumerable<Script> Scripts => new[]
        {
            new Script("/node_modules.jquery.dist.jquery.min.js", 100),
            new Script("/node_modules.popper.dist.js.popper.min.js", 200),
            new Script("/node_modules.select2.dist.js.select2.full.min.js", 210),
            new Script("/node_modules.bootstrap.dist.js.bootstrap.min.js", 220),
            new Script("/node_modules.jquery_validation.dist.jquery.validate.min.js", 300),
            new Script("/node_modules.jquery_validation_unobtrusive.dist.jquery.validate.unobtrusive.js", 400),
            new Script("/node_modules.js_cookie.src.js.cookie.js", 500),

            // -- Admin LTE
            new Script("/node_modules.inputmask.dist.jquery.inputmask.min.js", 600),
            new Script("/node_modules.admin_lte.dist.js.adminlte.min.js", 660),

            // -- Roastr
            new Script("/node_modules.toastr.build.toastr.min.js", 661),

            // -- Codemirror
            // new Script("/node_modules.codemirror.lib.codemirror.js",662),
            // new Script("/node_modules.codemirror.mode.sql.sql.js",663),
            // new Script("/node_modules.codemirror.addon.hint.show-hint.js",664),
            // new Script("/node_modules.codemirror.addon.hint.sql-hint.js",665),
            // new Script("/node_modules.codemirror.addon.hint.css-hint.js",666),
            // new Script("/node_modules.codemirror.addon.edit.trailingspace.js",667),
            // new Script("/node_modules.codemirror.addon.edit.matchbrackets.js",668),
            // new Script("/node_modules.codemirror.addon.edit.closebrackets.js",669),
            // new Script("/node_modules.codemirror.addon.display.fullscreen.js",670),
            // new Script("/node_modules.codemirror.addon.search.search.js",671),
            // new Script("/node_modules.codemirror.addon.search.searchcursor.js",672),
            // new Script("/node_modules.codemirror.addon.search.matchesonscrollbar.js",673),
            // new Script("/node_modules.codemirror.addon.search.match-highlighter.js",674),
            // new Script("/node_modules.codemirror.addon.selection.active-line.js",675),
            // new Script("/node_modules.codemirror.addon.dialog.dialog.js",676),
            // new Script("/node_modules.codemirror.addon.scroll.annotatescrollbar.js",677),
            // --
            new Script("/Scripts.barebone.min.js", 700, Script.JsType.IsModule),
            new Script("/Scripts.barebone_ajax.js", 701, Script.JsType.IsModule),
        };

        /// <inheritdoc />
        public IEnumerable<MenuGroup> MenuGroups => null;
    }
}