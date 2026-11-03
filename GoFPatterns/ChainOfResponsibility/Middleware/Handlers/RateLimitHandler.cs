namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Concrete handler - stops the chain if client sends too many requests.
    public class RateLimitHandler(int maxRequests) : IRequestHandler
    {
        private readonly Dictionary<string, int> _requests = [];

        public void Handle(HttpRequestContext context, Action next)
        {
            _requests[context.ClientIp] = _requests.GetValueOrDefault(context.ClientIp) + 1;
            if (_requests[context.ClientIp] > maxRequests)
            {
                context.StatusCode = 429;
                context.Response = "Too many requests";
                return; // chain is interrupted
            }
            next();
        }
    }
}
