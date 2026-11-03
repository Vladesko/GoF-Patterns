namespace GoFPatterns.Prototype.Documents.Prototypes
{
    // Prototype concreto - contratto con sezioni e stile.
    public class Contract : IDocumentPrototype
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public DocumentStyle Style { get; set; } = new();
        public List<string> Sections { get; set; } = [];

        public Contract() { }

        // Costruttore di copia - copia lo stato privato e pubblico del prototype.
        // Anche i campi di tipo riferimento vengono copiati, quindi il clone è completamente indipendente (copia profonda).
        private Contract(Contract source)
        {
            Title = source.Title;
            Company = source.Company;
            Style = source.Style.Copy();
            Sections = [.. source.Sections];
        }

        public IDocumentPrototype Clone() => new Contract(this);

        // Copia superficiale - vengono copiati solo i campi, i riferimenti puntano agli stessi oggetti.
        public Contract ShallowClone() => (Contract)MemberwiseClone();

        public override string ToString() =>
            $"Contratto '{Title}' per {Company}, stile: {Style}, sezioni: [{string.Join(", ", Sections)}]";
    }
}
