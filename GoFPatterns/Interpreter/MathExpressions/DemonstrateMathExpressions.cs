using GoFPatterns.Interpreter.MathExpressions.Expressions;

namespace GoFPatterns.Interpreter.MathExpressions
{
    public class DemonstrateMathExpressions
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Interpreter: Math expressions ===\n");

            // Syntax tree can be built by hand: (price * count) - discount
            IExpression manual = new BinaryExpression(
                new BinaryExpression(new VariableExpression("price"), '*', new VariableExpression("count")),
                '-',
                new VariableExpression("discount"));

            var context = new Context();
            context.Set("price", 250);
            context.Set("count", 3);
            context.Set("discount", 50);
            Console.WriteLine($"Manual tree {manual} = {manual.Interpret(context)}");

            // ... or by parser from a string.
            var parser = new ExpressionParser();
            string[] formulas = ["2 + 3 * 4", "(2 + 3) * 4", "price * count * (1 - tax)", "salary / 12"];
            context.Set("tax", 0.2);
            context.Set("salary", 60000);

            Console.WriteLine();
            foreach (var formula in formulas)
            {
                var tree = parser.Parse(formula);
                Console.WriteLine($"{formula,-28} tree: {tree,-32} = {tree.Interpret(context)}");
            }

            // The same tree is interpreted with different contexts.
            var totalPrice = parser.Parse("price * count");
            Console.WriteLine("\nOne tree, different contexts:");
            foreach (var (price, count) in new[] { (10.0, 2.0), (99.9, 1.0), (5.0, 100.0) })
            {
                context.Set("price", price);
                context.Set("count", count);
                Console.WriteLine($"  price={price}, count={count} -> {totalPrice.Interpret(context)}");
            }
        }
    }
}
