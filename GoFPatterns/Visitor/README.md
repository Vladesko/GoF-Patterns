🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Visitor) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Visitor) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Visitor) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Visitor)

# Visitor

**Type:** behavioral pattern.

**Visitor** lets you **add new operations** to a class hierarchy **without changing the classes themselves**. An operation is moved into a separate visitor object that "walks" over the elements and performs the required action on each one.

---

## The problem

There is a stable hierarchy of shapes: `Circle`, `Rectangle`. More and more operations need to be performed on them: calculate the area, export to XML, then to JSON, draw, calculate the perimeter...

If each operation is added as a method to the shape classes:
- the shape classes **grow** with code that does not belong to them (exporting to XML is not a circle's responsibility);
- every new operation requires editing **all** classes of the hierarchy;
- sometimes the classes cannot be changed at all (third-party code).

If the operation is written outside using type checks:

```csharp
foreach (var shape in shapes)
{
    if (shape is Circle c) ...
    else if (shape is Rectangle r) ...
}
```

…then when a new shape is added, it is easy to **forget** to update one of these chains — the compiler will not warn you.

## Solution

1. Move each operation into a separate class — a **visitor** — that has one method per element type: `VisitCircle`, `VisitRectangle`.
2. Add **one** method `Accept(visitor)` to the elements, which calls "its own" method on the visitor: a circle calls `VisitCircle(this)`, a rectangle calls `VisitRectangle(this)`.

This technique is called **double dispatch**: the code to run is selected by **two** types — the element type (via the virtual `Accept`) and the visitor type (via the virtual `Visit…`). No type checks and no casts.

A new operation is a new visitor class. The shape classes no longer change.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Visitor** | `IShapeVisitor` | Declares a visit method for each element type |
| **ConcreteVisitor** | `AreaCalculatorVisitor`, `XmlExportVisitor` | Implements one operation for all element types. Can accumulate a result |
| **Element** | `IShape` | Declares the `Accept(IShapeVisitor)` method |
| **ConcreteElement** | `Circle`, `Rectangle` | Implements `Accept` by calling the corresponding visitor method |
| **Client / ObjectStructure** | `ShapeDemostrateVisitor` (a `List<IShape>`) | Stores the collection of elements and "walks" the visitor through it |

## Diagram

```text
 «interface» IShape                    «interface» IShapeVisitor
   Accept(IShapeVisitor v)               VisitCircle(Circle)
      ▲              ▲                   VisitRectangle(Rectangle)
      │              │                        ▲                 ▲
   Circle        Rectangle                    │                 │
   Accept(v) =>  Accept(v) =>      AreaCalculatorVisitor   XmlExportVisitor
   v.VisitCircle(this)             TotalArea               GetXml()
                 v.VisitRectangle(this)

 shape.Accept(visitor)  ──►  visitor.VisitCircle(circle)     ← double dispatch
```

---

## Example. Shapes: area and XML export (`Shapes/`)

**Element** — `IShape`:

```csharp
public interface IShape
{
    void Accept(IShapeVisitor visitor);
}
```

**ConcreteElement** (folder `VisitShapes/`) — `Circle` (radius) and `Rectangle` (width, height) store only their own data (`required init` properties) and implement `Accept`:

```csharp
public class Circle : IShape
{
    public required double Radius { get; init; }

    public void Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
}
```

Note that the shapes contain **neither** area calculation **nor** export.

**Visitor** — `IShapeVisitor` with the methods `VisitCircle(Circle)` and `VisitRectangle(Rectangle)`.

**ConcreteVisitor** (folder `Visiotrs/`):

`AreaCalculatorVisitor` — accumulates the total area of all visited shapes:

```csharp
public void VisitCircle(Circle circle) => TotalArea += Math.PI * circle.Radius * circle.Radius;
public void VisitRectangle(Rectangle rectangle) => TotalArea += rectangle.Width * rectangle.Height;
```

`XmlExportVisitor` — builds an XML representation in a `StringBuilder` and returns it via `GetXml()`.

Both visitors **keep state** between visits — this is convenient when the result is collected over the whole collection.

**Client** — `ShapeDemostrateVisitor` creates a list of shapes and two visitors, then goes through the shapes and has each one "accept" both visitors:

```csharp
foreach (var shape in shapes)
{
    shape.Accept(areaVisitor);
    shape.Accept(xmlVisitor);
}
Console.WriteLine($"Total Area: {areaVisitor.TotalArea}");
Console.WriteLine(xmlVisitor.GetXml());
```

To add, for example, a `JsonExportVisitor` or a `PerimeterVisitor`, one new class is needed — `Circle` and `Rectangle` do not change.

---

## The price of flexibility

Visitor makes adding **operations** easy but adding **elements** hard. Add a `Triangle` — and you need to add `VisitTriangle` to `IShapeVisitor` and implement it in **all** visitors. On the other hand, the compiler itself will point out every place that needs updating — unlike chains of `if (shape is ...)`.

That is why the pattern fits when the **element hierarchy is stable** and **operations are added often**.

> In modern C#, a similar result is sometimes achieved with a `switch` expression on types (`shape switch { Circle c => ..., Rectangle r => ... }`). It is shorter, but the compiler does not guarantee that all types are handled (unless the hierarchy is closed). Visitor provides this guarantee.

## When to use

- Many different, unrelated operations need to be performed on objects of a complex structure (a tree, an AST, a document).
- You do not want to "pollute" the element classes with unrelated logic.
- The element hierarchy is stable, while operations are added often.
- Examples: compilers and code analyzers (Roslyn — `CSharpSyntaxVisitor`), `ExpressionVisitor` in `System.Linq.Expressions`, exporting documents to different formats, calculations over an order/warehouse structure.

## When not to use

- The element hierarchy changes often — every new class will require editing all visitors.
- There are few operations, and they naturally belong to the elements themselves.

## Pros and cons

**Pros**
- New operations without changing the element classes (Open/Closed).
- Related operations are gathered in one class (SRP).
- A visitor can accumulate state while traversing the structure.
- No type checks or casts.

**Cons**
- It is hard to add new element types.
- A visitor may need access to the elements' internals, which forces you to make their fields public.

## Relations with other patterns

- **Composite** — a visitor often traverses a composite tree, performing an operation on all nodes.
- **Iterator** — the iterator traverses the structure, the visitor performs an action on each element.
- **Interpreter** — visitors add new operations on the syntax tree (printing, optimization, checking) without changing the expression classes.
- **Command** — a visitor can be seen as an extended command that can work with different types of objects.
