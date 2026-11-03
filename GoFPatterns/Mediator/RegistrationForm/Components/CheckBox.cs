namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Concrete colleague - checkbox.
    public class CheckBox(string name) : FormComponent(name)
    {
        public bool IsChecked { get; private set; }

        public void Toggle()
        {
            IsChecked = !IsChecked;
            Console.WriteLine($"  User toggles {Name}: {(IsChecked ? "checked" : "unchecked")}");
            Mediator?.Notify(this, "toggled");
        }
    }
}
