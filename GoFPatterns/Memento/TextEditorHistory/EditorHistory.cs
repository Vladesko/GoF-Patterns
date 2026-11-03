namespace GoFPatterns.Memento.TextEditorHistory
{
    // Опекун (Caretaker) - решает, КОГДА сохранять и восстанавливать состояние, и хранит снимки.
    // Он никогда не заглядывает внутрь снимков.
    public class EditorHistory(TextEditor editor)
    {
        private readonly Stack<IEditorMemento> _history = new();

        public void Backup(string label)
        {
            _history.Push(editor.Save(label));
            Console.WriteLine($"  [история] сохранено \"{label}\"");
        }

        public void Undo()
        {
            if (!_history.TryPop(out var memento))
            {
                Console.WriteLine("  [история] нечего восстанавливать");
                return;
            }
            editor.Restore(memento);
            Console.WriteLine($"  [история] восстановлено \"{memento.Label}\" от {memento.CreatedAt:HH:mm:ss}");
        }

        public void ShowHistory()
        {
            Console.WriteLine($"  [история] {string.Join(" <- ", _history.Select(m => m.Label))}");
        }
    }
}
