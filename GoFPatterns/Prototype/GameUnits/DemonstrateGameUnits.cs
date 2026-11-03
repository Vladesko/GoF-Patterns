using GoFPatterns.Prototype.GameUnits.Units;

namespace GoFPatterns.Prototype.GameUnits
{
    public class DemonstrateGameUnits
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Прототип: создание игровых юнитов ===\n");

            // Прототипы настраиваются один раз - например, загружаются из игрового конфига.
            var eliteArcher = new Archer { Name = "Элитный лучник", Health = 120, Arrows = 50 };
            var fireMage = new Mage { Name = "Огненный маг", Health = 80, Spells = ["Огненный шар", "Стена огня"] };

            var spawners = new List<Spawner> { new(eliteArcher), new(fireMage) };

            for (var i = 0; i < 2; i++)
            {
                foreach (var spawner in spawners)
                {
                    Console.WriteLine(spawner.Spawn(new Position(i * 10, i * 5)));
                }
            }

            // Клонирование record с помощью выражения "with".
            var start = new Position(1, 1);
            var moved = start with { X = 7 };
            Console.WriteLine($"\nКлон record: {start} -> {moved}");
        }
    }
}
