using GoFPatterns.Prototype.GameUnits.Units;

namespace GoFPatterns.Prototype.GameUnits
{
    public class DemonstrateGameUnits
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: Game units spawner ===\n");

            // Prototypes are configured once - for example, loaded from game config.
            var eliteArcher = new Archer { Name = "Elite archer", Health = 120, Arrows = 50 };
            var fireMage = new Mage { Name = "Fire mage", Health = 80, Spells = ["Fireball", "Flame wall"] };

            var spawners = new List<Spawner> { new(eliteArcher), new(fireMage) };

            for (var i = 0; i < 2; i++)
            {
                foreach (var spawner in spawners)
                {
                    Console.WriteLine(spawner.Spawn(new Position(i * 10, i * 5)));
                }
            }

            // Record clone with "with" expression.
            var start = new Position(1, 1);
            var moved = start with { X = 7 };
            Console.WriteLine($"\nRecord clone: {start} -> {moved}");
        }
    }
}
