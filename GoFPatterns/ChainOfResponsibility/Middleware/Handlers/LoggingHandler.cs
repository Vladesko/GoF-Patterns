namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Handler concreto - lavora prima E dopo il resto della catena.
    public class LoggingHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            Console.WriteLine($"  [Log] -> {context.Path} da {context.ClientIp}");
            next();
            Console.WriteLine($"  [Log] <- {context.StatusCode} {context.Response}");
        }
    }
}
