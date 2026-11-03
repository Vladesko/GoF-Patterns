namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Concrete colleague - text field.
    public class TextBox(string name) : FormComponent(name)
    {
        public string Text { get; private set; } = string.Empty;

        public void Type(string text)
        {
            Text = text;
            Console.WriteLine($"  User types into {Name}: \"{text}\"");
            Mediator?.Notify(this, "changed");
        }
    }
}
