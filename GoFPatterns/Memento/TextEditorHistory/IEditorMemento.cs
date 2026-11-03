namespace GoFPatterns.Memento.TextEditorHistory
{
    // Schmale Schnittstelle des Mementos – mehr sieht der Verwalter nicht.
    // Der gespeicherte Zustand selbst ist verborgen, daher kann ihn niemand außer dem Editor lesen oder ändern.
    public interface IEditorMemento
    {
        string Label { get; }
        DateTime CreatedAt { get; }
    }
}
