namespace GoFPatterns.Prototype.Documents.Prototypes
{
    // Prototipo concreto: contrato con secciones y estilo.
    public class Contract : IDocumentPrototype
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public DocumentStyle Style { get; set; } = new();
        public List<string> Sections { get; set; } = [];

        public Contract() { }

        // Constructor de copia: copia el estado privado y público del prototipo.
        // Los campos de referencia también se copian, así que el clon es totalmente independiente (copia profunda).
        private Contract(Contract source)
        {
            Title = source.Title;
            Company = source.Company;
            Style = source.Style.Copy();
            Sections = [.. source.Sections];
        }

        public IDocumentPrototype Clone() => new Contract(this);

        // Copia superficial: solo se copian los campos; las referencias apuntan a los mismos objetos.
        public Contract ShallowClone() => (Contract)MemberwiseClone();

        public override string ToString() =>
            $"Contrato '{Title}' para {Company}, estilo: {Style}, secciones: [{string.Join(", ", Sections)}]";
    }
}
