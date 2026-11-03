using GoFPatterns.Mediator.RegistrationForm.Components;

namespace GoFPatterns.Mediator.RegistrationForm
{
    // Mediator concreto - il modulo. Qui si trovano tutte le regole su come i componenti si influenzano a vicenda.
    // I componenti sono riutilizzabili: TextBox non sa nulla di checkbox e pulsanti.
    public class RegistrationDialog : IFormMediator
    {
        public TextBox Email { get; } = new("Email");
        public TextBox Password { get; } = new("Password");
        public CheckBox IsCompany { get; } = new("IsCompany");
        public TextBox CompanyName { get; } = new("CompanyName");
        public CheckBox AcceptTerms { get; } = new("AcceptTerms");
        public Button Submit { get; } = new("Submit");

        public RegistrationDialog()
        {
            FormComponent[] components = [Email, Password, IsCompany, CompanyName, AcceptTerms, Submit];
            foreach (var component in components)
            {
                component.SetMediator(this);
            }
            CompanyName.IsVisible = false;
            UpdateSubmitState();
        }

        public void Notify(FormComponent sender, string eventName)
        {
            if (sender == IsCompany && eventName == "toggled")
            {
                CompanyName.IsVisible = IsCompany.IsChecked;
                Console.WriteLine($"    [modulo] il campo CompanyName è {(CompanyName.IsVisible ? "visibile" : "nascosto")}");
            }

            if (sender == Submit && eventName == "click")
            {
                var company = IsCompany.IsChecked ? $", azienda {CompanyName.Text}" : string.Empty;
                Console.WriteLine($"    [modulo] Registrato {Email.Text}{company}");
                return;
            }

            UpdateSubmitState();
        }

        private void UpdateSubmitState()
        {
            var isValid = Email.Text.Contains('@')
                          && Password.Text.Length >= 6
                          && AcceptTerms.IsChecked
                          && (!IsCompany.IsChecked || CompanyName.Text.Length > 0);

            if (Submit.IsEnabled != isValid)
            {
                Submit.IsEnabled = isValid;
                Console.WriteLine($"    [modulo] Submit è {(isValid ? "abilitato" : "disabilitato")}");
            }
        }
    }
}
