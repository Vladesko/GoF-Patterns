namespace GoFPatterns.Memento.TextEditorHistory
{
    // Urheber (Originator) – Objekt, dessen Zustand gespeichert und wiederhergestellt wird.
    // Nur er weiß, wie ein Schnappschuss erstellt und aus ihm wiederhergestellt wird.
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
            // Der Editor akzeptiert nur seine eigenen Schnappschüsse.
            if (memento is not Snapshot snapshot)
                throw new ArgumentException("Unbekannter Memento-Typ.", nameof(memento));

            _text = snapshot.Text;
            _cursor = snapshot.Cursor;
            _font = snapshot.Font;
        }

        public override string ToString() => $"\"{_text.Insert(_cursor, "|")}\" [{_font}]";

        // Memento – private verschachtelte Klasse. Ihre Felder sind für den Editor sichtbar,
        // außerhalb des Editors ist sie aber nur über IEditorMemento zugänglich.
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
