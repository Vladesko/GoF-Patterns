using GoFPatterns.Command.TextEditor.Commands;

namespace GoFPatterns.Command.TextEditor
{
    public class DemonstrateTextEditor
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: Text editor with Undo/Redo ===\n");

            var document = new Document();
            var history = new CommandHistory();

            void Show() => Console.WriteLine($"  Text: \"{document.Text}\"\n");

            history.Execute(new InsertTextCommand(document, 0, "Hello"));
            history.Execute(new InsertTextCommand(document, 5, " world"));
            Show();

            history.Execute(new ReplaceTextCommand(document, 6, 5, "patterns"));
            Show();

            history.Execute(new DeleteTextCommand(document, 0, 6));
            Show();

            history.Undo();
            Show();
            history.Undo();
            Show();
            history.Redo();
            Show();

            // New command clears redo history.
            history.Execute(new InsertTextCommand(document, document.Text.Length, "!"));
            history.Redo();
            Show();
        }
    }
}
