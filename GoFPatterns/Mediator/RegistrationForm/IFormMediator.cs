namespace GoFPatterns.Mediator.RegistrationForm
{
    // Mediador: los componentes le notifican sus eventos.
    public interface IFormMediator
    {
        void Notify(FormComponent sender, string eventName);
    }
}
