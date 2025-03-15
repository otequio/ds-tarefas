using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware
{
    public class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> _logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not NotFoundException notFoundException)
                return false;

            _logger.LogError(exception, "Ocorreu uma exceção sem tratamento");
            var statusCode = StatusCodes.Status404NotFound;
            var problemDetails  = new ProblemDetails
            {
                Title = "Recurso não encontrado",
                Detail = notFoundException.Message,
                Status = statusCode,
                Type = exception.GetType().Name,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };
            
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
