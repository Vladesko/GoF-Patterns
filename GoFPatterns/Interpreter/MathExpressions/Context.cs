namespace GoFPatterns.Interpreter.MathExpressions
{
    // Контекст - глобальная информация для интерпретации: значения переменных.
    public class Context
    {
        private readonly Dictionary<string, double> _variables = [];

        public void Set(string name, double value) => _variables[name] = value;

        public double Get(string name) =>
            _variables.TryGetValue(name, out var value)
                ? value
                : throw new KeyNotFoundException($"Переменная '{name}' не определена.");
    }
}
