using GoFPatterns.Command.TextEditor.Commands;

namespace GoFPatterns.Command.TextEditor
{
    public class DemonstrateTextEditor
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: editor de texto con Undo/Redo ===\n");

            var document = new Document();
            var history = new CommandHistory();

            void Show() => Console.WriteLine($"  Texto: \"{document.Text}\"\n");

            history.Execute(new InsertTextCommand(document, 0, "Hola"));
            history.Execute(new InsertTextCommand(document, 4, " mundo"));
            Show();

            history.Execute(new ReplaceTextCommand(document, 5, 5, "patrones"));
            Show();

            history.Execute(new DeleteTextCommand(document, 0, 5));
            Show();

            history.Undo();
            Show();
            history.Undo();
            Show();
            history.Redo();
            Show();

            // Un comando nuevo borra el historial de rehacer.
            history.Execute(new InsertTextCommand(document, document.Text.Length, "!"));
            history.Redo();
            Show();
        }
    }
}
