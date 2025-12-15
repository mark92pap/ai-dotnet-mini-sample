using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AiMiniSample.Common;

public static class Extensions
{
    public static ActionResult ToWebResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value!)
            : new NotFoundObjectResult(result.Error);
    }

    public static IActionResult ToWebResult(this Result result)
    {
        return result.IsSuccess
            ? new NoContentResult()
            : new NotFoundObjectResult(result.Error);
    }
}