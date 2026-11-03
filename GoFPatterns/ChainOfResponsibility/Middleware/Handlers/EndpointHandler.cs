namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Konkreter Handler – der letzte in der Kette. Erledigt die eigentliche Arbeit und ruft next nicht auf.
    public class EndpointHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            context.StatusCode = 201;
            context.Response = $"Bestellung erstellt von {context.UserName}";
        }
    }
}
