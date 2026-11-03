namespace GoFPatterns.Prototype.Documents.Prototypes
{
    // Konkreter Prototyp – Vertrag mit Abschnitten und Stil.
    public class Contract : IDocumentPrototype
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public DocumentStyle Style { get; set; } = new();
        public List<string> Sections { get; set; } = [];

        public Contract() { }

        // Kopierkonstruktor – kopiert den privaten und öffentlichen Zustand des Prototyps.
        // Auch Referenzfelder werden kopiert, daher ist der Klon völlig unabhängig (tiefe Kopie).
        private Contract(Contract source)
        {
            Title = source.Title;
            Company = source.Company;
            Style = source.Style.Copy();
            Sections = [.. source.Sections];
        }

        public IDocumentPrototype Clone() => new Contract(this);

        // Flache Kopie – nur die Felder werden kopiert, Referenzen zeigen auf dieselben Objekte.
        public Contract ShallowClone() => (Contract)MemberwiseClone();

        public override string ToString() =>
            $"Vertrag '{Title}' für {Company}, Stil: {Style}, Abschnitte: [{string.Join(", ", Sections)}]";
    }
}
