namespace GoFPatterns.Interpreter.MathExpressions
{
    // Context - global information for interpretation: values of variables.
    public class Context
    {
        private readonly Dictionary<string, double> _variables = [];

        public void Set(string name, double value) => _variables[name] = value;

        public double Get(string name) =>
            _variables.TryGetValue(name, out var value)
                ? value
                : throw new KeyNotFoundException($"Variable '{name}' is not defined.");
    }
}
