namespace GoFPatterns.Prototype.Documents
{
    // Tipo por referencia dentro del documento. Muestra la diferencia entre copia superficial y profunda.
    public class DocumentStyle
    {
        public string Font { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;

        public DocumentStyle Copy() => new() { Font = Font, FontSize = FontSize };

        public override string ToString() => $"{Font} {FontSize}pt";
    }
}
