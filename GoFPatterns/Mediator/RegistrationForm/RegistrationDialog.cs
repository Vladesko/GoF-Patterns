using GoFPatterns.Mediator.RegistrationForm.Components;

namespace GoFPatterns.Mediator.RegistrationForm
{
    // Mediador concreto: el formulario. Aquí están todas las reglas sobre cómo se afectan los componentes entre sí.
    // Los componentes son reutilizables: TextBox no sabe nada de casillas ni de botones.
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
                Console.WriteLine($"    [formulario] el campo CompanyName está {(CompanyName.IsVisible ? "visible" : "oculto")}");
            }

            if (sender == Submit && eventName == "click")
            {
                var company = IsCompany.IsChecked ? $", empresa {CompanyName.Text}" : string.Empty;
                Console.WriteLine($"    [formulario] Registrado {Email.Text}{company}");
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
                Console.WriteLine($"    [formulario] Submit está {(isValid ? "habilitado" : "deshabilitado")}");
            }
        }
    }
}
