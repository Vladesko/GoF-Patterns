namespace GoFPatterns.Composite.FileSystem
{
    // Komponente – gemeinsame Schnittstelle für einfache (Datei) und zusammengesetzte (Ordner) Elemente.
    // Der Client arbeitet über diese Klasse mit jedem Element, egal um welches es sich handelt.
    public abstract class FileSystemItem(string name)
    {
        public string Name { get; } = name;

        public abstract long GetSize();

        public abstract void Print(string indent = "");

        public abstract IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate);
    }
}
