namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Konkreter Handler – arbeitet vor UND nach dem Rest der Kette.
    public class LoggingHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            Console.WriteLine($"  [Log] -> {context.Path} von {context.ClientIp}");
            next();
            Console.WriteLine($"  [Log] <- {context.StatusCode} {context.Response}");
        }
    }
}
