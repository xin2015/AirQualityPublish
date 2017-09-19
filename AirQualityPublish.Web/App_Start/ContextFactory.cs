using AirQualityPublish.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirQualityPublish.Web
{
    public class ContextFactory
    {
        private static readonly string contextKey = typeof(FluentModel).FullName;

        public static FluentModel GetContextPerRequest()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return new FluentModel();
            }
            else
            {
                FluentModel context = httpContext.Items[contextKey] as FluentModel;

                if (context == null)
                {
                    context = new FluentModel();
                    httpContext.Items[contextKey] = context;
                }

                return context;
            }
        }

        public static void Dispose()
        {
            HttpContext httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                FluentModel context = httpContext.Items[contextKey] as FluentModel;

                if (context != null)
                {
                    context.Dispose();
                    httpContext.Items[contextKey] = null;
                }
            }
        }
    }
}