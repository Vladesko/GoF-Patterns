namespace GoFPatterns.Memento.TextEditorHistory
{
    // Cuidador (caretaker): decide CUÁNDO guardar y restaurar el estado, y almacena las instantáneas.
    // Nunca mira dentro de las instantáneas.
    public class EditorHistory(TextEditor editor)
    {
        private readonly Stack<IEditorMemento> _history = new();

        public void Backup(string label)
        {
            _history.Push(editor.Save(label));
            Console.WriteLine($"  [historial] guardado \"{label}\"");
        }

        public void Undo()
        {
            if (!_history.TryPop(out var memento))
            {
                Console.WriteLine("  [historial] no hay nada que restaurar");
                return;
            }
            editor.Restore(memento);
            Console.WriteLine($"  [historial] restaurado \"{memento.Label}\" de las {memento.CreatedAt:HH:mm:ss}");
        }

        public void ShowHistory()
        {
            Console.WriteLine($"  [historial] {string.Join(" <- ", _history.Select(m => m.Label))}");
        }
    }
}
