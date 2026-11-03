namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Clase concreta: solo sabe analizar CSV. El resto del algoritmo se hereda.
    public class CsvCustomerImporter : CustomerImporter
    {
        // Sobrescritura del hook: eliminar la línea de cabecera.
        protected override string Preprocess(string rawData) =>
            string.Join('\n', base.Preprocess(rawData).Split('\n').Skip(1));

        protected override List<Customer> Parse(string content) =>
            content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(';'))
                .Select(p => new Customer(p[0].Trim(), p[1].Trim(), int.Parse(p[2])))
                .ToList();
    }
}
