namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Manejador: en esta variante cada manejador recibe el delegado "next" (como el middleware de ASP.NET Core).
    // El manejador decide: hacer su parte y llamar a next, o detener la cadena.
    public interface IRequestHandler
    {
        void Handle(HttpRequestContext context, Action next);
    }
}
