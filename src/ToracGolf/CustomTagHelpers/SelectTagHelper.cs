using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracGolf.CustomTagHelpers
{
    /*add the following to the _ViewImports so it can be seen on all the view */
    //@addTagHelper "*, ToracGolf" <-- namespace of app

    /// <summary>
    /// This will let you build a select with a tag helper without having to bind it to a property
    /// </summary>
    [HtmlTargetElement("ToracSelect")]
    public class SelectTagHelper : TagHelper
    {

        private IHtmlEncoder encoder;
        public SelectTagHelper(IHtmlEncoder encoder)
        {
            this.encoder = encoder;
        }

        private const string HtmlAttributeName = "SelectItems";

        [HtmlAttributeName(HtmlAttributeName)]
        public IEnumerable<SelectListItem> SelectItems { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //set the tag name to a select...the attributes will carry along automatically. Otherwise grab context.AllAttributes
            output.TagName = "select";

            using (var writer = new StringWriter())
            {
                //loop through the items
                foreach (var item in SelectItems)
                {
                    //create the option
                    var option = new TagBuilder("option");

                    //set the value
                    option.MergeAttribute("value", item.Value);

                    //set the text
                    option.InnerHtml.Append(item.Text);

                    //write it to the writer
                    option.WriteTo(writer, encoder);
                }

                //now output the writer to the page
                output.Content.SetHtmlContent(writer.ToString());
            }

        }

    }
}
