namespace GoFPatterns.Bridge.Reports
{
    // Implementor - knows HOW to render primitive parts of a report.
    public interface IReportRenderer
    {
        string RenderTitle(string title);
        string RenderRow(string name, string value);
        string RenderFooter(string text);
    }
}
