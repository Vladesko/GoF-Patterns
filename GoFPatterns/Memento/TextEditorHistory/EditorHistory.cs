namespace GoFPatterns.Memento.TextEditorHistory
{
    // Caretaker - decides WHEN to save and restore the state, and stores snapshots.
    // It never looks inside snapshots.
    public class EditorHistory(TextEditor editor)
    {
        private readonly Stack<IEditorMemento> _history = new();

        public void Backup(string label)
        {
            _history.Push(editor.Save(label));
            Console.WriteLine($"  [history] saved \"{label}\"");
        }

        public void Undo()
        {
            if (!_history.TryPop(out var memento))
            {
                Console.WriteLine("  [history] nothing to restore");
                return;
            }
            editor.Restore(memento);
            Console.WriteLine($"  [history] restored \"{memento.Label}\" from {memento.CreatedAt:HH:mm:ss}");
        }

        public void ShowHistory()
        {
            Console.WriteLine($"  [history] {string.Join(" <- ", _history.Select(m => m.Label))}");
        }
    }
}
