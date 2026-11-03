namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Refined abstraction - sales report.
    public class SalesReport(IReportRenderer renderer, IReadOnlyDictionary<string, decimal> salesByMonth)
        : Report(renderer)
    {
        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Sales report") };
            lines.AddRange(salesByMonth.Select(s => Renderer.RenderRow(s.Key, s.Value.ToString("F2"))));
            lines.Add(Renderer.RenderFooter($"Total: {salesByMonth.Values.Sum():F2}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
