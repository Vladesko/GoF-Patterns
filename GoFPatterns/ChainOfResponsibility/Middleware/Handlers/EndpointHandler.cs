namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Manejador concreto: el último de la cadena. Hace el trabajo real y no llama a next.
    public class EndpointHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            context.StatusCode = 201;
            context.Response = $"Pedido creado por {context.UserName}";
        }
    }
}
