namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Handler concreto - interrompe la catena se il client invia troppe richieste.
    public class RateLimitHandler(int maxRequests) : IRequestHandler
    {
        private readonly Dictionary<string, int> _requests = [];

        public void Handle(HttpRequestContext context, Action next)
        {
            _requests[context.ClientIp] = _requests.GetValueOrDefault(context.ClientIp) + 1;
            if (_requests[context.ClientIp] > maxRequests)
            {
                context.StatusCode = 429;
                context.Response = "Troppe richieste";
                return; // la catena viene interrotta
            }
            next();
        }
    }
}
