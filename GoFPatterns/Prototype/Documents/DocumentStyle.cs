namespace GoFPatterns.Prototype.Documents
{
    // Reference type inside of the document. It shows the difference between shallow and deep copy.
    public class DocumentStyle
    {
        public string Font { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;

        public DocumentStyle Copy() => new() { Font = Font, FontSize = FontSize };

        public override string ToString() => $"{Font} {FontSize}pt";
    }
}
