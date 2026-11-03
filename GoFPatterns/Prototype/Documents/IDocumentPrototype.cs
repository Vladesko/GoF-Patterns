namespace GoFPatterns.Prototype.Documents
{
    // Прототип - объявляет метод клонирования.
    // Клиент копирует объекты через этот интерфейс, не зная их конкретных классов.
    public interface IDocumentPrototype
    {
        string Title { get; set; }
        IDocumentPrototype Clone();
    }
}
