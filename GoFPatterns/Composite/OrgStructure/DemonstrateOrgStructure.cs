using GoFPatterns.Composite.OrgStructure.Units;

namespace GoFPatterns.Composite.OrgStructure
{
    public class DemonstrateOrgStructure
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: estructura organizativa ===\n");

            var backend = new Department("Backend").Add(
                new Employee("Anna", "Jefa de equipo", 4_000),
                new Employee("Ivan", "Desarrollador", 3_000),
                new Employee("Oleg", "Desarrollador", 2_800));

            var frontend = new Department("Frontend").Add(
                new Employee("Maria", "Desarrolladora", 2_900));

            var development = new Department("Desarrollo").Add(
                new Employee("Pavel", "CTO", 6_000),
                backend,
                frontend);

            var company = new Department("Empresa").Add(
                new Employee("Elena", "CEO", 8_000),
                development,
                new Department("RR. HH.").Add(new Employee("Olga", "Reclutadora", 2_000)));

            company.Display();

            // Cualquier unidad, empleado o departamento, se puede analizar de la misma manera.
            IOrgUnit[] units = [company, development, backend, new Employee("Becario", "En prácticas", 800)];
            Console.WriteLine();
            foreach (var unit in units)
            {
                Console.WriteLine($"{unit.Name,-12} personas: {unit.GetHeadcount(),2}, presupuesto: {unit.GetSalaryBudget(),8:N0}");
            }
        }
    }
}
