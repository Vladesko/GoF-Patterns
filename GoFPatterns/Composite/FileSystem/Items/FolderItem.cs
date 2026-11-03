namespace GoFPatterns.Composite.FileSystem.Items
{
    // Kompositum – Element mit Kindern. Kinder können Blätter oder weitere Kompositen sein.
    // Es erledigt die Arbeit nicht selbst, sondern delegiert sie an die Kinder und fasst das Ergebnis zusammen.
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

        // Rekursiver Aufruf – die Größe eines Ordners ist die Summe der Größen aller Kinder.
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
