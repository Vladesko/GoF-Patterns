using GoFPatterns.Command.TextEditor.Commands;

namespace GoFPatterns.Command.TextEditor
{
    public class DemonstrateTextEditor
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Команда: текстовый редактор с Undo/Redo ===\n");

            var document = new Document();
            var history = new CommandHistory();

            void Show() => Console.WriteLine($"  Текст: \"{document.Text}\"\n");

            history.Execute(new InsertTextCommand(document, 0, "Салют"));
            history.Execute(new InsertTextCommand(document, 5, " народ"));
            Show();

            history.Execute(new ReplaceTextCommand(document, 6, 5, "паттерны"));
            Show();

            history.Execute(new DeleteTextCommand(document, 0, 6));
            Show();

            history.Undo();
            Show();
            history.Undo();
            Show();
            history.Redo();
            Show();

            // Новая команда очищает историю повтора.
            history.Execute(new InsertTextCommand(document, document.Text.Length, "!"));
            history.Redo();
            Show();
        }
    }
}
