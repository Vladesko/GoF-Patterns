namespace GoFPatterns.Memento.TextEditorHistory
{
    // Narrow interface of memento - this is all the caretaker can see.
    // Saved state itself is hidden, so nobody except editor can read or change it.
    public interface IEditorMemento
    {
        string Label { get; }
        DateTime CreatedAt { get; }
    }
}
