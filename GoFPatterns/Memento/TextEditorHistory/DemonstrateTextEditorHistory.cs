namespace GoFPatterns.Memento.TextEditorHistory
{
    public class DemonstrateTextEditorHistory
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: Verlauf im Texteditor ===\n");

            var editor = new TextEditor();
            var history = new EditorHistory(editor);

            history.Backup("leer");
            editor.Type("Hallo");
            Console.WriteLine($"  {editor}");

            history.Backup("hallo");
            editor.Type(" Welt");
            editor.SetFont("Times New Roman");
            Console.WriteLine($"  {editor}");

            history.Backup("hallo welt");
            editor.MoveCursor(0);
            editor.Type("Hoppla! ");
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
