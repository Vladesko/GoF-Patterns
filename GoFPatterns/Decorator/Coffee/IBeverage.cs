namespace GoFPatterns.Decorator.Coffee
{
    // Componente: interfaz común para las bebidas y para sus decoradores (complementos).
    public interface IBeverage
    {
        string GetDescription();
        decimal GetCost();
    }
}
