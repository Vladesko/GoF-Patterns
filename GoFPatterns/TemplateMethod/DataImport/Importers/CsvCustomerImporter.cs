namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Konkrete Klasse – weiß nur, wie CSV eingelesen wird. Der Rest des Algorithmus wird geerbt.
    public class CsvCustomerImporter : CustomerImporter
    {
        // Überschriebener Hook – entfernt die Kopfzeile.
        protected override string Preprocess(string rawData) =>
            string.Join('\n', base.Preprocess(rawData).Split('\n').Skip(1));

        protected override List<Customer> Parse(string content) =>
            content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(';'))
                .Select(p => new Customer(p[0].Trim(), p[1].Trim(), int.Parse(p[2])))
                .ToList();
    }
}
