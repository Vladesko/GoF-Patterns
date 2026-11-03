namespace GoFPatterns.Prototype.Documents.Prototypes
{
    // Конкретный прототип - договор с разделами и стилем.
    public class Contract : IDocumentPrototype
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public DocumentStyle Style { get; set; } = new();
        public List<string> Sections { get; set; } = [];

        public Contract() { }

        // Конструктор копирования - копирует приватное и публичное состояние прототипа.
        // Ссылочные поля тоже копируются, поэтому клон полностью независим (глубокая копия).
        private Contract(Contract source)
        {
            Title = source.Title;
            Company = source.Company;
            Style = source.Style.Copy();
            Sections = [.. source.Sections];
        }

        public IDocumentPrototype Clone() => new Contract(this);

        // Поверхностная копия - копируются только поля, ссылки указывают на те же объекты.
        public Contract ShallowClone() => (Contract)MemberwiseClone();

        public override string ToString() =>
            $"Договор '{Title}' для {Company}, стиль: {Style}, разделы: [{string.Join(", ", Sections)}]";
    }
}
