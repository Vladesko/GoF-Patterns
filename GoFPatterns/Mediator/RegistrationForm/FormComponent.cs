namespace GoFPatterns.Mediator.RegistrationForm
{
    // Base colleague - every UI component knows only about mediator.
    public abstract class FormComponent(string name)
    {
        public string Name { get; } = name;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        protected IFormMediator? Mediator { get; private set; }

        public void SetMediator(IFormMediator mediator) => Mediator = mediator;
    }
}
