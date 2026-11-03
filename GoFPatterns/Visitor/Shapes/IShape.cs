namespace GoFPatterns.Visitor.Shapes
{
    // La interfaz IShape define el método Accept, que recibe un visitante como argumento.
    // Implementar esta interfaz permite que distintas figuras acepten visitantes que realizan operaciones
    // sobre ellas sin modificar las propias clases de las figuras.
    public interface IShape
    {
        void Accept(IShapeVisitor visitor);
    }
}
