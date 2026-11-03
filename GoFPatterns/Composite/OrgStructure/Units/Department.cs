namespace GoFPatterns.Composite.OrgStructure.Units
{
    // Composite - department which contains employees and sub-departments.
    public class Department(string name) : IOrgUnit
    {
        private readonly List<IOrgUnit> _units = [];

        public string Name { get; } = name;

        public Department Add(params IOrgUnit[] units)
        {
            _units.AddRange(units);
            return this;
        }

        public decimal GetSalaryBudget() => _units.Sum(u => u.GetSalaryBudget());

        public int GetHeadcount() => _units.Sum(u => u.GetHeadcount());

        public void Display(int depth = 0)
        {
            Console.WriteLine($"{new string(' ', depth * 2)}[{Name}] people: {GetHeadcount()}, budget: {GetSalaryBudget():N0}");
            foreach (var unit in _units)
            {
                unit.Display(depth + 1);
            }
        }
    }
}
