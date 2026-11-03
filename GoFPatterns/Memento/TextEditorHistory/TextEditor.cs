namespace GoFPatterns.Memento.TextEditorHistory
{
    // Originator - oggetto il cui stato viene salvato e ripristinato.
    // Solo lui sa come creare uno snapshot e come ripristinarsi da esso.
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
            // L'editor accetta solo i propri snapshot.
            if (memento is not Snapshot snapshot)
                throw new ArgumentException("Tipo di memento sconosciuto.", nameof(memento));

            _text = snapshot.Text;
            _cursor = snapshot.Cursor;
            _font = snapshot.Font;
        }

        public override string ToString() => $"\"{_text.Insert(_cursor, "|")}\" [{_font}]";

        // Memento - classe annidata privata. I suoi campi sono visibili all'editor,
        // ma all'esterno dell'editor è accessibile solo tramite IEditorMemento.
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
