using GoFPatterns.Prototype.GameUnits.Units;

namespace GoFPatterns.Prototype.GameUnits
{
    public class DemonstrateGameUnits
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Prototype: generador de unidades de juego ===\n");

            // Los prototipos se configuran una sola vez, por ejemplo, cargados desde la configuración del juego.
            var eliteArcher = new Archer { Name = "Arquero de élite", Health = 120, Arrows = 50 };
            var fireMage = new Mage { Name = "Mago de fuego", Health = 80, Spells = ["Bola de fuego", "Muro de llamas"] };

            var spawners = new List<Spawner> { new(eliteArcher), new(fireMage) };

            for (var i = 0; i < 2; i++)
            {
                foreach (var spawner in spawners)
                {
                    Console.WriteLine(spawner.Spawn(new Position(i * 10, i * 5)));
                }
            }

            // Clonación de un record con la expresión "with".
            var start = new Position(1, 1);
            var moved = start with { X = 7 };
            Console.WriteLine($"\nClon del record: {start} -> {moved}");
        }
    }
}
