namespace GoFPatterns.Composite.FileSystem.Items
{
    // Лист - элемент без дочерних элементов. Выполняет реальную работу.
    public class FileItem(string name, long size) : FileSystemItem(name)
    {
        public override long GetSize() => size;

        public override void Print(string indent = "") =>
            Console.WriteLine($"{indent}- {Name} ({size} Б)");

        public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate) =>
            predicate(this) ? [this] : [];
    }
}
