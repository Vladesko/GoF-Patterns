namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Concrete handler - does work before AND after the rest of the chain.
    public class LoggingHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            Console.WriteLine($"  [Log] -> {context.Path} from {context.ClientIp}");
            next();
            Console.WriteLine($"  [Log] <- {context.StatusCode} {context.Response}");
        }
    }
}
