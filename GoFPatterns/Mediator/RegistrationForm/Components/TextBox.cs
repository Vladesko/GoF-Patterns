namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Конкретный коллега - текстовое поле.
    public class TextBox(string name) : FormComponent(name)
    {
        public string Text { get; private set; } = string.Empty;

        public void Type(string text)
        {
            Text = text;
            Console.WriteLine($"  Пользователь вводит в {Name}: \"{text}\"");
            Mediator?.Notify(this, "changed");
        }
    }
}
