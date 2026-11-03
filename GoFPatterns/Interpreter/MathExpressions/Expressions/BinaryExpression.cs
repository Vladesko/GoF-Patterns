namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Nichtterminaler Ausdruck – enthält andere Ausdrücke und verknüpft ihre Ergebnisse.
    // Eine Klasse für alle binären Operatoren, die Operation wird als Zeichen übergeben.
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
                _ => throw new NotSupportedException($"Operation '{operation}' wird nicht unterstützt.")
            };
        }

        public override string ToString() => $"({left} {operation} {right})";
    }
}
