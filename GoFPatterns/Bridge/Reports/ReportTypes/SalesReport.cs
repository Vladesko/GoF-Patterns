namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Abstracción refinada: informe de ventas.
    public class SalesReport(IReportRenderer renderer, IReadOnlyDictionary<string, decimal> salesByMonth)
        : Report(renderer)
    {
        public override string Build()
        {
            var lines = new List<string> { Renderer.RenderTitle("Informe de ventas") };
            lines.AddRange(salesByMonth.Select(s => Renderer.RenderRow(s.Key, s.Value.ToString("F2"))));
            lines.Add(Renderer.RenderFooter($"Total: {salesByMonth.Values.Sum():F2}"));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
