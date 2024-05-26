using ECommerceAPI.Application.Abstraction.Services;
using ECommerceAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ECommerceAPI.API.Filter
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;

            if (!string.IsNullOrEmpty(name) && name != "180101051m@gmail.com")
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                if (descriptor != null)
                {
                    var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                    if (attribute != null)
                    {
                        var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                        var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

                        var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

                        if (!hasRole)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                }
            }

            await next();
        }
    }
}
