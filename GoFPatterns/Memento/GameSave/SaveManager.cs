namespace GoFPatterns.Memento.GameSave
{
    // Cuidador (caretaker): guarda las partidas en ranuras con nombre. Puede leer los datos públicos del memento (para mostrar la lista de partidas),
    // pero cambia el estado del héroe solo mediante hero.Load().
    public class SaveManager
    {
        private readonly Dictionary<string, HeroMemento> _slots = [];

        public void Save(Hero hero, string slot)
        {
            _slots[slot] = hero.Save(slot);
            Console.WriteLine($"  [guardado] partida guardada en la ranura \"{slot}\"");
        }

        public bool TryLoad(Hero hero, string slot)
        {
            if (!_slots.TryGetValue(slot, out var memento))
            {
                Console.WriteLine($"  [guardado] la ranura \"{slot}\" está vacía");
                return false;
            }
            hero.Load(memento);
            Console.WriteLine($"  [guardado] partida cargada desde la ranura \"{slot}\"");
            return true;
        }

        public IEnumerable<string> Slots => _slots.Values.Select(m => $"{m.SlotName}: nv. {m.Level} en {m.Location}");
    }
}
