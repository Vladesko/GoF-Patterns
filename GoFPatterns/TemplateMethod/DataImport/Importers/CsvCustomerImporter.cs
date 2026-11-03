namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Concrete class - knows only how to parse CSV. The rest of the algorithm is inherited.
    public class CsvCustomerImporter : CustomerImporter
    {
        // Hook override - remove header line.
        protected override string Preprocess(string rawData) =>
            string.Join('\n', base.Preprocess(rawData).Split('\n').Skip(1));

        protected override List<Customer> Parse(string content) =>
            content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(';'))
                .Select(p => new Customer(p[0].Trim(), p[1].Trim(), int.Parse(p[2])))
                .ToList();
    }
}
