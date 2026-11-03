namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Colleague concreto - pulsante.
    public class Button(string name) : FormComponent(name)
    {
        public void Click()
        {
            if (!IsEnabled)
            {
                Console.WriteLine($"  {Name} è disabilitato - non succede nulla");
                return;
            }
            Mediator?.Notify(this, "click");
        }
    }
}
