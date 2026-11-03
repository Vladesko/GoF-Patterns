namespace GoFPatterns.Command.TextEditor.Commands
{
    // Конкретная команда, собранная из других команд: замена = удаление + вставка.
    public class ReplaceTextCommand(Document document, int position, int length, string newText) : ICommand
    {
        private readonly DeleteTextCommand _delete = new(document, position, length);
        private readonly InsertTextCommand _insert = new(document, position, newText);

        public string Name => $"Заменить на \"{newText}\"";

        public void Execute()
        {
            _delete.Execute();
            _insert.Execute();
        }

        // Отмена в обратном порядке.
        public void Undo()
        {
            _insert.Undo();
            _delete.Undo();
        }
    }
}
