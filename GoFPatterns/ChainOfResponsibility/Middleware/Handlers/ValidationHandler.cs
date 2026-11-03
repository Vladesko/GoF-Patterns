namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Handler concreto - convalida il corpo della richiesta.
    public class ValidationHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            if (string.IsNullOrWhiteSpace(context.Body))
            {
                context.StatusCode = 400;
                context.Response = "Il corpo è obbligatorio";
                return;
            }
            next();
        }
    }
}
