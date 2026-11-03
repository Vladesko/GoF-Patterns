namespace GoFPatterns.Composite.FileSystem.Items
{
    // Composite - elemento con figli. I figli possono essere foglie o altri composite.
    // Non esegue il lavoro da solo, ma lo delega ai figli e combina il risultato.
    public class FolderItem(string name) : FileSystemItem(name)
    {
        private readonly List<FileSystemItem> _children = [];

        public IReadOnlyList<FileSystemItem> Children => _children;

        public FolderItem Add(FileSystemItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            _children.Add(item);
            return this;
        }

        public void Remove(FileSystemItem item) => _children.Remove(item);

        // Chiamata ricorsiva - la dimensione della cartella è la somma delle dimensioni di tutti i figli.
        public override long GetSize() => _children.Sum(c => c.GetSize());

        public override void Print(string indent = "")
        {
            Console.WriteLine($"{indent}+ {Name}/ ({GetSize()} B)");
            foreach (var child in _children)
            {
                child.Print(indent + "    ");
            }
        }

        public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate)
        {
            var self = predicate(this) ? new[] { this } : [];
            return self.Concat(_children.SelectMany(c => c.Find(predicate)));
        }
    }
}
