namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Handler concreto - verifica il token e arricchisce il contesto con il nome utente.
    public class AuthenticationHandler : IRequestHandler
    {
        private readonly Dictionary<string, string> _tokens = new() { ["token-anna"] = "Anna", ["token-ivan"] = "Ivan" };

        public void Handle(HttpRequestContext context, Action next)
        {
            if (context.Token is null || !_tokens.TryGetValue(context.Token, out var user))
            {
                context.StatusCode = 401;
                context.Response = "Non autorizzato";
                return;
            }
            context.UserName = user;
            next();
        }
    }
}
