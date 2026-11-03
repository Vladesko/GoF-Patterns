namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Abstraction - sa COSA c'è nel report, ma delega la visualizzazione all'implementor.
    public abstract class Report(IReportRenderer renderer)
    {
        protected IReportRenderer Renderer { get; } = renderer;

        public abstract string Build();
    }
}
