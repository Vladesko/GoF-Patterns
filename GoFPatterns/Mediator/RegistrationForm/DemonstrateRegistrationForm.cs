namespace GoFPatterns.Mediator.RegistrationForm
{
    public class DemonstrateRegistrationForm
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Mediator: Registrierungsformular ===\n");

            var form = new RegistrationDialog();

            form.Submit.Click();
            form.Email.Type("anna@mail.com");
            form.Password.Type("qwerty123");
            form.AcceptTerms.Toggle();
            form.IsCompany.Toggle();
            form.Submit.Click();
            form.CompanyName.Type("Hörner & Hufe");
            form.Submit.Click();
        }
    }
}
