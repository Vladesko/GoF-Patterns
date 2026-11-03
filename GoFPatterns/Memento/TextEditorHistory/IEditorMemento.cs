namespace GoFPatterns.Memento.TextEditorHistory
{
    // Interfaccia ristretta del memento - è tutto ciò che il caretaker può vedere.
    // Lo stato salvato è nascosto, quindi nessuno tranne l'editor può leggerlo o modificarlo.
    public interface IEditorMemento
    {
        string Label { get; }
        DateTime CreatedAt { get; }
    }
}
