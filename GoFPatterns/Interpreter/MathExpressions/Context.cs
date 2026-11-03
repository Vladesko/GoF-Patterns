namespace GoFPatterns.Interpreter.MathExpressions
{
    // Contexto: información global para la interpretación: los valores de las variables.
    public class Context
    {
        private readonly Dictionary<string, double> _variables = [];

        public void Set(string name, double value) => _variables[name] = value;

        public double Get(string name) =>
            _variables.TryGetValue(name, out var value)
                ? value
                : throw new KeyNotFoundException($"La variable '{name}' no está definida.");
    }
}
