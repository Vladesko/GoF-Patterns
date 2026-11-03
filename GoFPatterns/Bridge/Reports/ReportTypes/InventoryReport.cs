namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Abstracción refinada: informe de inventario. Tiene su propia lógica: marca los artículos que se están agotando.
    public class InventoryReport(IReportRenderer renderer, IReadOnlyDictionary<string, int> stock)
        : Report(renderer)
    {
        private const int LowStockLimit = 5;

        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Inventario") };
            foreach (var (item, count) in stock)
            {
                var value = count < LowStockLimit ? $"{count} (¡poco!)" : count.ToString();
                lines.Add(Renderer.RenderRow(item, value));
            }
            lines.Add(Renderer.RenderFooter($"Artículos por pedir: {stock.Count(s => s.Value < LowStockLimit)}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
