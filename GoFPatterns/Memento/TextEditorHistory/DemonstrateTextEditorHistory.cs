namespace GoFPatterns.Memento.TextEditorHistory
{
    public class DemonstrateTextEditorHistory
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Снимок: история текстового редактора ===\n");

            var editor = new TextEditor();
            var history = new EditorHistory(editor);

            history.Backup("пусто");
            editor.Type("Привет");
            Console.WriteLine($"  {editor}");

            history.Backup("привет");
            editor.Type(" мир");
            editor.SetFont("Times New Roman");
            Console.WriteLine($"  {editor}");

            history.Backup("привет мир");
            editor.MoveCursor(0);
            editor.Type("Ой! ");
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
