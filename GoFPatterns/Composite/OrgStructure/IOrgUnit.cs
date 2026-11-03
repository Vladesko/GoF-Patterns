namespace GoFPatterns.Composite.OrgStructure
{
    // Komponente – Organisationseinheit: ein Mitarbeiter oder eine ganze Abteilung.
    public interface IOrgUnit
    {
        string Name { get; }
        decimal GetSalaryBudget();
        int GetHeadcount();
        void Display(int depth = 0);
    }
}
