namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Handler - in questa variante ogni handler riceve il delegate "next" (come i middleware di ASP.NET Core).
    // L'handler decide: fare la sua parte e chiamare next, oppure interrompere la catena.
    public interface IRequestHandler
    {
        void Handle(HttpRequestContext context, Action next);
    }
}
