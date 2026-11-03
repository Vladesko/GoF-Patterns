namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Конкретный коллега - флажок.
    public class CheckBox(string name) : FormComponent(name)
    {
        public bool IsChecked { get; private set; }

        public void Toggle()
        {
            IsChecked = !IsChecked;
            Console.WriteLine($"  Пользователь переключает {Name}: {(IsChecked ? "отмечен" : "не отмечен")}");
            Mediator?.Notify(this, "toggled");
        }
    }
}
