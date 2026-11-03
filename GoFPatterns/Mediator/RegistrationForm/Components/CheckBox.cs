namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Konkreter Kollege – Kontrollkästchen.
    public class CheckBox(string name) : FormComponent(name)
    {
        public bool IsChecked { get; private set; }

        public void Toggle()
        {
            IsChecked = !IsChecked;
            Console.WriteLine($"  Benutzer schaltet {Name} um: {(IsChecked ? "aktiviert" : "deaktiviert")}");
            Mediator?.Notify(this, "toggled");
        }
    }
}
