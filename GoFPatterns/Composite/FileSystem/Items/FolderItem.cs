namespace GoFPatterns.Composite.FileSystem.Items
{
    // Compuesto: elemento con hijos. Los hijos pueden ser hojas u otros compuestos.
    // No hace el trabajo por sí mismo, sino que lo delega en los hijos y combina el resultado.
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

        // Llamada recursiva: el tamaño de la carpeta es la suma de los tamaños de todos los hijos.
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
