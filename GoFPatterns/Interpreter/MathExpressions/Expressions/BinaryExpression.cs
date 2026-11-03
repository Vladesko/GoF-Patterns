namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Нетерминальное выражение - содержит другие выражения и объединяет их результаты.
    // Один класс для всех бинарных операторов, операция передаётся как символ.
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
                _ => throw new NotSupportedException($"Операция '{operation}' не поддерживается.")
            };
        }

        public override string ToString() => $"({left} {operation} {right})";
    }
}
