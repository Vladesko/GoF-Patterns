namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Verfeinerte Abstraktion – Umsatzbericht.
    public class SalesReport(IReportRenderer renderer, IReadOnlyDictionary<string, decimal> salesByMonth)
        : Report(renderer)
    {
        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Umsatzbericht") };
            lines.AddRange(salesByMonth.Select(s => Renderer.RenderRow(s.Key, s.Value.ToString("F2"))));
            lines.Add(Renderer.RenderFooter($"Gesamt: {salesByMonth.Values.Sum():F2}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
