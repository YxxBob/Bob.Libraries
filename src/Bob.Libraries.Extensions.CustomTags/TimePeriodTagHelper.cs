using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bob.Libraries.Extensions.CustomTags
{
    [HtmlTargetElement("timeperiod")]
    public class TimePeriodTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var now = DateTime.Now;
            string content = string.Empty;

            if (now.Hour >= 5 && now.Hour < 9)
            {
                content = "早上";
            }
            else if (now.Hour >= 9 && now.Hour < 11)
            {
                content = "上午";
            }
            else if (now.Hour >= 11 && now.Hour < 13)
            {
                content = "中午";
            }
            else if (now.Hour >= 13 && now.Hour < 17)
            {
                content = "下午";
            }
            else
            {
                content = "晚上";
            }
            output.Content.SetContent(content);
        }
    }
}
