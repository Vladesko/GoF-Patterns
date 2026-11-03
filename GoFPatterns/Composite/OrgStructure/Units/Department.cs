namespace GoFPatterns.Composite.OrgStructure.Units
{
    // Компоновщик - отдел, который содержит сотрудников и подотделы.
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
            Console.WriteLine($"{new string(' ', depth * 2)}[{Name}] человек: {GetHeadcount()}, бюджет: {GetSalaryBudget():N0}");
            foreach (var unit in _units)
            {
                unit.Display(depth + 1);
            }
        }
    }
}
