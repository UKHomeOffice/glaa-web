using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GLAA.Web.Core.Helpers
{
    [HtmlTargetElement("span", Attributes = MessageValueAttributeName)]
    public class TagHelpers : TagHelper
    {
        private const string MessageValueAttributeName = "asp-message-value";

        [HtmlAttributeName(MessageValueAttributeName)]
        public string MessageValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.AppendHtml($@"{MessageValue}");
        }
    }
}
