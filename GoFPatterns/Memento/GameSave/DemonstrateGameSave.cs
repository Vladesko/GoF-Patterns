namespace GoFPatterns.Memento.GameSave
{
    public class DemonstrateGameSave
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: Game saves ===\n");

            var hero = new Hero("Arthur");
            var saves = new SaveManager();

            hero.Travel("Forest");
            hero.Fight(damage: 30, reward: 50);
            hero.Buy("health potion", 20);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "before castle");

            hero.Travel("Dark castle");
            hero.Fight(damage: 40, reward: 100);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "autosave");

            Console.WriteLine("\nBoss fight...");
            hero.Fight(damage: 500, reward: 1000);
            Console.WriteLine($"  {hero}  <- game over");

            Console.WriteLine("\nLoading last autosave:");
            saves.TryLoad(hero, "autosave");
            Console.WriteLine($"  {hero}");

            Console.WriteLine("\nPlayer decides to prepare better and loads earlier save:");
            saves.TryLoad(hero, "before castle");
            Console.WriteLine($"  {hero}");

            saves.TryLoad(hero, "slot 3");

            Console.WriteLine("\nAll saves:");
            foreach (var slot in saves.Slots)
            {
                Console.WriteLine($"  {slot}");
            }
        }
    }
}
