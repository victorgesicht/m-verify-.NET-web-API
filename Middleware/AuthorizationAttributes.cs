using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace m_verify_BE.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireApiKeyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var key))
            {
                context.Result = new UnauthorizedObjectResult(new { error = "API key required" });
                return;
            }
            var apiKey = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("ApiKey");
            if (string.IsNullOrWhiteSpace(apiKey) || !apiKey.Equals(key))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
