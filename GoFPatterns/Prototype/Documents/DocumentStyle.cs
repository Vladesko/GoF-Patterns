namespace GoFPatterns.Prototype.Documents
{
    // Ссылочный тип внутри документа. Показывает разницу между поверхностной и глубокой копией.
    public class DocumentStyle
    {
        public string Font { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;

        public DocumentStyle Copy() => new() { Font = Font, FontSize = FontSize };

        public override string ToString() => $"{Font} {FontSize}пт";
    }
}
