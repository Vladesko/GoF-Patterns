namespace GoFPatterns.Memento.GameSave
{
    // Снимок (Memento) - неизменяемый снимок состояния героя. Record - удобный способ описать его в C#.
    // Инвентарь копируется в новый массив, поэтому последующие изменения героя не влияют на сохранение.
    public sealed record HeroMemento(
        string SlotName,
        int Level,
        int Health,
        int Gold,
        string Location,
        IReadOnlyList<string> Inventory);
}
