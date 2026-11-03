namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Colleague concreto - campo di testo.
    public class TextBox(string name) : FormComponent(name)
    {
        public string Text { get; private set; } = string.Empty;

        public void Type(string text)
        {
            Text = text;
            Console.WriteLine($"  L'utente scrive in {Name}: \"{text}\"");
            Mediator?.Notify(this, "changed");
        }
    }
}
