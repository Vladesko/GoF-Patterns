namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Colleague concreto - checkbox.
    public class CheckBox(string name) : FormComponent(name)
    {
        public bool IsChecked { get; private set; }

        public void Toggle()
        {
            IsChecked = !IsChecked;
            Console.WriteLine($"  L'utente cambia {Name}: {(IsChecked ? "selezionato" : "deselezionato")}");
            Mediator?.Notify(this, "toggled");
        }
    }
}
