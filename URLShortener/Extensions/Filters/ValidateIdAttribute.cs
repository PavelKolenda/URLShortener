using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace URLShortener.Extensions.Filters;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!IsValidId(context))
        {
            context.Result = new BadRequestObjectResult("Invalid Id");
        }
    }

    private static bool IsValidId(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("id", out object? idObj))
        {
            return idObj is > 0 and < int.MaxValue;
        }

        return idObj is null;
    }
}
