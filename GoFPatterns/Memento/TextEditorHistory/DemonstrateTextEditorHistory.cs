namespace GoFPatterns.Memento.TextEditorHistory
{
    public class DemonstrateTextEditorHistory
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: Cronologia dell'editor di testo ===\n");

            var editor = new TextEditor();
            var history = new EditorHistory(editor);

            history.Backup("vuoto");
            editor.Type("Ciao");
            Console.WriteLine($"  {editor}");

            history.Backup("ciao");
            editor.Type(" mondo");
            editor.SetFont("Times New Roman");
            Console.WriteLine($"  {editor}");

            history.Backup("ciao mondo");
            editor.MoveCursor(0);
            editor.Type("Ops! ");
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
