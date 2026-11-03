namespace GoFPatterns.Mediator.RegistrationForm
{
    public class DemonstrateRegistrationForm
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Посредник: форма регистрации ===\n");

            var form = new RegistrationDialog();

            form.Submit.Click();
            form.Email.Type("anna@mail.com");
            form.Password.Type("qwerty123");
            form.AcceptTerms.Toggle();
            form.IsCompany.Toggle();
            form.Submit.Click();
            form.CompanyName.Type("Рога и копыта");
            form.Submit.Click();
        }
    }
}
