using System.Text;

namespace GoFPatterns.Command.TextEditor
{
    // Receiver - contains real business logic. Knows nothing about commands.
    public class Document
    {
        private readonly StringBuilder _text = new();

        public string Text => _text.ToString();

        public void Insert(int position, string text) => _text.Insert(position, text);

        public void Delete(int position, int length) => _text.Remove(position, length);

        public string GetText(int position, int length) => _text.ToString(position, length);
    }
}
