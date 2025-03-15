using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware
{
    public class BusinessValidationExceptionHandler(ILogger<BusinessValidationExceptionHandler> _logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BusinessValidationException validationException)
                return false;

            _logger.LogError(exception, "Ocorreu uma exceção sem tratamento");
            
            var statusCode = StatusCodes.Status400BadRequest;
            var problemDetails = new ProblemDetails
            {
                Title = "Ocorreram erros de validação ao processar a requisição",
                Detail = exception.Message,
                Status = statusCode,
                Type = exception.GetType().Name,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Extensions = { ["errors"] = validationException.Errors }
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
