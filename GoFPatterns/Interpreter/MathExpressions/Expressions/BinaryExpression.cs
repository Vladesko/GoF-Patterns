namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Nonterminal expression - contains other expressions and combines their results.
    // One class for all binary operators, the operation is passed as a symbol.
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
                _ => throw new NotSupportedException($"Operation '{operation}' is not supported.")
            };
        }

        public override string ToString() => $"({left} {operation} {right})";
    }
}
