namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Konkrete Klasse auf Basis einer anderen konkreten Klasse – der Partner sendet CSV,
    // hat aber strengere Validierungsregeln: nur Volljährige mit Firmen-E-Mail-Adresse.
    public class PartnerCsvCustomerImporter : CsvCustomerImporter
    {
        protected override bool Validate(Customer customer)
        {
            if (!base.Validate(customer))
                return false;

            var isPartner = customer.Age >= 18 && customer.Email.EndsWith("@partner.com");
            if (!isPartner)
                Console.WriteLine($"  durch Partnerregeln übersprungen: {customer.Name}");
            return isPartner;
        }
    }
}
