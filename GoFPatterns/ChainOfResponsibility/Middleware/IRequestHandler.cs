namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Handler – in dieser Variante erhält jeder Handler den Delegaten "next" (wie Middleware in ASP.NET Core).
    // Der Handler entscheidet: seinen Teil erledigen und next aufrufen oder die Kette stoppen.
    public interface IRequestHandler
    {
        void Handle(HttpRequestContext context, Action next);
    }
}
