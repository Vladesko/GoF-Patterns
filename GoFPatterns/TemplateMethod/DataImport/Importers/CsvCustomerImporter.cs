namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Classe concreta - sa solo come leggere il CSV. Il resto dell'algoritmo è ereditato.
    public class CsvCustomerImporter : CustomerImporter
    {
        // Override dell'hook - rimuove la riga di intestazione.
        protected override string Preprocess(string rawData) =>
            string.Join('\n', base.Preprocess(rawData).Split('\n').Skip(1));

        protected override List<Customer> Parse(string content) =>
            content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(';'))
                .Select(p => new Customer(p[0].Trim(), p[1].Trim(), int.Parse(p[2])))
                .ToList();
    }
}
