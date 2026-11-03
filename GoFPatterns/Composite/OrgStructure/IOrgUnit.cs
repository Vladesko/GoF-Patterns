namespace GoFPatterns.Composite.OrgStructure
{
    // Component - unit of organization: an employee or a whole department.
    public interface IOrgUnit
    {
        string Name { get; }
        decimal GetSalaryBudget();
        int GetHeadcount();
        void Display(int depth = 0);
    }
}
