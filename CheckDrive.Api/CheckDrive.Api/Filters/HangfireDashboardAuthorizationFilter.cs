using Hangfire.Dashboard;

namespace CheckDrive.Api.Filters;

internal sealed class HangfireDashboardAuthorizationFilter(string username, string password) : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();

        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            var encodedCredentials = authHeader["Basic ".Length..].Trim();
            var decodedCredentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = decodedCredentials.Split(':', 2);

            if (parts.Length == 2 && parts[0] == username && parts[1] == password)
            {
                return true;
            }
        }

        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers.WWWAuthenticate = "Basic realm=\"Hangfire Dashboard\"";

        return false;
    }
}
