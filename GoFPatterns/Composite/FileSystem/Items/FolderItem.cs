namespace GoFPatterns.Composite.FileSystem.Items
{
    // Компоновщик - элемент с дочерними элементами. Ими могут быть листья или другие компоновщики.
    // Сам он работу не выполняет, а делегирует её дочерним элементам и объединяет результат.
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

        // Рекурсивный вызов - размер папки равен сумме размеров всех дочерних элементов.
        public override long GetSize() => _children.Sum(c => c.GetSize());

        public override void Print(string indent = "")
        {
            Console.WriteLine($"{indent}+ {Name}/ ({GetSize()} Б)");
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
