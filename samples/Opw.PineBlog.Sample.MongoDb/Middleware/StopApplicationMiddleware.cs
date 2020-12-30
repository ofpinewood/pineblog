using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Opw.PineBlog.Sample.MongoDb.Middleware
{
    public class StopApplicationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger<StopApplicationMiddleware> _logger;
        private readonly string _restartPath;

        public StopApplicationMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            IHostApplicationLifetime applicationLifetime,
            ILogger<StopApplicationMiddleware> logger)
        {
            _next = next;
            _applicationLifetime = applicationLifetime;
            _logger = logger;
            _restartPath = configuration.GetValue<string>("StopApplicationPath");
        }

        public async Task Invoke(HttpContext context)
        {
            if (string.Equals(context.Request.Path, _restartPath, StringComparison.OrdinalIgnoreCase))
            {
                var message = $"Application stopped at {DateTime.UtcNow}.";
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                await context.Response.WriteAsync(message);

                _logger.LogInformation(message);

                await Task.Delay(500);

                _applicationLifetime.StopApplication();
                return;
            }

            await _next(context);
        }
    }
}
