namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Конкретный обработчик - прерывает цепочку, если клиент отправляет слишком много запросов.
    public class RateLimitHandler(int maxRequests) : IRequestHandler
    {
        private readonly Dictionary<string, int> _requests = [];

        public void Handle(HttpRequestContext context, Action next)
        {
            _requests[context.ClientIp] = _requests.GetValueOrDefault(context.ClientIp) + 1;
            if (_requests[context.ClientIp] > maxRequests)
            {
                context.StatusCode = 429;
                context.Response = "Слишком много запросов";
                return; // цепочка прерывается
            }
            next();
        }
    }
}
