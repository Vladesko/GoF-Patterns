namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Конкретный обработчик - проверяет токен и дополняет контекст именем пользователя.
    public class AuthenticationHandler : IRequestHandler
    {
        private readonly Dictionary<string, string> _tokens = new() { ["token-anna"] = "Анна", ["token-ivan"] = "Иван" };

        public void Handle(HttpRequestContext context, Action next)
        {
            if (context.Token is null || !_tokens.TryGetValue(context.Token, out var user))
            {
                context.StatusCode = 401;
                context.Response = "Не авторизован";
                return;
            }
            context.UserName = user;
            next();
        }
    }
}
