namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Konkreter Handler – prüft das Token und ergänzt den Kontext um den Benutzernamen.
    public class AuthenticationHandler : IRequestHandler
    {
        private readonly Dictionary<string, string> _tokens = new() { ["token-anna"] = "Anna", ["token-ivan"] = "Ivan" };

        public void Handle(HttpRequestContext context, Action next)
        {
            if (context.Token is null || !_tokens.TryGetValue(context.Token, out var user))
            {
                context.StatusCode = 401;
                context.Response = "Nicht autorisiert";
                return;
            }
            context.UserName = user;
            next();
        }
    }
}
