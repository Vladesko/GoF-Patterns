namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Terminal expression - variable. Its value is taken from context.
    public class VariableExpression(string name) : IExpression
    {
        public double Interpret(Context context) => context.Get(name);

        public override string ToString() => name;
    }
}
