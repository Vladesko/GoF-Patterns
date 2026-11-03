namespace GoFPatterns.Interpreter.MathExpressions
{
    // Context - informazioni globali per l'interpretazione: i valori delle variabili.
    public class Context
    {
        private readonly Dictionary<string, double> _variables = [];

        public void Set(string name, double value) => _variables[name] = value;

        public double Get(string name) =>
            _variables.TryGetValue(name, out var value)
                ? value
                : throw new KeyNotFoundException($"La variabile '{name}' non è definita.");
    }
}
