namespace GoFPatterns.Bridge.Reports.Renderers
{
    // Implementador concreto: texto plano para la consola o el correo electrónico.
    public class PlainTextRenderer : IReportRenderer
    {
        public string RenderTitle(string title) => $"{title.ToUpper()}{Environment.NewLine}{new string('-', title.Length)}";

        public string RenderRow(string name, string value) => $"{name,-15}{value,10}";

        public string RenderFooter(string text) => $"-- {text}";
    }
}
