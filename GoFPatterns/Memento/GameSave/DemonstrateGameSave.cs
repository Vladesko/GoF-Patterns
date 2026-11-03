namespace GoFPatterns.Memento.GameSave
{
    public class DemonstrateGameSave
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: Spielstände ===\n");

            var hero = new Hero("Arthur");
            var saves = new SaveManager();

            hero.Travel("Wald");
            hero.Fight(damage: 30, reward: 50);
            hero.Buy("Heiltrank", 20);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "vor der Burg");

            hero.Travel("Dunkle Burg");
            hero.Fight(damage: 40, reward: 100);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "Autosave");

            Console.WriteLine("\nBosskampf...");
            hero.Fight(damage: 500, reward: 1000);
            Console.WriteLine($"  {hero}  <- Game Over");

            Console.WriteLine("\nLetzter Autosave wird geladen:");
            saves.TryLoad(hero, "Autosave");
            Console.WriteLine($"  {hero}");

            Console.WriteLine("\nDer Spieler will sich besser vorbereiten und lädt einen früheren Spielstand:");
            saves.TryLoad(hero, "vor der Burg");
            Console.WriteLine($"  {hero}");

            saves.TryLoad(hero, "Slot 3");

            Console.WriteLine("\nAlle Spielstände:");
            foreach (var slot in saves.Slots)
            {
                Console.WriteLine($"  {slot}");
            }
        }
    }
}
