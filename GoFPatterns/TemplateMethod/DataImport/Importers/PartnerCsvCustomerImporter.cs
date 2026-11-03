namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Clase concreta construida sobre otra clase concreta: el socio envía CSV,
    // pero tiene reglas de validación más estrictas: solo adultos con correos corporativos.
    public class PartnerCsvCustomerImporter : CsvCustomerImporter
    {
        protected override bool Validate(Customer customer)
        {
            if (!base.Validate(customer))
                return false;

            var isPartner = customer.Age >= 18 && customer.Email.EndsWith("@partner.com");
            if (!isPartner)
                Console.WriteLine($"  omitido por las reglas del socio: {customer.Name}");
            return isPartner;
        }
    }
}
