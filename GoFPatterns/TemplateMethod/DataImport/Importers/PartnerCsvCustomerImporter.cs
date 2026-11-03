namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Classe concreta costruita sopra un'altra classe concreta - il partner invia un CSV,
    // ma ha regole di convalida più severe: solo maggiorenni con e-mail aziendali.
    public class PartnerCsvCustomerImporter : CsvCustomerImporter
    {
        protected override bool Validate(Customer customer)
        {
            if (!base.Validate(customer))
                return false;

            var isPartner = customer.Age >= 18 && customer.Email.EndsWith("@partner.com");
            if (!isPartner)
                Console.WriteLine($"  scartato dalle regole del partner: {customer.Name}");
            return isPartner;
        }
    }
}
