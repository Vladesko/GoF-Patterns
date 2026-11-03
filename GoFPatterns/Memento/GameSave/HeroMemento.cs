namespace GoFPatterns.Memento.GameSave
{
    // Memento: instantánea inmutable del estado del héroe. Un record es una forma cómoda de escribirlo en C#.
    // El inventario se copia en un array nuevo, así que los cambios posteriores del héroe no afectan a la partida guardada.
    public sealed record HeroMemento(
        string SlotName,
        int Level,
        int Health,
        int Gold,
        string Location,
        IReadOnlyList<string> Inventory);
}
