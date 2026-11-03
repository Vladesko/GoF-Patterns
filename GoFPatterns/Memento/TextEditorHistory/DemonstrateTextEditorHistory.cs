namespace GoFPatterns.Memento.TextEditorHistory
{
    public class DemonstrateTextEditorHistory
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: Text editor history ===\n");

            var editor = new TextEditor();
            var history = new EditorHistory(editor);

            history.Backup("empty");
            editor.Type("Hello");
            Console.WriteLine($"  {editor}");

            history.Backup("hello");
            editor.Type(" world");
            editor.SetFont("Times New Roman");
            Console.WriteLine($"  {editor}");

            history.Backup("hello world");
            editor.MoveCursor(0);
            editor.Type("Oops! ");
            Console.WriteLine($"  {editor}");

            history.ShowHistory();

            Console.WriteLine();
            history.Undo();
            Console.WriteLine($"  {editor}");
            history.Undo();
            Console.WriteLine($"  {editor}");
            history.Undo();
            Console.WriteLine($"  {editor}");
            history.Undo();
        }
    }
}
