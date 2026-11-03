using GoFPatterns.Composite.OrgStructure.Units;

namespace GoFPatterns.Composite.OrgStructure
{
    public class DemonstrateOrgStructure
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: Organization structure ===\n");

            var backend = new Department("Backend").Add(
                new Employee("Anna", "Team lead", 4_000),
                new Employee("Ivan", "Developer", 3_000),
                new Employee("Oleg", "Developer", 2_800));

            var frontend = new Department("Frontend").Add(
                new Employee("Maria", "Developer", 2_900));

            var development = new Department("Development").Add(
                new Employee("Pavel", "CTO", 6_000),
                backend,
                frontend);

            var company = new Department("Company").Add(
                new Employee("Elena", "CEO", 8_000),
                development,
                new Department("HR").Add(new Employee("Olga", "Recruiter", 2_000)));

            company.Display();

            // Any unit - employee or department - can be analyzed in the same way.
            IOrgUnit[] units = [company, development, backend, new Employee("Intern", "Trainee", 800)];
            Console.WriteLine();
            foreach (var unit in units)
            {
                Console.WriteLine($"{unit.Name,-12} people: {unit.GetHeadcount(),2}, budget: {unit.GetSalaryBudget(),8:N0}");
            }
        }
    }
}
