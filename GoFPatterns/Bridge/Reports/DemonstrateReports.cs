using GoFPatterns.Bridge.Reports.Renderers;
using GoFPatterns.Bridge.Reports.ReportTypes;

namespace GoFPatterns.Bridge.Reports
{
    public class DemonstrateReports
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: informes ===\n");

            var sales = new Dictionary<string, decimal> { ["Enero"] = 1200.5m, ["Febrero"] = 980m, ["Marzo"] = 1430.25m };
            var stock = new Dictionary<string, int> { ["Portátil"] = 12, ["Ratón"] = 3, ["Monitor"] = 1 };

            IReportRenderer[] renderers = [new PlainTextRenderer(), new HtmlRenderer()];

            // Cualquier tipo de informe funciona con cualquier renderizador. El renderizador se puede elegir en tiempo de ejecución.
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
