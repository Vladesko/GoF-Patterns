namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Konkreter Handler – validiert den Body der Anfrage.
    public class ValidationHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            if (string.IsNullOrWhiteSpace(context.Body))
            {
                context.StatusCode = 400;
                context.Response = "Body ist erforderlich";
                return;
            }
            next();
        }
    }
}
