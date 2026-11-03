namespace GoFPatterns.Flyweight.TextFormatting
{
    // Flyweight factory - pool di stili. Stili uguali vengono restituiti come lo stesso oggetto.
    public class StyleFactory
    {
        private readonly Dictionary<CharacterStyle, CharacterStyle> _pool = [];

        public int Count => _pool.Count;

        public CharacterStyle Get(string font, int size, bool bold = false, string color = "nero")
        {
            var candidate = new CharacterStyle(font, size, bold, color);
            if (_pool.TryGetValue(candidate, out var existing))
                return existing;

            _pool[candidate] = candidate;
            return candidate;
        }
    }
}
