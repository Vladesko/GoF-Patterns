namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Konkreter Kollege – Schaltfläche.
    public class Button(string name) : FormComponent(name)
    {
        public void Click()
        {
            if (!IsEnabled)
            {
                Console.WriteLine($"  {Name} ist deaktiviert – nichts passiert");
                return;
            }
            Mediator?.Notify(this, "click");
        }
    }
}
