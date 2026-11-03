namespace GoFPatterns.Composite.OrgStructure.Units
{
    // Leaf - employee.
    public class Employee(string name, string position, decimal salary) : IOrgUnit
    {
        public string Name { get; } = name;

        public decimal GetSalaryBudget() => salary;

        public int GetHeadcount() => 1;

        public void Display(int depth = 0) =>
            Console.WriteLine($"{new string(' ', depth * 2)}- {Name}, {position}: {salary:N0}");
    }
}
