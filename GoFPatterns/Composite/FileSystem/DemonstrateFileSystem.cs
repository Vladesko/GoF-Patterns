using GoFPatterns.Composite.FileSystem.Items;

namespace GoFPatterns.Composite.FileSystem
{
    public class DemonstrateFileSystem
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: Dateisystem ===\n");

            var root = new FolderItem("Projekt")
                .Add(new FileItem("README.md", 1_200))
                .Add(new FolderItem("src")
                    .Add(new FileItem("Program.cs", 3_400))
                    .Add(new FolderItem("Models")
                        .Add(new FileItem("User.cs", 900))
                        .Add(new FileItem("Order.cs", 1_500))))
                .Add(new FolderItem("assets")
                    .Add(new FileItem("logo.png", 52_000)));

            root.Print();

            // Der Client ruft dieselbe Methode für eine Datei und für einen ganzen Baum auf.
            FileSystemItem singleFile = new FileItem("notes.txt", 300);
            Console.WriteLine($"\nGröße einer einzelnen Datei: {singleFile.GetSize()} B");
            Console.WriteLine($"Größe des gesamten Projekts: {root.GetSize()} B");

            Console.WriteLine("\nC#-Dateien im Projekt:");
            foreach (var item in root.Find(i => i.Name.EndsWith(".cs")))
            {
                Console.WriteLine($"  {item.Name}");
            }
        }
    }
}
