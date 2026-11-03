namespace GoFPatterns.Prototype.Documents
{
    // Referenztyp innerhalb des Dokuments. Er zeigt den Unterschied zwischen flacher und tiefer Kopie.
    public class DocumentStyle
    {
        public string Font { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;

        public DocumentStyle Copy() => new() { Font = Font, FontSize = FontSize };

        public override string ToString() => $"{Font} {FontSize}pt";
    }
}
