namespace GoFPatterns.Visitor.Shapes
{
    // Интерфейс IShape определяет метод Accept, который принимает посетителя в качестве аргумента.
    // Реализация этого интерфейса позволяет разным фигурам принимать посетителей, которые могут выполнять над ними операции
    // без изменения самих классов фигур.
    public interface IShape
    {
        void Accept(IShapeVisitor visitor);
    }
}
