namespace GoFPatterns.Composite.FileSystem
{
    // Компонент - общий интерфейс для простых (файл) и составных (папка) элементов.
    // Клиент работает с любым элементом через этот класс, и ему неважно, что это за элемент.
    public abstract class FileSystemItem(string name)
    {
        public string Name { get; } = name;

        public abstract long GetSize();

        public abstract void Print(string indent = "");

        public abstract IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate);
    }
}
