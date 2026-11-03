using GoFPatterns.Composite.FileSystem.Items;

namespace GoFPatterns.Composite.FileSystem
{
    public class DemonstrateFileSystem
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: sistema de archivos ===\n");

            var root = new FolderItem("Proyecto")
                .Add(new FileItem("README.md", 1_200))
                .Add(new FolderItem("src")
                    .Add(new FileItem("Program.cs", 3_400))
                    .Add(new FolderItem("Models")
                        .Add(new FileItem("User.cs", 900))
                        .Add(new FileItem("Order.cs", 1_500))))
                .Add(new FolderItem("assets")
                    .Add(new FileItem("logo.png", 52_000)));

            root.Print();

            // El cliente llama al mismo método para un archivo y para un árbol completo.
            FileSystemItem singleFile = new FileItem("notes.txt", 300);
            Console.WriteLine($"\nTamaño de un solo archivo: {singleFile.GetSize()} B");
            Console.WriteLine($"Tamaño de todo el proyecto: {root.GetSize()} B");

            Console.WriteLine("\nArchivos C# del proyecto:");
            foreach (var item in root.Find(i => i.Name.EndsWith(".cs")))
            {
                Console.WriteLine($"  {item.Name}");
            }
        }
    }
}
