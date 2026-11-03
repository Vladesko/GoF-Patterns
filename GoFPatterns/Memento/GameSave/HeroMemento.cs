namespace GoFPatterns.Memento.GameSave
{
    // Memento – unveränderlicher Schnappschuss des Heldenzustands. Ein Record ist in C# ein bequemer Weg dafür.
    // Das Inventar wird in ein neues Array kopiert, damit spätere Änderungen am Helden den Spielstand nicht beeinflussen.
    public sealed record HeroMemento(
        string SlotName,
        int Level,
        int Health,
        int Gold,
        string Location,
        IReadOnlyList<string> Inventory);
}
