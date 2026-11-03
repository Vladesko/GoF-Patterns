🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Visitor) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Visitor) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Visitor) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Visitor)

# Visitor (Besucher)

**Typ:** Verhaltensmuster.

**Visitor** ermöglicht es, einer Klassenhierarchie **neue Operationen hinzuzufügen**, **ohne die Klassen selbst zu ändern**. Die Operation wird in ein eigenes Besucherobjekt ausgelagert, das die Elemente „abläuft“ und an jedem die gewünschte Aktion ausführt.

---

## Welches Problem wird gelöst

Es gibt eine stabile Hierarchie von Formen: `Circle`, `Rectangle`. Auf ihnen sollen immer neue Operationen ausgeführt werden: Fläche berechnen, nach XML exportieren, später nach JSON, zeichnen, Umfang berechnen...

Fügt man jede Operation als Methode in die Formklassen ein:
- **wachsen** die Formklassen um Code, der nicht zu ihnen gehört (XML-Export ist nicht die Aufgabe eines Kreises);
- erfordert jede neue Operation eine Änderung an **allen** Klassen der Hierarchie;
- dürfen die Klassen manchmal gar nicht geändert werden (Fremdcode).

Schreibt man die Operation von außen mit Typprüfungen:

```csharp
foreach (var shape in shapes)
{
    if (shape is Circle c) ...
    else if (shape is Rectangle r) ...
}
```

…dann **vergisst** man beim Hinzufügen einer neuen Form leicht, eine dieser Ketten anzupassen – der Compiler weist nicht darauf hin.

## Lösung

1. Jede Operation wird in eine eigene Klasse ausgelagert – einen **Besucher**, der für jeden Elementtyp eine Methode hat: `VisitCircle`, `VisitRectangle`.
2. Den Elementen wird **eine** Methode `Accept(visitor)` hinzugefügt, die beim Besucher „ihre“ Methode aufruft: Der Kreis ruft `VisitCircle(this)` auf, das Rechteck `VisitRectangle(this)`.

Dieser Kniff heißt **Double Dispatch**: Der passende Code wird anhand **zweier** Typen ausgewählt – des Elementtyps (über das virtuelle `Accept`) und des Besuchertyps (über das virtuelle `Visit…`). Ohne Typprüfungen und Casts.

Eine neue Operation ist eine neue Besucherklasse. Die Formklassen ändern sich nicht mehr.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Visitor** | `IShapeVisitor` | Deklariert für jeden Elementtyp eine Besuchsmethode |
| **ConcreteVisitor** | `AreaCalculatorVisitor`, `XmlExportVisitor` | Implementiert eine Operation für alle Elementtypen. Kann ein Ergebnis ansammeln |
| **Element** | `IShape` | Deklariert die Methode `Accept(IShapeVisitor)` |
| **ConcreteElement** | `Circle`, `Rectangle` | Implementiert `Accept`, indem es die passende Methode des Besuchers aufruft |
| **Client / ObjectStructure** | `ShapeDemostrateVisitor` (Liste `List<IShape>`) | Hält die Sammlung von Elementen und „führt“ den Besucher hindurch |

## Schema

```text
 «interface» IShape                    «interface» IShapeVisitor
   Accept(IShapeVisitor v)               VisitCircle(Circle)
      ▲              ▲                   VisitRectangle(Rectangle)
      │              │                        ▲                 ▲
   Circle        Rectangle                    │                 │
   Accept(v) =>  Accept(v) =>      AreaCalculatorVisitor   XmlExportVisitor
   v.VisitCircle(this)             TotalArea               GetXml()
                 v.VisitRectangle(this)

 shape.Accept(visitor)  ──►  visitor.VisitCircle(circle)     ← Double Dispatch
```

---

## Beispiel. Formen: Fläche und XML-Export (`Shapes/`)

**Element** – `IShape`:

```csharp
public interface IShape
{
    void Accept(IShapeVisitor visitor);
}
```

**ConcreteElement** (Ordner `VisitShapes/`) – `Circle` (Radius) und `Rectangle` (Breite, Höhe) speichern nur ihre eigenen Daten (`required init`-Eigenschaften) und implementieren `Accept`:

```csharp
public class Circle : IShape
{
    public required double Radius { get; init; }

    public void Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
}
```

Beachten Sie: In den Formen gibt es **weder** eine Flächenberechnung **noch** einen Export.

**Visitor** – `IShapeVisitor` mit den Methoden `VisitCircle(Circle)` und `VisitRectangle(Rectangle)`.

