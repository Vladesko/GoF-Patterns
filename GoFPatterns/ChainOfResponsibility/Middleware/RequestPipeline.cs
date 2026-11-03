namespace GoFPatterns.ChainOfResponsibility.Middleware
{
    // Construye la cadena a partir de la lista de manejadores. Los manejadores no se conocen entre sí,
    // solo reciben el delegado "next".
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
