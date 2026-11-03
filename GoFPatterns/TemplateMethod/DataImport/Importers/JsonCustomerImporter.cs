using System.Text.Json;

namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Konkrete Klasse – liest JSON ein und ergänzt eine eigene Reaktion nach dem Import.
    public class JsonCustomerImporter : CustomerImporter
    {
        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        protected override List<Customer> Parse(string content) =>
            JsonSerializer.Deserialize<List<Customer>>(content, _options) ?? [];

        // Überschriebener Hook.
        protected override void AfterImport(List<Customer> customers) =>
            Console.WriteLine($"  Begrüßungs-E-Mails an {customers.Count} Kunden werden versendet");
    }
}
