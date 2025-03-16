using Api.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Globalization;
using System.Threading.RateLimiting;

namespace Api.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin();
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    });
            });

            services.AddExceptionHandler<BusinessValidationExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddProblemDetails();

            services.AddRateLimiter(options => {
                options.RejectionStatusCode = 429;
                options.OnRejected = async (context, cancellationToken) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }

                    var problemDetails = new ProblemDetails
                    {
                        Title = "Too many requests",
                        Detail = $"Por favor, aguarde {retryAfter.TotalSeconds} segundos e tente novamente",
                        Status = StatusCodes.Status429TooManyRequests,
                        Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}"
                    };
                    await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                };
                options.AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 2;
                    options.Window = TimeSpan.FromSeconds(1);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;
                });
            });

            return services;
        }
    }
}
