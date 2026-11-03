namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Manejador concreto: trabaja antes Y después del resto de la cadena.
    public class LoggingHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            Console.WriteLine($"  [Log] -> {context.Path} desde {context.ClientIp}");
            next();
            Console.WriteLine($"  [Log] <- {context.StatusCode} {context.Response}");
        }
    }
}
