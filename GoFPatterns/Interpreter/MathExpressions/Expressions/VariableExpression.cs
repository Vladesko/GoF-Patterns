namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Терминальное выражение - переменная. Её значение берётся из контекста.
    public class VariableExpression(string name) : IExpression
    {
        public double Interpret(Context context) => context.Get(name);

        public override string ToString() => name;
    }
}
