namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Конкретный обработчик - последний в цепочке. Выполняет реальную работу и не вызывает next.
    public class EndpointHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            context.StatusCode = 201;
            context.Response = $"Заказ создан пользователем {context.UserName}";
        }
    }
}
