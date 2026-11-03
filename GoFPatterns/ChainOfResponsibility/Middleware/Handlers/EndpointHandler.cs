namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Concrete handler - the last one in the chain. Does the real work and doesn't call next.
    public class EndpointHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            context.StatusCode = 201;
            context.Response = $"Order created by {context.UserName}";
        }
    }
}
