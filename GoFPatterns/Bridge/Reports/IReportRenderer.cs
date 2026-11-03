namespace GoFPatterns.Bridge.Reports
{
    // Implementierer – weiß, WIE die einfachen Bestandteile eines Berichts dargestellt werden.
    public interface IReportRenderer
    {
        string RenderTitle(string title);
        string RenderRow(string name, string value);
        string RenderFooter(string text);
    }
}
