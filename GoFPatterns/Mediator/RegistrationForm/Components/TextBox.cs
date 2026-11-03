namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Colega concreto: campo de texto.
    public class TextBox(string name) : FormComponent(name)
    {
        public string Text { get; private set; } = string.Empty;

        public void Type(string text)
        {
            Text = text;
            Console.WriteLine($"  El usuario escribe en {Name}: \"{text}\"");
            Mediator?.Notify(this, "changed");
        }
    }
}
