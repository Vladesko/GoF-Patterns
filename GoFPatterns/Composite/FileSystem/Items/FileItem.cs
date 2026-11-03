namespace GoFPatterns.Composite.FileSystem.Items
{
    // Leaf - elemento senza figli. Esegue il lavoro reale.
    public class FileItem(string name, long size) : FileSystemItem(name)
    {
        public override long GetSize() => size;

        public override void Print(string indent = "") =>
            Console.WriteLine($"{indent}- {Name} ({size} B)");

        public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate) =>
            predicate(this) ? [this] : [];
    }
}
