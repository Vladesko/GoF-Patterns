using GoFPatterns.Composite.FileSystem.Items;

namespace GoFPatterns.Composite.FileSystem
{
    public class DemonstrateFileSystem
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: File system ===\n");

            var root = new FolderItem("Progetto")
                .Add(new FileItem("README.md", 1_200))
                .Add(new FolderItem("src")
                    .Add(new FileItem("Program.cs", 3_400))
                    .Add(new FolderItem("Models")
                        .Add(new FileItem("User.cs", 900))
                        .Add(new FileItem("Order.cs", 1_500))))
                .Add(new FolderItem("assets")
                    .Add(new FileItem("logo.png", 52_000)));

            root.Print();

            // Il client chiama lo stesso metodo per un file e per un intero albero.
            FileSystemItem singleFile = new FileItem("note.txt", 300);
            Console.WriteLine($"\nDimensione del singolo file: {singleFile.GetSize()} B");
            Console.WriteLine($"Dimensione dell'intero progetto: {root.GetSize()} B");

            Console.WriteLine("\nFile C# nel progetto:");
            foreach (var item in root.Find(i => i.Name.EndsWith(".cs")))
            {
                Console.WriteLine($"  {item.Name}");
            }
        }
    }
}
