using GoFPatterns.Composite.OrgStructure.Units;

namespace GoFPatterns.Composite.OrgStructure
{
    public class DemonstrateOrgStructure
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Composite: Struttura organizzativa ===\n");

            var backend = new Department("Backend").Add(
                new Employee("Anna", "Team leader", 4_000),
                new Employee("Ivan", "Sviluppatore", 3_000),
                new Employee("Oleg", "Sviluppatore", 2_800));

            var frontend = new Department("Frontend").Add(
                new Employee("Maria", "Sviluppatrice", 2_900));

            var development = new Department("Sviluppo").Add(
                new Employee("Pavel", "CTO", 6_000),
                backend,
                frontend);

            var company = new Department("Azienda").Add(
                new Employee("Elena", "CEO", 8_000),
                development,
                new Department("HR").Add(new Employee("Olga", "Selezionatrice", 2_000)));

            company.Display();

            // Qualsiasi unità - dipendente o reparto - può essere analizzata allo stesso modo.
            IOrgUnit[] units = [company, development, backend, new Employee("Stagista", "Tirocinante", 800)];
            Console.WriteLine();
            foreach (var unit in units)
            {
                Console.WriteLine($"{unit.Name,-12} persone: {unit.GetHeadcount(),2}, budget: {unit.GetSalaryBudget(),8:N0}");
            }
        }
    }
}
