namespace GoFPatterns.Memento.GameSave
{
    // Caretaker - stores saves in named slots. It can read public data of memento (to show list of saves),
    // but changes hero state only through hero.Load().
    public class SaveManager
    {
        private readonly Dictionary<string, HeroMemento> _slots = [];

        public void Save(Hero hero, string slot)
        {
            _slots[slot] = hero.Save(slot);
            Console.WriteLine($"  [save] game saved to slot \"{slot}\"");
        }

        public bool TryLoad(Hero hero, string slot)
        {
            if (!_slots.TryGetValue(slot, out var memento))
            {
                Console.WriteLine($"  [save] slot \"{slot}\" is empty");
                return false;
            }
            hero.Load(memento);
            Console.WriteLine($"  [save] game loaded from slot \"{slot}\"");
            return true;
        }

        public IEnumerable<string> Slots => _slots.Values.Select(m => $"{m.SlotName}: lvl {m.Level} at {m.Location}");
    }
}
