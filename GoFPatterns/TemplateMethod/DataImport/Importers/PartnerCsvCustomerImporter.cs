namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Конкретный класс, построенный поверх другого конкретного класса: партнёр присылает CSV,
    // но у него более строгие правила проверки: только совершеннолетние с корпоративными e-mail.
    public class PartnerCsvCustomerImporter : CsvCustomerImporter
    {
        protected override bool Validate(Customer customer)
        {
            if (!base.Validate(customer))
                return false;

            var isPartner = customer.Age >= 18 && customer.Email.EndsWith("@partner.com");
            if (!isPartner)
                Console.WriteLine($"  пропущен по правилам партнёра: {customer.Name}");
            return isPartner;
        }
    }
}
