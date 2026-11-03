namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Уточнённая абстракция - отчёт об остатках. У него своя логика: помечает заканчивающиеся товары.
    public class InventoryReport(IReportRenderer renderer, IReadOnlyDictionary<string, int> stock)
        : Report(renderer)
    {
        private const int LowStockLimit = 5;

        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Остатки на складе") };
            foreach (var (item, count) in stock)
            {
                var value = count < LowStockLimit ? $"{count} (мало!)" : count.ToString();
                lines.Add(Renderer.RenderRow(item, value));
            }
            lines.Add(Renderer.RenderFooter($"Нужно заказать позиций: {stock.Count(s => s.Value < LowStockLimit)}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
