using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    internal class RedisCacheAttribute(int durationInSec = 1800) : ActionFilterAttribute /*Attribute, IAsyncActionFilter*/
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            //Create Cache Key
            string cacheKey = CreateCacheKey(context.HttpContext.Request);

            //Search with Key
            var cacheValue = await cacheService.GetAsync(cacheKey);

            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult res)
            {
                await cacheService.SetAsync(cacheKey, res.Value!, TimeSpan.FromSeconds(durationInSec));
            }
        }
        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(request.Path + "?");

            foreach (var item in request.Query.OrderBy(q => q.Key))
                builder.Append($"{item.Key}={item.Value}&");

            return builder.ToString().Trim('&');
        }
    }
}
