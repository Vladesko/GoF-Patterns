namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Handler - in this variant every handler gets the "next" delegate (like ASP.NET Core middleware).
    // Handler decides: do its part and call next, or stop the chain.
    public interface IRequestHandler
    {
        void Handle(HttpRequestContext context, Action next);
    }
}
