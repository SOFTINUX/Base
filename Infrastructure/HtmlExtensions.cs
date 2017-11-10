// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;

namespace Htmlextensions
{
    public static class HtmlExtensions
    {
        /*  public static HtmlString FALink(this IHtmlHelper htmlHelper, string action, string controller, string link_text, string fa_class, string btn_css_classes = "", string button_id = "", object route_values = null)
         {
             //Declare the span
             TagBuilder span = new TagBuilder("span");
             span.AddCssClass($"fa fa-{fa_class}");
             span.MergeAttribute("aria-hidden", "true");

             //Declare the anchor tag
             TagBuilder anchor = new TagBuilder("a");
             //add the href attribute to the <a> element
             if (string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action))
                 anchor.MergeAttribute("href", "#");
             else
                 anchor.MergeAttribute("href", new UrlHelper(UrlActionContext).Action(action, controller, route_values));

             //Add the <span> element and the text to the <a> element
             anchor.InnerHtml.AppendHtml($"{span} {link_text}");
             anchor.AddCssClass(btn_css_classes);
             anchor.GenerateId(button_id, "");

             //Create the helper
             return HtmlString.Create(anchor.ToString(TagRenderMode.Normal));
         } */
    }
}