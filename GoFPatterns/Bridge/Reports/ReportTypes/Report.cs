namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Абстракция - знает, ЧТО содержится в отчёте, но делегирует отрисовку исполнителю.
    public abstract class Report(IReportRenderer renderer)
    {
        protected IReportRenderer Renderer { get; } = renderer;

        public abstract string Build();
    }
}
