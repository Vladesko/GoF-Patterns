namespace GoFPatterns.Mediator.RegistrationForm
{
    // Vermittler – die Komponenten benachrichtigen ihn über ihre Ereignisse.
    public interface IFormMediator
    {
        void Notify(FormComponent sender, string eventName);
    }
}
