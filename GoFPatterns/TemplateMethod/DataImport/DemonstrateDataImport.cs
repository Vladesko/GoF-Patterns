using GoFPatterns.TemplateMethod.DataImport.Importers;

namespace GoFPatterns.TemplateMethod.DataImport
{
    public class DemonstrateDataImport
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Template Method: Importazione dati ===\n");

            const string csv = """
                Nome;Email;Età
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

            // Il client lavora con la classe base, l'algoritmo è lo stesso per tutti gli importer.
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
