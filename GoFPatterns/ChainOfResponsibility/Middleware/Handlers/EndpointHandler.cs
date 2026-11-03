namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Handler concreto - l'ultimo della catena. Esegue il lavoro reale e non chiama next.
    public class EndpointHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            context.StatusCode = 201;
            context.Response = $"Ordine creato da {context.UserName}";
        }
    }
}
