namespace GoFPatterns.Memento.GameSave
{
    // Caretaker - conserva i salvataggi in slot con nome. Può leggere i dati pubblici del memento (per mostrare la lista dei salvataggi),
    // ma modifica lo stato dell'eroe solo tramite hero.Load().
    public class SaveManager
    {
        private readonly Dictionary<string, HeroMemento> _slots = [];

        public void Save(Hero hero, string slot)
        {
            _slots[slot] = hero.Save(slot);
            Console.WriteLine($"  [salvataggio] partita salvata nello slot \"{slot}\"");
        }

        public bool TryLoad(Hero hero, string slot)
        {
            if (!_slots.TryGetValue(slot, out var memento))
            {
                Console.WriteLine($"  [salvataggio] lo slot \"{slot}\" è vuoto");
                return false;
            }
            hero.Load(memento);
            Console.WriteLine($"  [salvataggio] partita caricata dallo slot \"{slot}\"");
            return true;
        }

        public IEnumerable<string> Slots => _slots.Values.Select(m => $"{m.SlotName}: liv. {m.Level}, luogo: {m.Location}");
    }
}
