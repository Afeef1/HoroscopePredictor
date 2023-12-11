using HoroscopePredictorApp.Services;

namespace HoroscopePredictorApp
{
    public class HttpHeaderHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public HttpHeaderHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            var token = _tokenService.GetAccessToken();
            if (token != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
