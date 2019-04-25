using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AuthHeader.MessageHandlers
{
    /// <summary>
    /// An HttpMessageHandler that will add the authorization to the outgoing request
    /// </summary>
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILogger<AuthHeaderHandler> _logger;
        private readonly HttpContext _httpContext;

        public AuthHeaderHandler(IHttpContextAccessor contextAccessor, ILogger<AuthHeaderHandler> logger)
        {
            _logger = logger;
            // get hold of the HttpContext as we need to check if
            // the authorization header was passed in to the request
            _httpContext = contextAccessor.HttpContext;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_httpContext == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var headers = _httpContext.Request.Headers["Authorization"];

            var authHeader = headers.Any() ? headers.First() : string.Empty;

            // check if we have authorization in the incoming request header
            if (!string.IsNullOrEmpty(authHeader))
            {
                // log the info if needed
                _logger.LogInformation($"The request {_httpContext.Request.Path.ToUriComponent()} was called with header {authHeader}");

                // add it to the outgoing API request header via HttpClient
                request.Headers.Add("Authorization", $"Bearer {authHeader}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}