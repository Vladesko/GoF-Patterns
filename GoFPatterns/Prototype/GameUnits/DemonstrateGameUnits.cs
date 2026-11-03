using GoFPatterns.Prototype.GameUnits.Units;

namespace GoFPatterns.Prototype.GameUnits
{
    public class DemonstrateGameUnits
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: generatore di unità di gioco ===\n");

            // I prototype vengono configurati una sola volta - ad esempio, caricati dalla configurazione del gioco.
            var eliteArcher = new Archer { Name = "Arciere d'élite", Health = 120, Arrows = 50 };
            var fireMage = new Mage { Name = "Mago del fuoco", Health = 80, Spells = ["Palla di fuoco", "Muro di fiamme"] };

            var spawners = new List<Spawner> { new(eliteArcher), new(fireMage) };

            for (var i = 0; i < 2; i++)
            {
                foreach (var spawner in spawners)
                {
                    Console.WriteLine(spawner.Spawn(new Position(i * 10, i * 5)));
                }
            }

            // Clonazione di un record con l'espressione "with".
            var start = new Position(1, 1);
            var moved = start with { X = 7 };
            Console.WriteLine($"\nClone del record: {start} -> {moved}");
        }
    }
}
