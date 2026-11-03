namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Refined abstraction - inventory report. It has its own logic: marks items which are running out.
    public class InventoryReport(IReportRenderer renderer, IReadOnlyDictionary<string, int> stock)
        : Report(renderer)
    {
        private const int LowStockLimit = 5;

        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Inventory") };
            foreach (var (item, count) in stock)
            {
                var value = count < LowStockLimit ? $"{count} (low!)" : count.ToString();
                lines.Add(Renderer.RenderRow(item, value));
            }
            lines.Add(Renderer.RenderFooter($"Items to order: {stock.Count(s => s.Value < LowStockLimit)}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
