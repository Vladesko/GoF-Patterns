namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Expresión no terminal: contiene otras expresiones y combina sus resultados.
    // Una sola clase para todos los operadores binarios; la operación se pasa como símbolo.
    public class BinaryExpression(IExpression left, char operation, IExpression right) : IExpression
    {
        public double Interpret(Context context)
        {
            var l = left.Interpret(context);
            var r = right.Interpret(context);
            return operation switch
            {
                '+' => l + r,
                '-' => l - r,
                '*' => l * r,
                '/' => r == 0 ? throw new DivideByZeroException() : l / r,
                _ => throw new NotSupportedException($"La operación '{operation}' no está soportada.")
            };
        }

        public override string ToString() => $"({left} {operation} {right})";
    }
}
