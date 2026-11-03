namespace GoFPatterns.Bridge.Reports.Renderers
{
    // Concrete implementor - plain text for console or e-mail.
    public class PlainTextRenderer : IReportRenderer
    {
        public string RenderTitle(string title) => $"{title.ToUpper()}{Environment.NewLine}{new string('-', title.Length)}";

        public string RenderRow(string name, string value) => $"{name,-15}{value,10}";

        public string RenderFooter(string text) => $"-- {text}";
    }
}
