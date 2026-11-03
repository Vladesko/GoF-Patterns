namespace GoFPatterns.Flyweight.Forest
{
    // Клиент - хранит контексты (деревья) и использует фабрику для получения легковесов.
    public class ForestMap
    {
        private readonly List<Tree> _trees = [];
        private readonly TreeTypeFactory _factory = new();

        public int TreesCount => _trees.Count;
        public int TreeTypesCount => _factory.Count;

        public void PlantTree(int x, int y, string name, string color, string texturePath)
        {
            var type = _factory.GetTreeType(name, color, texturePath);
            _trees.Add(new Tree(x, y, type));
        }

        public IEnumerable<string> Draw(int count) => _trees.Take(count).Select(t => t.Draw());
    }
}
