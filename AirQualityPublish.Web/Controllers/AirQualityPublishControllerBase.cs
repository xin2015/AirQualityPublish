using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Telerik.OpenAccess;

namespace AirQualityPublish.Web.Controllers
{
    public class AirQualityPublishControllerBase : Controller
    {
        protected OpenAccessContext Context { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Context = ContextFactory.GetContextPerRequest();
        }

        protected override void Dispose(bool disposing)
        {
            ContextFactory.Dispose();
            base.Dispose(disposing);
        }
    }
}