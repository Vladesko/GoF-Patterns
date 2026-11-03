namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Concrete class built on top of another concrete class - partner sends CSV,
    // but has stricter validation rules: only adults with corporate e-mails.
    public class PartnerCsvCustomerImporter : CsvCustomerImporter
    {
        protected override bool Validate(Customer customer)
        {
            if (!base.Validate(customer))
                return false;

            var isPartner = customer.Age >= 18 && customer.Email.EndsWith("@partner.com");
            if (!isPartner)
                Console.WriteLine($"  skipped by partner rules: {customer.Name}");
            return isPartner;
        }
    }
}
