namespace GoFPatterns.Memento.GameSave
{
    // Memento - immutable snapshot of hero state. Record is a convenient way to write it in C#.
    // Inventory is copied into a new array, so later changes of hero don't affect the save.
    public sealed record HeroMemento(
        string SlotName,
        int Level,
        int Health,
        int Gold,
        string Location,
        IReadOnlyList<string> Inventory);
}
