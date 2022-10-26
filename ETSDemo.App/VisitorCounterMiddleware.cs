using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETSDemo.App
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IMemoryCache _cache;

        public VisitorCounterMiddleware(RequestDelegate requestDelegate, IMemoryCache cache)
        {
            _requestDelegate = requestDelegate;
            _cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            string visitorId = context.Request.Cookies["VisitorId"];
            if (visitorId == null)
            {
                //don the necessary staffs here to save the count by one

                context.Response.Cookies.Append("VisitorId", Guid.NewGuid().ToString(), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });

                var visitorCount = 0;
                _cache.TryGetValue(Constants.VisitorCountKey, out visitorCount);
                visitorCount++;
                _cache.Set(Constants.VisitorCountKey, visitorCount);
            }

            await _requestDelegate(context);
        }
    }
}
