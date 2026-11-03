namespace GoFPatterns.Composite.FileSystem
{
    // Component - common interface for simple (file) and complex (folder) elements.
    // Client works with any element through this class and doesn't care what it is.
    public abstract class FileSystemItem(string name)
    {
        public string Name { get; } = name;

        public abstract long GetSize();

        public abstract void Print(string indent = "");

        public abstract IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate);
    }
}
