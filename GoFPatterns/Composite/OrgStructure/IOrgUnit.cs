namespace GoFPatterns.Composite.OrgStructure
{
    // Componente: unidad de la organización, un empleado o un departamento completo.
    public interface IOrgUnit
    {
        string Name { get; }
        decimal GetSalaryBudget();
        int GetHeadcount();
        void Display(int depth = 0);
    }
}
