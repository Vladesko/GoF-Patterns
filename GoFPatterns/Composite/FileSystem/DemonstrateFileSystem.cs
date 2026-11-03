using GoFPatterns.Composite.FileSystem.Items;

namespace GoFPatterns.Composite.FileSystem
{
    public class DemonstrateFileSystem
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Компоновщик: файловая система ===\n");

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

            // Клиент вызывает один и тот же метод и для файла, и для целого дерева.
            FileSystemItem singleFile = new FileItem("notes.txt", 300);
            Console.WriteLine($"\nРазмер одного файла: {singleFile.GetSize()} Б");
            Console.WriteLine($"Размер всего проекта: {root.GetSize()} Б");

            Console.WriteLine("\nФайлы C# в проекте:");
            foreach (var item in root.Find(i => i.Name.EndsWith(".cs")))
            {
                Console.WriteLine($"  {item.Name}");
            }
        }
    }
}
