using System.Text.Json;

namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Concrete class - parses JSON and adds its own reaction after import.
    public class JsonCustomerImporter : CustomerImporter
    {
        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        protected override List<Customer> Parse(string content) =>
            JsonSerializer.Deserialize<List<Customer>>(content, _options) ?? [];

        // Hook override.
        protected override void AfterImport(List<Customer> customers) =>
            Console.WriteLine($"  sending welcome e-mails to {customers.Count} customers");
    }
}
