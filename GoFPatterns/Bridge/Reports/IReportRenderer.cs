namespace GoFPatterns.Bridge.Reports
{
    // Исполнитель - знает, КАК отрисовать примитивные части отчёта.
    public interface IReportRenderer
    {
        string RenderTitle(string title);
        string RenderRow(string name, string value);
        string RenderFooter(string text);
    }
}
