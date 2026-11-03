namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Manejador concreto: comprueba el token y completa el contexto con el nombre de usuario.
    public class AuthenticationHandler : IRequestHandler
    {
        private readonly Dictionary<string, string> _tokens = new() { ["token-anna"] = "Anna", ["token-ivan"] = "Ivan" };

        public void Handle(HttpRequestContext context, Action next)
        {
            if (context.Token is null || !_tokens.TryGetValue(context.Token, out var user))
            {
                context.StatusCode = 401;
                context.Response = "No autorizado";
                return;
            }
            context.UserName = user;
            next();
        }
    }
}
