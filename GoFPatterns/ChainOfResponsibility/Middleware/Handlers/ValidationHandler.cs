namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Concrete handler - validates request body.
    public class ValidationHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            if (string.IsNullOrWhiteSpace(context.Body))
            {
                context.StatusCode = 400;
                context.Response = "Body is required";
                return;
            }
            next();
        }
    }
}
