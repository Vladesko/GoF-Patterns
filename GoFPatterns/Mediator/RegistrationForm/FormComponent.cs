namespace GoFPatterns.Mediator.RegistrationForm
{
    // Colega base: cada componente de la interfaz solo conoce al mediador.
    public abstract class FormComponent(string name)
    {
        public string Name { get; } = name;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        protected IFormMediator? Mediator { get; private set; }

        public void SetMediator(IFormMediator mediator) => Mediator = mediator;
    }
}
