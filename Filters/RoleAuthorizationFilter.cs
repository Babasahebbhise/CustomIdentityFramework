using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class RoleAuthorizationFilter : IAuthorizationFilter
{
    private readonly string _role;

    public RoleAuthorizationFilter(string role)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            // Redirect to login if the user is not authenticated
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (!user.IsInRole(_role))
        {
            // Handle unauthorized access based on roles
            context.Result = new ViewResult
            {
                ViewName = "AccessDenied" // You can customize this view
            };
        }
    }
}
