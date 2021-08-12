/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace DocIt
{
    public partial class _Default : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            /* Change the current path so that the Routing handler can correctly interpret
             * the request, then restore the original path so that the OutputCache module
             * can correctly process the response (if caching is enabled).
             */

            string originalPath = Request.Path;
            HttpContext.Current.RewritePath(Request.ApplicationPath, false);
            IHttpHandler httpHandler = new MvcHttpHandler();
            httpHandler.ProcessRequest(HttpContext.Current);
            HttpContext.Current.RewritePath(originalPath, false);
        }
    }
}