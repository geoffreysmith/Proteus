using System;
using System.Web;
using Sitecore.Web;

namespace Proteus
{
    //http://www.partechit.nl/en/blog/2012/11/return-404-status-code-when-itemnotfound-page-is-loaded
    public class FourOhFourStatusCodeFix : Sitecore.Pipelines.HttpRequest.ExecuteRequest
    {
        protected override void RedirectOnItemNotFound(string url)
        {
            var context = HttpContext.Current;

            try
            {
                // Request the NotFound page
                var domain = context.Request.Url.GetComponents(UriComponents.Scheme | UriComponents.Host, UriFormat.Unescaped);
                var content = WebUtil.ExecuteWebPage(string.Concat(domain, url));

                // Send the NotFound page content to the client with a 404 status code
                context.Response.TrySkipIisCustomErrors = true;
                context.Response.StatusCode = 404;
                context.Response.Write(content);
            }
            catch
            {
                // If our plan fails for any reason, fall back to the base method
                base.RedirectOnItemNotFound(url);
            }

            // Must be outside the try/catch, cause Response.End() throws an exception
            context.Response.End();
        }
    }
}