**ConcreteVisitor** (Ordner `Visiotrs/`):

`AreaCalculatorVisitor` – summiert die Fläche aller besuchten Formen auf:

```csharp
public void VisitCircle(Circle circle) => TotalArea += Math.PI * circle.Radius * circle.Radius;
public void VisitRectangle(Rectangle rectangle) => TotalArea += rectangle.Width * rectangle.Height;
```

`XmlExportVisitor` – baut die XML-Darstellung in einem `StringBuilder` auf und gibt sie über `GetXml()` zurück.

Beide Besucher **halten Zustand** zwischen den Besuchen – das ist praktisch, wenn das Ergebnis über die gesamte Sammlung hinweg gesammelt wird.

**Client** – `ShapeDemostrateVisitor` erzeugt eine Liste von Formen und zwei Besucher, durchläuft dann die Formen und lässt jede jeden Besucher „annehmen“:

```csharp
foreach (var shape in shapes)
{
    shape.Accept(areaVisitor);
    shape.Accept(xmlVisitor);
}
Console.WriteLine($"Gesamtfläche: {areaVisitor.TotalArea}");
Console.WriteLine(xmlVisitor.GetXml());
```

Um zum Beispiel einen `JsonExportVisitor` oder `PerimeterVisitor` hinzuzufügen, braucht man eine einzige neue Klasse – `Circle` und `Rectangle` bleiben unverändert.

---

## Der Preis der Flexibilität

Visitor macht das Hinzufügen von **Operationen** leicht, aber das Hinzufügen von **Elementen** schwer. Kommt `Triangle` hinzu, muss `VisitTriangle` in `IShapeVisitor` ergänzt und in **allen** Besuchern implementiert werden. Dafür zeigt der Compiler selbst alle Stellen an, die angepasst werden müssen – anders als bei `if (shape is ...)`-Ketten.

Deshalb eignet sich das Muster, wenn die **Hierarchie der Elemente stabil** ist und **Operationen häufig hinzukommen**.

> In modernem C# erreicht man manchmal ein ähnliches Ergebnis mit einem `switch`-Ausdruck über Typen (`shape switch { Circle c => ..., Rectangle r => ... }`). Das ist kürzer, aber der Compiler garantiert nicht, dass alle Typen behandelt werden (sofern die Hierarchie nicht geschlossen ist). Visitor gibt diese Garantie.

## Wann einsetzen

- Auf Objekten einer komplexen Struktur (Baum, AST, Dokument) sollen viele verschiedene, voneinander unabhängige Operationen ausgeführt werden.
- Die Elementklassen sollen nicht mit fremder Logik „vermüllt“ werden.
- Die Hierarchie der Elemente ist stabil, aber Operationen kommen häufig hinzu.
- Beispiele: Compiler und Code-Analysatoren (Roslyn – `CSharpSyntaxVisitor`), `ExpressionVisitor` in `System.Linq.Expressions`, Export von Dokumenten in verschiedene Formate, Berechnungen über die Struktur einer Bestellung/eines Lagers.

## Wann nicht nötig

- Die Hierarchie der Elemente ändert sich häufig – jede neue Klasse erfordert eine Änderung an allen Besuchern.
- Es gibt wenige Operationen, und sie gehören natürlicherweise zu den Elementen selbst.

## Vor- und Nachteile

**Vorteile**
- Neue Operationen ohne Änderung der Elementklassen (Open/Closed).
- Verwandte Operationen sind in einer Klasse gebündelt (SRP).
- Ein Besucher kann beim Durchlaufen der Struktur Zustand ansammeln.
- Keine Typprüfungen und Casts.

**Nachteile**
- Neue Elementtypen lassen sich schwer hinzufügen.
- Ein Besucher kann Zugriff auf das Innere der Elemente benötigen, was dazu zwingt, deren Felder öffentlich zu machen.

## Beziehung zu anderen Mustern

- **Composite** – ein Besucher durchläuft oft den Baum eines Kompositums und führt an allen Knoten eine Operation aus.
- **Iterator** – der Iterator durchläuft die Struktur, der Besucher führt an jedem Element eine Aktion aus.
- **Interpreter** – Besucher fügen dem Syntaxbaum neue Operationen hinzu (Ausgabe, Optimierung, Prüfung), ohne die Ausdrucksklassen zu ändern.
- **Command** – ein Besucher lässt sich als erweiterter Befehl betrachten, der mit verschiedenen Objekttypen umgehen kann.
