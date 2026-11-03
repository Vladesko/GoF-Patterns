using GoFPatterns.Mediator.RegistrationForm.Components;

namespace GoFPatterns.Mediator.RegistrationForm
{
    // Konkreter Vermittler – das Formular. Hier stehen alle Regeln, wie sich die Komponenten gegenseitig beeinflussen.
    // Die Komponenten sind wiederverwendbar: TextBox weiß nichts über Kontrollkästchen und Schaltflächen.
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
                Console.WriteLine($"    [Formular] Feld CompanyName wird {(CompanyName.IsVisible ? "angezeigt" : "ausgeblendet")}");
            }

            if (sender == Submit && eventName == "click")
            {
                var company = IsCompany.IsChecked ? $", Firma {CompanyName.Text}" : string.Empty;
                Console.WriteLine($"    [Formular] Registriert: {Email.Text}{company}");
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
                Console.WriteLine($"    [Formular] Submit ist {(isValid ? "aktiviert" : "deaktiviert")}");
            }
        }
    }
}
