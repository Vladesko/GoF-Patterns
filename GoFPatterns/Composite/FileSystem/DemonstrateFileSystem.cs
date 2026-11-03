using GoFPatterns.Composite.FileSystem.Items;

namespace GoFPatterns.Composite.FileSystem
{
    public class DemonstrateFileSystem
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: File system ===\n");

            var root = new FolderItem("Project")
                .Add(new FileItem("README.md", 1_200))
                .Add(new FolderItem("src")
                    .Add(new FileItem("Program.cs", 3_400))
                    .Add(new FolderItem("Models")
                        .Add(new FileItem("User.cs", 900))
                        .Add(new FileItem("Order.cs", 1_500))))
                .Add(new FolderItem("assets")
                    .Add(new FileItem("logo.png", 52_000)));

            root.Print();

            // Client calls the same method for a file and for a whole tree.
            FileSystemItem singleFile = new FileItem("notes.txt", 300);
            Console.WriteLine($"\nSize of single file: {singleFile.GetSize()} B");
            Console.WriteLine($"Size of whole project: {root.GetSize()} B");

            Console.WriteLine("\nC# files in project:");
            foreach (var item in root.Find(i => i.Name.EndsWith(".cs")))
            {
                Console.WriteLine($"  {item.Name}");
            }
        }
    }
}
