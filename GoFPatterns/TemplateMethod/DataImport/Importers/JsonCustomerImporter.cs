using System.Text.Json;

namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Clase concreta: analiza JSON y añade su propia reacción después de la importación.
    public class JsonCustomerImporter : CustomerImporter
    {
        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        protected override List<Customer> Parse(string content) =>
            JsonSerializer.Deserialize<List<Customer>>(content, _options) ?? [];

        // Sobrescritura del hook.
        protected override void AfterImport(List<Customer> customers) =>
            Console.WriteLine($"  enviando correos de bienvenida a {customers.Count} clientes");
    }
}
