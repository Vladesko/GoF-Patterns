namespace GoFPatterns.Memento.GameSave
{
    // Verwalter (Caretaker) – speichert Spielstände in benannten Slots. Er darf die öffentlichen Daten des Mementos lesen (für die Liste der Spielstände),
    // ändert den Zustand des Helden aber nur über hero.Load().
    public class SaveManager
    {
        private readonly Dictionary<string, HeroMemento> _slots = [];

        public void Save(Hero hero, string slot)
        {
            _slots[slot] = hero.Save(slot);
            Console.WriteLine($"  [save] Spiel in Slot \"{slot}\" gespeichert");
        }

        public bool TryLoad(Hero hero, string slot)
        {
            if (!_slots.TryGetValue(slot, out var memento))
            {
                Console.WriteLine($"  [save] Slot \"{slot}\" ist leer");
                return false;
            }
            hero.Load(memento);
            Console.WriteLine($"  [save] Spiel aus Slot \"{slot}\" geladen");
            return true;
        }

        public IEnumerable<string> Slots => _slots.Values.Select(m => $"{m.SlotName}: Lvl {m.Level}, Ort: {m.Location}");
    }
}
