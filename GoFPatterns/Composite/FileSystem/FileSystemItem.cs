namespace GoFPatterns.Composite.FileSystem
{
    // Component - interfaccia comune per gli elementi semplici (file) e complessi (cartella).
    // Il client lavora con qualsiasi elemento tramite questa classe e non gli importa di cosa si tratti.
    public abstract class FileSystemItem(string name)
    {
        public string Name { get; } = name;

        public abstract long GetSize();

        public abstract void Print(string indent = "");

        public abstract IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate);
    }
}
