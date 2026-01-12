using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error");
            await WriteProblem(context, HttpStatusCode.BadRequest, "Validation error", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, HttpStatusCode.InternalServerError, "Server error", "Unexpected error");
        }
    }

    private static async Task WriteProblem(HttpContext ctx, HttpStatusCode code, string title, string detail)
    {
        ctx.Response.StatusCode = (int)code;
        ctx.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails
        {
            Status = (int)code,
            Title = title,
            Detail = detail,
            Instance = ctx.Request.Path
        };
        await ctx.Response.WriteAsJsonAsync(problem);
    }
}
