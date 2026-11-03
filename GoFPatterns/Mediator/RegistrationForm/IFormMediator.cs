namespace GoFPatterns.Mediator.RegistrationForm
{
    // Посредник - компоненты уведомляют его о своих событиях.
    public interface IFormMediator
    {
        void Notify(FormComponent sender, string eventName);
    }
}
