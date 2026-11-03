namespace GoFPatterns.Mediator.RegistrationForm
{
    // Mediator - components notify it about their events.
    public interface IFormMediator
    {
        void Notify(FormComponent sender, string eventName);
    }
}
