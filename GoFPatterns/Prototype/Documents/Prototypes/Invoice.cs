namespace GoFPatterns.Prototype.Documents.Prototypes
{
    // Prototype concreto - fattura con righe.
    public class Invoice : IDocumentPrototype
    {
        public string Title { get; set; } = string.Empty;
        public decimal TaxRate { get; set; }
        public List<(string Item, decimal Price)> Lines { get; set; } = [];

        public decimal Total => Lines.Sum(l => l.Price) * (1 + TaxRate);

        public IDocumentPrototype Clone() => new Invoice
        {
            Title = Title,
            TaxRate = TaxRate,
            Lines = [.. Lines]
        };

        public override string ToString() =>
            $"Fattura '{Title}', imposta {TaxRate:P0}, righe: {Lines.Count}, totale: {Total:F2}";
    }
}
