namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Строит цепочку из списка обработчиков. Обработчики ничего не знают друг о друге,
    // они получают только делегат "next".
    public class RequestPipeline
    {
        private readonly List<IRequestHandler> _handlers = [];

        public RequestPipeline Use(IRequestHandler handler)
        {
            _handlers.Add(handler);
            return this;
        }

        public void Execute(HttpRequestContext context) => Invoke(0, context);

        private void Invoke(int index, HttpRequestContext context)
        {
            if (index >= _handlers.Count)
                return;

            _handlers[index].Handle(context, () => Invoke(index + 1, context));
        }
    }
}
