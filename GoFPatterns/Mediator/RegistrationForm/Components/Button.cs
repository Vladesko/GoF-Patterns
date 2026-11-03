namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Concrete colleague - button.
    public class Button(string name) : FormComponent(name)
    {
        public void Click()
        {
            if (!IsEnabled)
            {
                Console.WriteLine($"  {Name} is disabled - nothing happens");
                return;
            }
            Mediator?.Notify(this, "click");
        }
    }
}
