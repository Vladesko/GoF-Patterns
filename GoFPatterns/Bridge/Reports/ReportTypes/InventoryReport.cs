namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Verfeinerte Abstraktion – Lagerbericht. Er hat eigene Logik: markiert Artikel, die zur Neige gehen.
    public class InventoryReport(IReportRenderer renderer, IReadOnlyDictionary<string, int> stock)
        : Report(renderer)
    {
        private const int LowStockLimit = 5;

        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Lagerbestand") };
            foreach (var (item, count) in stock)
            {
                var value = count < LowStockLimit ? $"{count} (knapp!)" : count.ToString();
                lines.Add(Renderer.RenderRow(item, value));
            }
            lines.Add(Renderer.RenderFooter($"Nachzubestellende Artikel: {stock.Count(s => s.Value < LowStockLimit)}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
