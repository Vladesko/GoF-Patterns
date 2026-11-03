using GoFPatterns.Composite.OrgStructure.Units;

namespace GoFPatterns.Composite.OrgStructure
{
    public class DemonstrateOrgStructure
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: Organisationsstruktur ===\n");

            var backend = new Department("Backend").Add(
                new Employee("Anna", "Teamleiterin", 4_000),
                new Employee("Ivan", "Entwickler", 3_000),
                new Employee("Oleg", "Entwickler", 2_800));

            var frontend = new Department("Frontend").Add(
                new Employee("Maria", "Entwicklerin", 2_900));

            var development = new Department("Entwicklung").Add(
                new Employee("Pavel", "CTO", 6_000),
                backend,
                frontend);

            var company = new Department("Unternehmen").Add(
                new Employee("Elena", "CEO", 8_000),
                development,
                new Department("HR").Add(new Employee("Olga", "Recruiterin", 2_000)));

            company.Display();

            // Jede Einheit – Mitarbeiter oder Abteilung – kann auf dieselbe Weise ausgewertet werden.
            IOrgUnit[] units = [company, development, backend, new Employee("Praktikant", "Trainee", 800)];
            Console.WriteLine();
            foreach (var unit in units)
            {
                Console.WriteLine($"{unit.Name,-12} Personen: {unit.GetHeadcount(),2}, Budget: {unit.GetSalaryBudget(),8:N0}");
            }
        }
    }
}
