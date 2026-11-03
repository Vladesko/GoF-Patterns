namespace GoFPatterns.Bridge.Reports.Renderers
{
    // Konkreter Implementierer – reiner Text für Konsole oder E-Mail.
    public class PlainTextRenderer : IReportRenderer
    {
        public string RenderTitle(string title) => $"{title.ToUpper()}{Environment.NewLine}{new string('-', title.Length)}";

        public string RenderRow(string name, string value) => $"{name,-15}{value,10}";

        public string RenderFooter(string text) => $"-- {text}";
    }
}
