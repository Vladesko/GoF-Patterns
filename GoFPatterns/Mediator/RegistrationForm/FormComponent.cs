namespace GoFPatterns.Mediator.RegistrationForm
{
    // Basis-Kollege – jede UI-Komponente kennt nur den Vermittler.
    public abstract class FormComponent(string name)
    {
        public string Name { get; } = name;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        protected IFormMediator? Mediator { get; private set; }

        public void SetMediator(IFormMediator mediator) => Mediator = mediator;
    }
}
