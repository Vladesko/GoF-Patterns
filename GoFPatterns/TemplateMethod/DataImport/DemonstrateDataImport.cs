using GoFPatterns.TemplateMethod.DataImport.Importers;

namespace GoFPatterns.TemplateMethod.DataImport
{
    public class DemonstrateDataImport
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Template Method: Datenimport ===\n");

            const string csv = """
                Name;Email;Age
                Anna;anna@mail.com;28
                Ivan;wrong-email;35
                Petr;petr@partner.com;17
                Olga;olga@partner.com;41
                """;

            const string json = """
                [
                  { "name": "Maria", "email": "maria@mail.com", "age": 30 },
                  { "name": "Oleg", "email": "oleg@mail.com", "age": 150 }
                ]
                """;

            // Der Client arbeitet mit der Basisklasse, der Algorithmus ist für alle Importer gleich.
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
