namespace GoFPatterns.Mediator.RegistrationForm.Components
{
    // Конкретный коллега - кнопка.
    public class Button(string name) : FormComponent(name)
    {
        public void Click()
        {
            if (!IsEnabled)
            {
                Console.WriteLine($"  {Name} неактивна - ничего не происходит");
                return;
            }
            Mediator?.Notify(this, "click");
        }
    }
}
