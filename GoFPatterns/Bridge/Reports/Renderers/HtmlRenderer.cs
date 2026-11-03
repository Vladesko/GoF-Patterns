namespace GoFPatterns.Bridge.Reports.Renderers
{
    // Implementador concreto: HTML para una página web.
    public class HtmlRenderer : IReportRenderer
    {
        public string RenderTitle(string title) => $"<h1>{title}</h1>";

        public string RenderRow(string name, string value) => $"<tr><td>{name}</td><td>{value}</td></tr>";

        public string RenderFooter(string text) => $"<footer>{text}</footer>";
    }
}
