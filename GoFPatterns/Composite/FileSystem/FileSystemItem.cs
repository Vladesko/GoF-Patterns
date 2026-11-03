namespace GoFPatterns.Composite.FileSystem
{
    // Componente: interfaz común para elementos simples (archivo) y complejos (carpeta).
    // El cliente trabaja con cualquier elemento a través de esta clase y no le importa qué es.
    public abstract class FileSystemItem(string name)
    {
        public string Name { get; } = name;

        public abstract long GetSize();

        public abstract void Print(string indent = "");

        public abstract IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate);
    }
}
