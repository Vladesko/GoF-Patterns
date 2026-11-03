using System.Text.Json;

namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Конкретный класс - разбирает JSON и добавляет собственную реакцию после импорта.
    public class JsonCustomerImporter : CustomerImporter
    {
        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        protected override List<Customer> Parse(string content) =>
            JsonSerializer.Deserialize<List<Customer>>(content, _options) ?? [];

        // Переопределение хука.
        protected override void AfterImport(List<Customer> customers) =>
            Console.WriteLine($"  отправка приветственных писем клиентам: {customers.Count}");
    }
}
