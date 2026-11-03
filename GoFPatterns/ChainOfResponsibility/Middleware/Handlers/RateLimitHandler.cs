namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Konkreter Handler – stoppt die Kette, wenn der Client zu viele Anfragen sendet.
    public class RateLimitHandler(int maxRequests) : IRequestHandler
    {
        private readonly Dictionary<string, int> _requests = [];

        public void Handle(HttpRequestContext context, Action next)
        {
            _requests[context.ClientIp] = _requests.GetValueOrDefault(context.ClientIp) + 1;
            if (_requests[context.ClientIp] > maxRequests)
            {
                context.StatusCode = 429;
                context.Response = "Zu viele Anfragen";
                return; // die Kette wird unterbrochen
            }
            next();
        }
    }
}
