namespace GoFPatterns.Interpreter.MathExpressions
{
    // Kontext – globale Informationen für die Interpretation: die Werte der Variablen.
    public class Context
    {
        private readonly Dictionary<string, double> _variables = [];

        public void Set(string name, double value) => _variables[name] = value;

        public double Get(string name) =>
            _variables.TryGetValue(name, out var value)
                ? value
                : throw new KeyNotFoundException($"Variable '{name}' ist nicht definiert.");
    }
}
