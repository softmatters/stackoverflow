using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AuthHeader.MessageHandlers
{
    /// <summary>
    /// An HttpMessageHandler that will add the authorization to the outgoing request
    /// </summary>
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly HttpContext _httpContext;

        public AuthHeaderHandler(IHttpContextAccessor contextAccessor)
        {
            // get hold of the HttpContext as we need to check if
            // the authorization header was passed in to the request
            _httpContext = contextAccessor.HttpContext;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = _httpContext?.Request.Headers["Authorization"];

            var authHeader = headers?.Any() == true ? headers.Value.First() : string.Empty;

            // check if we have authorization in the incoming request header
            if (!string.IsNullOrEmpty(authHeader))
            {
                // add it to the outgoing API request header via HttpClient
                request.Headers.Add("Authorization", $"Bearer {authHeader}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}