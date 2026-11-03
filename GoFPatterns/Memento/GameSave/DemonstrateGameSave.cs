namespace GoFPatterns.Memento.GameSave
{
    public class DemonstrateGameSave
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Memento: Salvataggi di gioco ===\n");

            var hero = new Hero("Artù");
            var saves = new SaveManager();

            hero.Travel("Foresta");
            hero.Fight(damage: 30, reward: 50);
            hero.Buy("pozione di salute", 20);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "prima del castello");

            hero.Travel("Castello oscuro");
            hero.Fight(damage: 40, reward: 100);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "salvataggio automatico");

            Console.WriteLine("\nCombattimento con il boss...");
            hero.Fight(damage: 500, reward: 1000);
            Console.WriteLine($"  {hero}  <- game over");

            Console.WriteLine("\nCaricamento dell'ultimo salvataggio automatico:");
            saves.TryLoad(hero, "salvataggio automatico");
            Console.WriteLine($"  {hero}");

            Console.WriteLine("\nIl giocatore decide di prepararsi meglio e carica un salvataggio precedente:");
            saves.TryLoad(hero, "prima del castello");
            Console.WriteLine($"  {hero}");

            saves.TryLoad(hero, "slot 3");

            Console.WriteLine("\nTutti i salvataggi:");
            foreach (var slot in saves.Slots)
            {
                Console.WriteLine($"  {slot}");
            }
        }
    }
}
