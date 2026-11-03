using GoFPatterns.Prototype.GameUnits.Units;

namespace GoFPatterns.Prototype.GameUnits
{
    public class DemonstrateGameUnits
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: Spawner für Spieleinheiten ===\n");

            // Prototypen werden einmal konfiguriert – z. B. aus der Spielkonfiguration geladen.
            var eliteArcher = new Archer { Name = "Elite-Bogenschütze", Health = 120, Arrows = 50 };
            var fireMage = new Mage { Name = "Feuermagier", Health = 80, Spells = ["Feuerball", "Flammenwand"] };

            var spawners = new List<Spawner> { new(eliteArcher), new(fireMage) };

            for (var i = 0; i < 2; i++)
            {
                foreach (var spawner in spawners)
                {
                    Console.WriteLine(spawner.Spawn(new Position(i * 10, i * 5)));
                }
            }

            // Klonen eines Records mit dem Ausdruck "with".
            var start = new Position(1, 1);
            var moved = start with { X = 7 };
            Console.WriteLine($"\nRecord-Klon: {start} -> {moved}");
        }
    }
}
