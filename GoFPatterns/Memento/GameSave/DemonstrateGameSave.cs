namespace GoFPatterns.Memento.GameSave
{
    public class DemonstrateGameSave
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: partidas guardadas ===\n");

            var hero = new Hero("Arthur");
            var saves = new SaveManager();

            hero.Travel("Bosque");
            hero.Fight(damage: 30, reward: 50);
            hero.Buy("poción de salud", 20);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "antes del castillo");

            hero.Travel("Castillo oscuro");
            hero.Fight(damage: 40, reward: 100);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "autoguardado");

            Console.WriteLine("\nPelea contra el jefe...");
            hero.Fight(damage: 500, reward: 1000);
            Console.WriteLine($"  {hero}  <- fin de la partida");

            Console.WriteLine("\nCargando el último autoguardado:");
            saves.TryLoad(hero, "autoguardado");
            Console.WriteLine($"  {hero}");

            Console.WriteLine("\nEl jugador decide prepararse mejor y carga una partida anterior:");
            saves.TryLoad(hero, "antes del castillo");
            Console.WriteLine($"  {hero}");

            saves.TryLoad(hero, "ranura 3");

            Console.WriteLine("\nTodas las partidas guardadas:");
            foreach (var slot in saves.Slots)
            {
                Console.WriteLine($"  {slot}");
            }
        }
    }
}
