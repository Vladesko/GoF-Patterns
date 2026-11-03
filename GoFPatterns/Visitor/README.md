🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Visitor) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Visitor) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Visitor) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Visitor) | **Español**

# Visitor (Visitante)

**Tipo:** patrón de comportamiento.

**Visitor** permite **añadir operaciones nuevas** a una jerarquía de clases **sin modificar las propias clases**. La operación se traslada a un objeto visitante aparte, que «recorre» los elementos y ejecuta sobre cada uno la acción necesaria.

---

## Qué problema resuelve

Hay una jerarquía estable de figuras: `Circle`, `Rectangle`. Sobre ellas hay que ejecutar operaciones cada vez más variadas: calcular el área, exportar a XML, luego a JSON, dibujar, calcular el perímetro...

Si cada operación se añade como método en las clases de las figuras:
- las clases de las figuras **se llenan** de código que no les corresponde (exportar a XML no es responsabilidad de un círculo);
- cada operación nueva exige modificar **todas** las clases de la jerarquía;
- a veces las clases ni siquiera se pueden modificar (código de terceros).

Si la operación se escribe fuera comprobando tipos:

```csharp
foreach (var shape in shapes)
{
    if (shape is Circle c) ...
    else if (shape is Rectangle r) ...
}
```

…al añadir una figura nueva es fácil **olvidarse** de actualizar alguna de esas cadenas, y el compilador no avisa.

## Solución

1. Sacar cada operación a una clase aparte, el **visitante**, que tiene un método por cada tipo de elemento: `VisitCircle`, `VisitRectangle`.
2. Añadir a los elementos **un único** método `Accept(visitor)`, que llama a «su» método del visitante: el círculo llama a `VisitCircle(this)` y el rectángulo, a `VisitRectangle(this)`.

Esta técnica se llama **doble despacho** (double dispatch): el código adecuado se elige según **dos** tipos, el del elemento (mediante el `Accept` virtual) y el del visitante (mediante el `Visit…` virtual). Sin comprobaciones de tipo ni conversiones.

Una operación nueva es una nueva clase visitante. Las clases de las figuras ya no cambian.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Visitor** | `IShapeVisitor` | Declara un método de visita para cada tipo de elemento |
| **ConcreteVisitor** | `AreaCalculatorVisitor`, `XmlExportVisitor` | Implementa una operación para todos los tipos de elemento. Puede acumular el resultado |
| **Element** | `IShape` | Declara el método `Accept(IShapeVisitor)` |
| **ConcreteElement** | `Circle`, `Rectangle` | Implementa `Accept` llamando al método correspondiente del visitante |
| **Client / ObjectStructure** | `ShapeDemostrateVisitor` (lista `List<IShape>`) | Guarda la colección de elementos y «pasea» al visitante por ella |

## Esquema

```text
 «interface» IShape                    «interface» IShapeVisitor
   Accept(IShapeVisitor v)               VisitCircle(Circle)
      ▲              ▲                   VisitRectangle(Rectangle)
      │              │                        ▲                 ▲
   Circle        Rectangle                    │                 │
   Accept(v) =>  Accept(v) =>      AreaCalculatorVisitor   XmlExportVisitor
   v.VisitCircle(this)             TotalArea               GetXml()
                 v.VisitRectangle(this)

 shape.Accept(visitor)  ──►  visitor.VisitCircle(circle)     ← doble despacho
```

---

## Ejemplo. Figuras: área y exportación a XML (`Shapes/`)

**Element**: `IShape`:

```csharp
public interface IShape
{
    void Accept(IShapeVisitor visitor);
}
```

**ConcreteElement** (carpeta `VisitShapes/`): `Circle` (radio) y `Rectangle` (ancho, alto) guardan solo sus propios datos (propiedades `required init`) e implementan `Accept`:

```csharp
public class Circle : IShape
{
    public required double Radius { get; init; }

    public void Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
}
```

Fíjese en que en las figuras **no hay** ni cálculo del área ni exportación.

