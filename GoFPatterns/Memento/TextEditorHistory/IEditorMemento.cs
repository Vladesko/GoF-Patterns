namespace GoFPatterns.Memento.TextEditorHistory
{
    // Узкий интерфейс снимка - это всё, что видит опекун.
    // Само сохранённое состояние скрыто, поэтому никто, кроме редактора, не может его прочитать или изменить.
    public interface IEditorMemento
    {
        string Label { get; }
        DateTime CreatedAt { get; }
    }
}
