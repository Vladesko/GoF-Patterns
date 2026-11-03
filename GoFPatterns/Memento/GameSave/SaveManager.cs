namespace GoFPatterns.Memento.GameSave
{
    // Опекун (Caretaker) - хранит сохранения в именованных слотах. Он может читать публичные данные снимка (чтобы показать список сохранений),
    // но изменяет состояние героя только через hero.Load().
    public class SaveManager
    {
        private readonly Dictionary<string, HeroMemento> _slots = [];

        public void Save(Hero hero, string slot)
        {
            _slots[slot] = hero.Save(slot);
            Console.WriteLine($"  [сохранение] игра сохранена в слот \"{slot}\"");
        }

        public bool TryLoad(Hero hero, string slot)
        {
            if (!_slots.TryGetValue(slot, out var memento))
            {
                Console.WriteLine($"  [сохранение] слот \"{slot}\" пуст");
                return false;
            }
            hero.Load(memento);
            Console.WriteLine($"  [сохранение] игра загружена из слота \"{slot}\"");
            return true;
        }

        public IEnumerable<string> Slots => _slots.Values.Select(m => $"{m.SlotName}: ур. {m.Level}, локация: {m.Location}");
    }
}
