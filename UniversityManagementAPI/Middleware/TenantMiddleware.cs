namespace UniversityManagementAPI.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        var tenant = context.Request.Headers["Tenant"]; 

        if (string.IsNullOrEmpty(tenant))
        {
            // Default to branch_1 if no tenant header is provided
            tenant = "branch_1";
        }
        
        context.Items["Tenant"] = tenant;

        await _next(context);
    }
}