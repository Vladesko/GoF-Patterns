namespace GoFPatterns.Prototype.Documents
{
    // Tipo riferimento all'interno del documento. Mostra la differenza tra copia superficiale e profonda.
    public class DocumentStyle
    {
        public string Font { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;

        public DocumentStyle Copy() => new() { Font = Font, FontSize = FontSize };

        public override string ToString() => $"{Font} {FontSize}pt";
    }
}
