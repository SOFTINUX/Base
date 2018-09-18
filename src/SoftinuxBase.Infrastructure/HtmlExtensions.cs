// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Infrastructure
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