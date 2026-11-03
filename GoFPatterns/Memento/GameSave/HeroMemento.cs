namespace GoFPatterns.Memento.GameSave
{
    // Memento - snapshot immutabile dello stato dell'eroe. Un record è un modo comodo per scriverlo in C#.
    // L'inventario viene copiato in un nuovo array, così le modifiche successive dell'eroe non influenzano il salvataggio.
    public sealed record HeroMemento(
        string SlotName,
        int Level,
        int Health,
        int Gold,
        string Location,
        IReadOnlyList<string> Inventory);
}
