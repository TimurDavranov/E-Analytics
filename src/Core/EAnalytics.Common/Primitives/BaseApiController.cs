﻿using Microsoft.AspNetCore.Mvc;

namespace EAnalytics.Common.Primitives;

public record BaseApiResponse<T>(T? Data, bool Success = true, string? Message = null) where T : class;
[Route("[controller]/[action]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected new IActionResult Ok(object? value = null)
    {
        var response = new BaseApiResponse<object>(value);

        return base.Ok(response);
    }

    protected IActionResult Error(string message)
    {
        var response = new BaseApiResponse<object>(null) { Message = message, Success = false };
        return base.Ok(response);
    }
}
