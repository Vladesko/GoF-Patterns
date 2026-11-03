namespace GoFPatterns.Prototype.Documents.Prototypes
{
    // Concrete prototype - contract with sections and style.
    public class Contract : IDocumentPrototype
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public DocumentStyle Style { get; set; } = new();
        public List<string> Sections { get; set; } = [];

        public Contract() { }

        // Copy constructor - copies private and public state of the prototype.
        // Reference fields are copied too, so clone is fully independent (deep copy).
        private Contract(Contract source)
        {
            Title = source.Title;
            Company = source.Company;
            Style = source.Style.Copy();
            Sections = [.. source.Sections];
        }

        public IDocumentPrototype Clone() => new Contract(this);

        // Shallow copy - only fields are copied, references point to the same objects.
        public Contract ShallowClone() => (Contract)MemberwiseClone();

        public override string ToString() =>
            $"Contract '{Title}' for {Company}, style: {Style}, sections: [{string.Join(", ", Sections)}]";
    }
}
