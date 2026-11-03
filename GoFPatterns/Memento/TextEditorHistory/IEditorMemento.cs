namespace GoFPatterns.Memento.TextEditorHistory
{
    // Interfaz estrecha del memento: es todo lo que el cuidador puede ver.
    // El estado guardado en sí está oculto, así que nadie salvo el editor puede leerlo o modificarlo.
    public interface IEditorMemento
    {
        string Label { get; }
        DateTime CreatedAt { get; }
    }
}
