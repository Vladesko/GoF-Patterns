namespace GoFPatterns.Decorator.Coffee
{
    // Компонент - общий интерфейс для напитков и их декораторов (добавок).
    public interface IBeverage
    {
        string GetDescription();
        decimal GetCost();
    }
}
