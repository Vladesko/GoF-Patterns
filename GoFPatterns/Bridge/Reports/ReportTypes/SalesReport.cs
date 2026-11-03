namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Refined abstraction - report delle vendite.
    public class SalesReport(IReportRenderer renderer, IReadOnlyDictionary<string, decimal> salesByMonth)
        : Report(renderer)
    {
        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Report delle vendite") };
            lines.AddRange(salesByMonth.Select(s => Renderer.RenderRow(s.Key, s.Value.ToString("F2"))));
            lines.Add(Renderer.RenderFooter($"Totale: {salesByMonth.Values.Sum():F2}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
