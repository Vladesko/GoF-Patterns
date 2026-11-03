using GoFPatterns.Composite.OrgStructure.Units;

namespace GoFPatterns.Composite.OrgStructure
{
    public class DemonstrateOrgStructure
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Компоновщик: структура организации ===\n");

            var backend = new Department("Бэкенд").Add(
                new Employee("Анна", "Тимлид", 4_000),
                new Employee("Иван", "Разработчик", 3_000),
                new Employee("Олег", "Разработчик", 2_800));

            var frontend = new Department("Фронтенд").Add(
                new Employee("Мария", "Разработчик", 2_900));

            var development = new Department("Разработка").Add(
                new Employee("Павел", "CTO", 6_000),
                backend,
                frontend);

            var company = new Department("Компания").Add(
                new Employee("Елена", "CEO", 8_000),
                development,
                new Department("HR").Add(new Employee("Ольга", "Рекрутер", 2_000)));

            company.Display();

            // Любую единицу - сотрудника или отдел - можно анализировать одинаково.
            IOrgUnit[] units = [company, development, backend, new Employee("Стажёр", "Стажёр", 800)];
            Console.WriteLine();
            foreach (var unit in units)
            {
                Console.WriteLine($"{unit.Name,-12} человек: {unit.GetHeadcount(),2}, бюджет: {unit.GetSalaryBudget(),8:N0}");
            }
        }
    }
}
