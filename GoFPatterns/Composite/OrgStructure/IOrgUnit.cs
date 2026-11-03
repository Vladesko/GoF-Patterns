namespace GoFPatterns.Composite.OrgStructure
{
    // Component - unità dell'organizzazione: un dipendente o un intero reparto.
    public interface IOrgUnit
    {
        string Name { get; }
        decimal GetSalaryBudget();
        int GetHeadcount();
        void Display(int depth = 0);
    }
}
