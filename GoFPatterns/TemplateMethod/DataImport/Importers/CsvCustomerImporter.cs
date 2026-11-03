namespace GoFPatterns.TemplateMethod.DataImport.Importers
{
    // Конкретный класс - умеет только разбирать CSV. Остальная часть алгоритма унаследована.
    public class CsvCustomerImporter : CustomerImporter
    {
        // Переопределение хука - удаляем строку заголовка.
        protected override string Preprocess(string rawData) =>
            string.Join('\n', base.Preprocess(rawData).Split('\n').Skip(1));

        protected override List<Customer> Parse(string content) =>
            content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(';'))
                .Select(p => new Customer(p[0].Trim(), p[1].Trim(), int.Parse(p[2])))
                .ToList();
    }
}
