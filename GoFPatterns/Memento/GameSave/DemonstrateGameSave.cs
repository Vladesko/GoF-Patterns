namespace GoFPatterns.Memento.GameSave
{
    public class DemonstrateGameSave
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Снимок: сохранения игры ===\n");

            var hero = new Hero("Артур");
            var saves = new SaveManager();

            hero.Travel("Лес");
            hero.Fight(damage: 30, reward: 50);
            hero.Buy("зелье здоровья", 20);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "перед замком");

            hero.Travel("Тёмный замок");
            hero.Fight(damage: 40, reward: 100);
            Console.WriteLine($"  {hero}");
            saves.Save(hero, "автосохранение");

            Console.WriteLine("\nБитва с боссом...");
            hero.Fight(damage: 500, reward: 1000);
            Console.WriteLine($"  {hero}  <- игра окончена");

            Console.WriteLine("\nЗагружаем последнее автосохранение:");
            saves.TryLoad(hero, "автосохранение");
            Console.WriteLine($"  {hero}");

            Console.WriteLine("\nИгрок решает лучше подготовиться и загружает более раннее сохранение:");
            saves.TryLoad(hero, "перед замком");
            Console.WriteLine($"  {hero}");

            saves.TryLoad(hero, "слот 3");

            Console.WriteLine("\nВсе сохранения:");
            foreach (var slot in saves.Slots)
            {
                Console.WriteLine($"  {slot}");
            }
        }
    }
}
