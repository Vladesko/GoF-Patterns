using GoFPatterns.TemplateMethod.DataImport.Importers;

namespace GoFPatterns.TemplateMethod.DataImport
{
    public class DemonstrateDataImport
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Шаблонный метод: импорт данных ===\n");

            const string csv = """
                Name;Email;Age
                Анна;anna@mail.com;28
                Иван;wrong-email;35
                Пётр;petr@partner.com;17
                Ольга;olga@partner.com;41
                """;

            const string json = """
                [
                  { "name": "Мария", "email": "maria@mail.com", "age": 30 },
                  { "name": "Олег", "email": "oleg@mail.com", "age": 150 }
                ]
                """;

            // Клиент работает с базовым классом, алгоритм одинаков для всех импортёров.
            (CustomerImporter Importer, string Data)[] imports =
            [
                (new CsvCustomerImporter(), csv),
                (new JsonCustomerImporter(), json),
                (new PartnerCsvCustomerImporter(), csv)
            ];

            foreach (var (importer, data) in imports)
            {
                importer.Import(data);
                Console.WriteLine();
            }
        }
    }
}
