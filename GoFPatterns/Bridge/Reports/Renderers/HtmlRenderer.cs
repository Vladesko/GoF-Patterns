namespace GoFPatterns.Bridge.Reports.Renderers
{
    // Implementor concreto - HTML per una pagina web.
    public class HtmlRenderer : IReportRenderer
    {
        public string RenderTitle(string title) => $"<h1>{title}</h1>";

        public string RenderRow(string name, string value) => $"<tr><td>{name}</td><td>{value}</td></tr>";

        public string RenderFooter(string text) => $"<footer>{text}</footer>";
    }
}
