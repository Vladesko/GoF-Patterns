namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Colega concreto: casilla de verificación.
    public class CheckBox(string name) : FormComponent(name)
    {
        public bool IsChecked { get; private set; }

        public void Toggle()
        {
            IsChecked = !IsChecked;
            Console.WriteLine($"  El usuario cambia {Name}: {(IsChecked ? "marcada" : "desmarcada")}");
            Mediator?.Notify(this, "toggled");
        }
    }
}
