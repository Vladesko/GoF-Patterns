namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Terminaler Ausdruck – Variable. Ihr Wert wird aus dem Kontext geholt.
    public class VariableExpression(string name) : IExpression
    {
        public double Interpret(Context context) => context.Get(name);

        public override string ToString() => name;
    }
}
