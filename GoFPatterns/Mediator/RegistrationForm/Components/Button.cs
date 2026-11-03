namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Colega concreto: botón.
    public class Button(string name) : FormComponent(name)
    {
        public void Click()
        {
            if (!IsEnabled)
            {
                Console.WriteLine($"  {Name} está deshabilitado: no ocurre nada");
                return;
            }
            Mediator?.Notify(this, "click");
        }
    }
}
