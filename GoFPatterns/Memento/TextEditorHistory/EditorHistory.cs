namespace GoFPatterns.Memento.TextEditorHistory
{
    // Caretaker - decide QUANDO salvare e ripristinare lo stato, e conserva gli snapshot.
    // Non guarda mai all'interno degli snapshot.
    public class EditorHistory(TextEditor editor)
    {
        private readonly Stack<IEditorMemento> _history = new();

        public void Backup(string label)
        {
            _history.Push(editor.Save(label));
            Console.WriteLine($"  [cronologia] salvato \"{label}\"");
        }

        public void Undo()
        {
            if (!_history.TryPop(out var memento))
            {
                Console.WriteLine("  [cronologia] niente da ripristinare");
                return;
            }
            editor.Restore(memento);
            Console.WriteLine($"  [cronologia] ripristinato \"{memento.Label}\" delle {memento.CreatedAt:HH:mm:ss}");
        }

        public void ShowHistory()
        {
            Console.WriteLine($"  [cronologia] {string.Join(" <- ", _history.Select(m => m.Label))}");
        }
    }
}
