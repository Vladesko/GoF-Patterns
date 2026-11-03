namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Espressione non terminale - contiene altre espressioni e ne combina i risultati.
    // Una sola classe per tutti gli operatori binari, l'operazione viene passata come simbolo.
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
                _ => throw new NotSupportedException($"L'operazione '{operation}' non è supportata.")
            };
        }

        public override string ToString() => $"({left} {operation} {right})";
    }
}
