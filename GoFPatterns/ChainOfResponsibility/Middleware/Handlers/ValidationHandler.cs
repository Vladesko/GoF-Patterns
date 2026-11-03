namespace GoFPatterns.ChainOfResponsibility.Middleware.Handlers
{
    // Manejador concreto: valida el cuerpo de la petición.
    public class ValidationHandler : IRequestHandler
    {
        public void Handle(HttpRequestContext context, Action next)
        {
            if (string.IsNullOrWhiteSpace(context.Body))
            {
                context.StatusCode = 400;
                context.Response = "El cuerpo es obligatorio";
                return;
            }
            next();
        }
    }
}
