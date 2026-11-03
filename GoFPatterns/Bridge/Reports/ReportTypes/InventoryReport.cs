namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Refined abstraction - report di magazzino. Ha una sua logica: segnala gli articoli in esaurimento.
    public class InventoryReport(IReportRenderer renderer, IReadOnlyDictionary<string, int> stock)
        : Report(renderer)
    {
        private const int LowStockLimit = 5;

        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Magazzino") };
            foreach (var (item, count) in stock)
            {
                var value = count < LowStockLimit ? $"{count} (scarso!)" : count.ToString();
                lines.Add(Renderer.RenderRow(item, value));
            }
            lines.Add(Renderer.RenderFooter($"Articoli da ordinare: {stock.Count(s => s.Value < LowStockLimit)}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
