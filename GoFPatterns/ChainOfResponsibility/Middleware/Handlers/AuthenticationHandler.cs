namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Concrete handler - checks token and enriches context with user name.
    public class AuthenticationHandler : IRequestHandler
    {
        private readonly Dictionary<string, string> _tokens = new() { ["token-anna"] = "Anna", ["token-ivan"] = "Ivan" };

        public void Handle(HttpRequestContext context, Action next)
        {
            if (context.Token is null || !_tokens.TryGetValue(context.Token, out var user))
            {
                context.StatusCode = 401;
                context.Response = "Unauthorized";
                return;
            }
            context.UserName = user;
            next();
        }
    }
}