**Visitor**: `IShapeVisitor` con los métodos `VisitCircle(Circle)` y `VisitRectangle(Rectangle)`.

**ConcreteVisitor** (carpeta `Visiotrs/`):

`AreaCalculatorVisitor`: acumula el área total de todas las figuras visitadas:

```csharp
public void VisitCircle(Circle circle) => TotalArea += Math.PI * circle.Radius * circle.Radius;
public void VisitRectangle(Rectangle rectangle) => TotalArea += rectangle.Width * rectangle.Height;
```

`XmlExportVisitor`: construye la representación XML en un `StringBuilder` y la entrega mediante `GetXml()`.

Ambos visitantes **guardan estado** entre visitas, lo cual es cómodo cuando el resultado se acumula a lo largo de toda la colección.

**Client**: `ShapeDemostrateVisitor` crea una lista de figuras y dos visitantes, y después recorre las figuras haciendo que cada una «acepte» a cada visitante:

```csharp
foreach (var shape in shapes)
{
    shape.Accept(areaVisitor);
    shape.Accept(xmlVisitor);
}
Console.WriteLine($"Área total: {areaVisitor.TotalArea}");
Console.WriteLine(xmlVisitor.GetXml());
```

Para añadir, por ejemplo, `JsonExportVisitor` o `PerimeterVisitor`, hace falta una sola clase nueva: `Circle` y `Rectangle` no cambian.

---

## El precio de la flexibilidad

Visitor facilita añadir **operaciones**, pero dificulta añadir **elementos**. Si se añade `Triangle`, hay que añadir `VisitTriangle` a `IShapeVisitor` e implementarlo en **todos** los visitantes. A cambio, el propio compilador señala todos los lugares que hay que actualizar, a diferencia de las cadenas `if (shape is ...)`.

Por eso el patrón es adecuado cuando la **jerarquía de elementos es estable** y las **operaciones se añaden a menudo**.

> En el C# moderno, a veces se consigue un resultado parecido con una expresión `switch` sobre tipos (`shape switch { Circle c => ..., Rectangle r => ... }`). Es más breve, pero el compilador no garantiza que se traten todos los tipos (si la jerarquía no está cerrada). Visitor sí da esa garantía.

## Cuándo usarlo

- Sobre objetos de estructura compleja (árbol, AST, documento) hay que ejecutar muchas operaciones distintas y no relacionadas.
- No se quiere «ensuciar» las clases de los elementos con lógica ajena.
- La jerarquía de elementos es estable y las operaciones se añaden a menudo.
- Ejemplos: compiladores y analizadores de código (Roslyn: `CSharpSyntaxVisitor`), `ExpressionVisitor` en `System.Linq.Expressions`, exportación de documentos a distintos formatos, cálculos sobre la estructura de un pedido/almacén.

## Cuándo no hace falta

- La jerarquía de elementos cambia a menudo: cada clase nueva exigirá modificar todos los visitantes.
- Hay pocas operaciones y pertenecen de forma natural a los propios elementos.

## Ventajas y desventajas

**Ventajas**
- Operaciones nuevas sin modificar las clases de los elementos (Open/Closed).
- Las operaciones relacionadas están reunidas en una sola clase (SRP).
- El visitante puede acumular estado mientras recorre la estructura.
- Sin comprobaciones de tipo ni conversiones.

**Desventajas**
- Es difícil añadir tipos de elemento nuevos.
- El visitante puede necesitar acceder a las interioridades de los elementos, lo que obliga a hacer públicos sus campos.

## Relación con otros patrones

- **Composite**: el visitante suele recorrer el árbol de un compuesto, ejecutando la operación sobre todos los nodos.
- **Iterator**: el iterador recorre la estructura y el visitante ejecuta una acción sobre cada elemento.
- **Interpreter**: los visitantes añaden operaciones nuevas sobre el árbol sintáctico (impresión, optimización, comprobación) sin modificar las clases de las expresiones.
- **Command**: el visitante puede considerarse un comando ampliado que sabe trabajar con distintos tipos de objetos.
