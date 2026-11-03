namespace GoFPatterns.Flyweight.TextFormatting
{
    // Fábrica de flyweights: pool de estilos. Los estilos iguales se devuelven como el mismo objeto.
    public class StyleFactory
    {
        private readonly Dictionary<CharacterStyle, CharacterStyle> _pool = [];

        public int Count => _pool.Count;

        public CharacterStyle Get(string font, int size, bool bold = false, string color = "negro")
        {
            var candidate = new CharacterStyle(font, size, bold, color);
            if (_pool.TryGetValue(candidate, out var existing))
                return existing;

            _pool[candidate] = candidate;
            return candidate;
        }
    }
}
