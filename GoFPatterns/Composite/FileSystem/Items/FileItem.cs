namespace GoFPatterns.Composite.FileSystem.Items
{
    // Hoja: elemento sin hijos. Realiza el trabajo real.
    public class FileItem(string name, long size) : FileSystemItem(name)
    {
        public override long GetSize() => size;

        public override void Print(string indent = "") =>
            Console.WriteLine($"{indent}- {Name} ({size} B)");

        public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate) =>
            predicate(this) ? [this] : [];
    }
}
