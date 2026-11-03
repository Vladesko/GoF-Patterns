namespace GoFPatterns.Mediator.RegistrationForm
{
    // Mediator - i componenti gli notificano i propri eventi.
    public interface IFormMediator
    {
        void Notify(FormComponent sender, string eventName);
    }
}
