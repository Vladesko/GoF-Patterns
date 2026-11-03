namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Abstraktion – weiß, WAS im Bericht steht, delegiert die Darstellung aber an den Implementierer.
    public abstract class Report(IReportRenderer renderer)
    {
        protected IReportRenderer Renderer { get; } = renderer;

        public abstract string Build();
    }
}
