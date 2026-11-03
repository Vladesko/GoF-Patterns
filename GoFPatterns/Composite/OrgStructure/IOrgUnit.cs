namespace GoFPatterns.Composite.OrgStructure
{
    // Компонент - единица организации: сотрудник или целый отдел.
    public interface IOrgUnit
    {
        string Name { get; }
        decimal GetSalaryBudget();
        int GetHeadcount();
        void Display(int depth = 0);
    }
}
