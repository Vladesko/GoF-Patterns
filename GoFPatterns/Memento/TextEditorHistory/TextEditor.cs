namespace GoFPatterns.Memento.TextEditorHistory
{
    // Originator - object whose state is saved and restored.
    // Only it knows how to create a snapshot and how to restore from it.
    public class TextEditor
    {
        private string _text = string.Empty;
        private int _cursor;
        private string _font = "Arial";

        public void Type(string text)
        {
            _text = _text.Insert(_cursor, text);
            _cursor += text.Length;
        }

        public void MoveCursor(int position) => _cursor = Math.Clamp(position, 0, _text.Length);

        public void SetFont(string font) => _font = font;

        public IEditorMemento Save(string label) => new Snapshot(label, _text, _cursor, _font);

        public void Restore(IEditorMemento memento)
        {
            // Editor accepts only its own snapshots.
            if (memento is not Snapshot snapshot)
                throw new ArgumentException("Unknown memento type.", nameof(memento));

            _text = snapshot.Text;
            _cursor = snapshot.Cursor;
            _font = snapshot.Font;
        }

        public override string ToString() => $"\"{_text.Insert(_cursor, "|")}\" [{_font}]";

        // Memento - private nested class. Its fields are visible to the editor,
        // but outside the editor it is accessible only through IEditorMemento.
        private sealed class Snapshot(string label, string text, int cursor, string font) : IEditorMemento
        {
            public string Label { get; } = label;
            public DateTime CreatedAt { get; } = DateTime.Now;
            public string Text { get; } = text;
            public int Cursor { get; } = cursor;
            public string Font { get; } = font;
        }
    }
}
