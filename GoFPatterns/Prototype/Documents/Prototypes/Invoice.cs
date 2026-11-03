namespace GoFPatterns.Prototype.Documents.Prototypes
{
    // Конкретный прототип - счёт с позициями.
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
            $"Счёт '{Title}', налог {TaxRate:P0}, позиций: {Lines.Count}, итого: {Total:F2}";
    }
}
