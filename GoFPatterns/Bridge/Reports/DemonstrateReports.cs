using GoFPatterns.Bridge.Reports.Renderers;
using GoFPatterns.Bridge.Reports.ReportTypes;

namespace GoFPatterns.Bridge.Reports
{
    public class DemonstrateReports
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Мост: отчёты ===\n");

            var sales = new Dictionary<string, decimal> { ["Январь"] = 1200.5m, ["Февраль"] = 980m, ["Март"] = 1430.25m };
            var stock = new Dictionary<string, int> { ["Ноутбук"] = 12, ["Мышь"] = 3, ["Монитор"] = 1 };

            IReportRenderer[] renderers = [new PlainTextRenderer(), new HtmlRenderer()];

            // Любой тип отчёта работает с любым рендерером. Рендерер можно выбрать во время выполнения.
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
