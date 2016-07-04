using System.Web.Mvc;

namespace PhotoFiler.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string className = "")
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);

            if (className != "")
                builder.MergeAttribute("class", className);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}