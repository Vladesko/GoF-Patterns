namespace GoFPatterns.Memento.TextEditorHistory
{
    // Verwalter (Caretaker) – entscheidet, WANN der Zustand gespeichert und wiederhergestellt wird, und bewahrt die Schnappschüsse auf.
    // In die Schnappschüsse selbst schaut er nie hinein.
    public class EditorHistory(TextEditor editor)
    {
        private readonly Stack<IEditorMemento> _history = new();

        public void Backup(string label)
        {
            _history.Push(editor.Save(label));
            Console.WriteLine($"  [history] \"{label}\" gespeichert");
        }

        public void Undo()
        {
            if (!_history.TryPop(out var memento))
            {
                Console.WriteLine("  [history] nichts zum Wiederherstellen");
                return;
            }
            editor.Restore(memento);
            Console.WriteLine($"  [history] \"{memento.Label}\" vom Stand {memento.CreatedAt:HH:mm:ss} wiederhergestellt");
        }

        public void ShowHistory()
        {
            Console.WriteLine($"  [history] {string.Join(" <- ", _history.Select(m => m.Label))}");
        }
    }
}
