namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Konkreter Kollege – Textfeld.
    public class TextBox(string name) : FormComponent(name)
    {
        public string Text { get; private set; } = string.Empty;

        public void Type(string text)
        {
            Text = text;
            Console.WriteLine($"  Benutzer tippt in {Name}: \"{text}\"");
            Mediator?.Notify(this, "changed");
        }
    }
}
