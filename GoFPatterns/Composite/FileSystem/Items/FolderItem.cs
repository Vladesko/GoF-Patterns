namespace GoFPatterns.Composite.FileSystem.Items
{
    // Composite - element with children. Children can be leaves or other composites.
    // It doesn't do work itself, but delegates it to children and combines the result.
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

        // Recursive call - size of folder is a sum of sizes of all children.
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
