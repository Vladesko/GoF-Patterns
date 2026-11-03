namespace GoFPatterns.Bridge.Reports
{
    // Implementador: sabe CÓMO renderizar las partes primitivas de un informe.
    public interface IReportRenderer
    {
        string RenderTitle(string title);
        string RenderRow(string name, string value);
        string RenderFooter(string text);
    }
}
