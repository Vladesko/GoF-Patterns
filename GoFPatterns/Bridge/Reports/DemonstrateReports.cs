using GoFPatterns.Bridge.Reports.Renderers;
using GoFPatterns.Bridge.Reports.ReportTypes;

namespace GoFPatterns.Bridge.Reports
{
    public class DemonstrateReports
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: Reports ===\n");

            var sales = new Dictionary<string, decimal> { ["January"] = 1200.5m, ["February"] = 980m, ["March"] = 1430.25m };
            var stock = new Dictionary<string, int> { ["Laptop"] = 12, ["Mouse"] = 3, ["Monitor"] = 1 };

            IReportRenderer[] renderers = [new PlainTextRenderer(), new HtmlRenderer()];

            // Any report type works with any renderer. Renderer can be chosen at runtime.
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
