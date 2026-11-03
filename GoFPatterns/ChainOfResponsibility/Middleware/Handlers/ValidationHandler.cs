namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Конкретный обработчик - проверяет тело запроса.
    public class ValidationHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            if (string.IsNullOrWhiteSpace(context.Body))
            {
                context.StatusCode = 400;
                context.Response = "Тело запроса обязательно";
                return;
            }
            next();
        }
    }
}
