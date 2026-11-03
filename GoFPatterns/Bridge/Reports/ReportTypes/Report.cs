namespace GoFPatterns.Bridge.Reports.ReportTypes
{
    // Abstracción: sabe QUÉ hay en el informe, pero delega el renderizado al implementador.
    public abstract class Report(IReportRenderer renderer)
    {
        protected IReportRenderer Renderer { get; } = renderer;

        public abstract string Build();
    }
}
