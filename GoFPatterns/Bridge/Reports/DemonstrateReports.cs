using GoFPatterns.Bridge.Reports.Renderers;
using GoFPatterns.Bridge.Reports.ReportTypes;

namespace GoFPatterns.Bridge.Reports
{
    public class DemonstrateReports
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: Berichte ===\n");

            var sales = new Dictionary<string, decimal> { ["Januar"] = 1200.5m, ["Februar"] = 980m, ["März"] = 1430.25m };
            var stock = new Dictionary<string, int> { ["Laptop"] = 12, ["Maus"] = 3, ["Monitor"] = 1 };

            IReportRenderer[] renderers = [new PlainTextRenderer(), new HtmlRenderer()];

            // Jeder Berichtstyp funktioniert mit jedem Renderer. Der Renderer kann zur Laufzeit gewählt werden.
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
