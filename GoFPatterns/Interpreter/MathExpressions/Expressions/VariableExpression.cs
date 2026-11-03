namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Espressione terminale - variabile. Il suo valore viene preso dal contesto.
    public class VariableExpression(string name) : IExpression
    {
        public double Interpret(Context context) => context.Get(name);

        public override string ToString() => name;
    }
}
