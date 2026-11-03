namespace GoFPatterns.Memento.TextEditorHistory
{
    public class DemonstrateTextEditorHistory
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: historial del editor de texto ===\n");

            var editor = new TextEditor();
            var history = new EditorHistory(editor);

            history.Backup("vacío");
            editor.Type("Hola");
            Console.WriteLine($"  {editor}");

            history.Backup("hola");
            editor.Type(" mundo");
            editor.SetFont("Times New Roman");
            Console.WriteLine($"  {editor}");

            history.Backup("hola mundo");
            editor.MoveCursor(0);
            editor.Type("¡Uy! ");
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
