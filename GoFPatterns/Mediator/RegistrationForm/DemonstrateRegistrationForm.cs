namespace GoFPatterns.Mediator.RegistrationForm
{
    public class DemonstrateRegistrationForm
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Mediator: formulario de registro ===\n");

            var form = new RegistrationDialog();

            form.Submit.Click();
            form.Email.Type("anna@mail.com");
            form.Password.Type("qwerty123");
            form.AcceptTerms.Toggle();
            form.IsCompany.Toggle();
            form.Submit.Click();
            form.CompanyName.Type("Cuernos y Pezuñas");
            form.Submit.Click();
        }
    }
}
