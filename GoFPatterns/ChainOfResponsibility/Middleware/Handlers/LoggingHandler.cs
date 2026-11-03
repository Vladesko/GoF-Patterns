namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Конкретный обработчик - выполняет работу до И после остальной части цепочки.
    public class LoggingHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            Console.WriteLine($"  [Лог] -> {context.Path} от {context.ClientIp}");
            next();
            Console.WriteLine($"  [Лог] <- {context.StatusCode} {context.Response}");
        }
    }
}
