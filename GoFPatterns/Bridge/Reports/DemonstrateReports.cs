using GoFPatterns.Bridge.Reports.Renderers;
using GoFPatterns.Bridge.Reports.ReportTypes;

namespace GoFPatterns.Bridge.Reports
{
    public class DemonstrateReports
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: Report ===\n");

            var sales = new Dictionary<string, decimal> { ["Gennaio"] = 1200.5m, ["Febbraio"] = 980m, ["Marzo"] = 1430.25m };
            var stock = new Dictionary<string, int> { ["Portatile"] = 12, ["Mouse"] = 3, ["Monitor"] = 1 };

            IReportRenderer[] renderers = [new PlainTextRenderer(), new HtmlRenderer()];

            // Qualsiasi tipo di report funziona con qualsiasi renderer. Il renderer può essere scelto a runtime.
            foreach (var renderer in renderers)
            {
                Report[] reports = [new SalesReport(renderer, sales), new InventoryReport(renderer, stock)];
                foreach (var report in reports)
                {
                    Console.WriteLine($"{report.GetType().Name} + {renderer.GetType().Name}:");
                    Console.WriteLine(report.Build());
                    Console.WriteLine();
                }
            }
        }
    }
}
