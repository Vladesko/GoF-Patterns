namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Abstraction - knows WHAT is in the report, but delegates rendering to implementor.
    public abstract class Report(IReportRenderer renderer)
    {
        protected IReportRenderer Renderer { get; } = renderer;

        public abstract string Build();
    }
}
