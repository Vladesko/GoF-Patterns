using System.Text.Json;

namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Classe concreta - legge il JSON e aggiunge una propria reazione dopo l'importazione.
    public class JsonCustomerImporter : CustomerImporter
    {
        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        protected override List<Customer> Parse(string content) =>
            JsonSerializer.Deserialize<List<Customer>>(content, _options) ?? [];

        // Override dell'hook.
        protected override void AfterImport(List<Customer> customers) =>
            Console.WriteLine($"  invio delle e-mail di benvenuto a {customers.Count} clienti");
    }
}
