namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Обработчик - в этом варианте каждый обработчик получает делегат "next" (как middleware в ASP.NET Core).
    // Обработчик решает: выполнить свою часть и вызвать next или остановить цепочку.
    public interface IRequestHandler
    {
        void Handle(HttpRequestContext context, Action next);
    }
}
