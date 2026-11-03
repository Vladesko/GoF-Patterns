using GoFPatterns.Interpreter.MathExpressions.Expressions;

namespace GoFPatterns.Interpreter.MathExpressions
{
    public class DemonstrateMathExpressions
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Интерпретатор: математические выражения ===\n");

            // Синтаксическое дерево можно построить вручную: (price * count) - discount
            IExpression manual = new BinaryExpression(
                new BinaryExpression(new VariableExpression("price"), '*', new VariableExpression("count")),
                '-',
                new VariableExpression("discount"));

            var context = new Context();
            context.Set("price", 250);
            context.Set("count", 3);
            context.Set("discount", 50);
            Console.WriteLine($"Дерево, построенное вручную, {manual} = {manual.Interpret(context)}");

            // ... или с помощью парсера из строки.
            var parser = new ExpressionParser();
            string[] formulas = ["2 + 3 * 4", "(2 + 3) * 4", "price * count * (1 - tax)", "salary / 12"];
            context.Set("tax", 0.2);
            context.Set("salary", 60000);

            Console.WriteLine();
            foreach (var formula in formulas)
            {
                var tree = parser.Parse(formula);
                Console.WriteLine($"{formula,-28} дерево: {tree,-32} = {tree.Interpret(context)}");
            }

            // Одно и то же дерево интерпретируется с разными контекстами.
            var totalPrice = parser.Parse("price * count");
            Console.WriteLine("\nОдно дерево, разные контексты:");
            foreach (var (price, count) in new[] { (10.0, 2.0), (99.9, 1.0), (5.0, 100.0) })
            {
                context.Set("price", price);
                context.Set("count", count);
                Console.WriteLine($"  price={price}, count={count} -> {totalPrice.Interpret(context)}");
            }
        }
    }
}
