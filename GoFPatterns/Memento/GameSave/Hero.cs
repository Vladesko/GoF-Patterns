namespace GoFPatterns.Memento.GameSave
{
    // Originador: el héroe del juego.
    public class Hero(string name)
    {
        private readonly List<string> _inventory = ["espada de madera"];

        public string Name { get; } = name;
        public int Level { get; private set; } = 1;
        public int Health { get; private set; } = 100;
        public int Gold { get; private set; }
        public string Location { get; private set; } = "Aldea";

        public void Travel(string location) => Location = location;

        public void Fight(int damage, int reward)
        {
            Health = Math.Max(0, Health - damage);
            if (Health > 0)
            {
                Gold += reward;
                Level++;
            }
        }

        public void Buy(string item, int price)
        {
            if (Gold < price) return;
            Gold -= price;
            _inventory.Add(item);
        }

        public HeroMemento Save(string slotName) =>
            new(slotName, Level, Health, Gold, Location, [.. _inventory]);

        public void Load(HeroMemento memento)
        {
            Level = memento.Level;
            Health = memento.Health;
            Gold = memento.Gold;
            Location = memento.Location;
            _inventory.Clear();
            _inventory.AddRange(memento.Inventory);
        }

        public override string ToString() =>
            $"{Name} nv. {Level}, HP {Health}, oro {Gold}, en {Location}, objetos: {string.Join(", ", _inventory)}";
    }
}
