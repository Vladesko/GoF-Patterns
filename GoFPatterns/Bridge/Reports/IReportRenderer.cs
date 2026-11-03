namespace GoFPatterns.Bridge.Reports
{
    // Implementor - sa COME visualizzare le parti primitive di un report.
    public interface IReportRenderer
    {
        string RenderTitle(string title);
        string RenderRow(string name, string value);
        string RenderFooter(string text);
    }
}
