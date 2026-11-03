namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Manejador concreto: detiene la cadena si el cliente envía demasiadas peticiones.
    public class RateLimitHandler(int maxRequests) : IRequestHandler
    {
        private readonly Dictionary<string, int> _requests = [];

        public void Handle(HttpRequestContext context, Action next)
        {
            _requests[context.ClientIp] = _requests.GetValueOrDefault(context.ClientIp) + 1;
            if (_requests[context.ClientIp] > maxRequests)
            {
                context.StatusCode = 429;
                context.Response = "Demasiadas peticiones";
                return; // la cadena se interrumpe
            }
            next();
        }
    }
}
