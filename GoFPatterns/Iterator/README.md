🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Iterator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Iterator) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Iterator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Iterator)

# Iterator (Iterator)

**Typ:** Verhaltensmuster.

**Iterator** bietet eine Möglichkeit, die **Elemente** eines zusammengesetzten Objekts (einer Sammlung) **nacheinander zu durchlaufen**, **ohne seinen inneren Aufbau offenzulegen** – ob Array, Liste, Baum oder etwas anderes.

---

## Welches Problem wird gelöst

Sammlungen sind sehr unterschiedlich: Array, verkettete Liste, Baum, Graph, Ergebnis einer Datenbankabfrage. Der Client will einfach „alle Elemente durchgehen“, aber:

- ist der Durchlauf im Client geschrieben, ist der Client **an die Struktur** der Sammlung **gebunden** (Indizes beim Array, Rekursion beim Baum). Wird das Array durch einen Baum ersetzt, müssen alle Clients umgeschrieben werden;
- es kann **mehrere** Durchläufe geben (vorwärts, rückwärts, mit Filter, Tiefensuche, Breitensuche). Fügt man sie der Sammlung selbst hinzu, lädt sie fremde Zuständigkeiten auf sich;
- manchmal braucht man **mehrere unabhängige Durchläufe gleichzeitig** – die Position des Durchlaufs kann also nicht in der Sammlung selbst gespeichert werden.

## Lösung

Die Durchlauflogik wird in ein eigenes Objekt ausgelagert – den **Iterator**. Der Iterator:
- speichert die **aktuelle Position**;
- bietet eine einfache Schnittstelle: „Gibt es ein nächstes?“ und „Gib mir das nächste“;
- kennt den inneren Aufbau der Sammlung, der Client dagegen nicht.

Die Sammlung (das **Aggregat**) kann Iteratoren für sich selbst erzeugen. Jeder Aufruf erzeugt einen neuen Iterator mit eigener Position.

### Iterator in .NET

In C# ist das Muster in die Sprache eingebaut:

| GoF | .NET |
|-----|------|
| Aggregate | `IEnumerable<T>` (`GetEnumerator()`) |
| Iterator | `IEnumerator<T>` (`MoveNext()`, `Current`, `Reset()`) |
| Client-Schleife | `foreach` |
| Schreiben eines Iterators | `yield return` – der Compiler erzeugt die Iterator-Klasse selbst |

Deshalb schreibt man in echtem Code meist keinen eigenen `IBookIterator`, sondern implementiert `IEnumerable<T>`. Beide Wege werden unten gezeigt.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Iterator** | `IBookIterator`, `IEnumerator<T>` | Durchlaufschnittstelle: `HasNext()`, `Next()`, `Reset()` |
| **ConcreteIterator** | `ForwardBookIterator`, `ReverseBookIterator`, `AuthorFilterIterator`, aus `yield return` generierte Iteratoren | Implementiert einen konkreten Durchlaufalgorithmus und speichert die aktuelle Position |
| **Aggregate** | `IBookAggregate`, `IEnumerable<T>` | Schnittstelle einer Sammlung, die Iteratoren erzeugt |
| **ConcreteAggregate** | `Library`, `CategoryNode` | Sammlung, die die passenden Iteratoren liefert |
| **Client** | Demo-Klassen | Durchläuft die Sammlung ausschließlich über den Iterator |

## Schema

```text
 Client ─────► «interface» IBookAggregate        «interface» IBookIterator
                 CreateIterator()        ─────►    HasNext(), Next(), Reset()
                        ▲                                   ▲
                        │                ┌──────────────────┼──────────────────┐
                     Library             │                  │                  │
                  (intern Book[]) ◄── ForwardBookIterator  ReverseBookIterator  AuthorFilterIterator
                                         _position          _position           _position, author
```

---

## Beispiel 1. Büchersammlung (`BookCollection/`) – klassischer Iterator und `IEnumerable<T>`

**ConcreteAggregate** – `Library` speichert die Bücher in einem **Array**, das von Hand vergrößert wird (`Array.Resize`). Die Clients wissen das nicht – das Array kann durch etwas beliebig anderes ersetzt werden, ohne sie anzufassen.

**Iterator** – `IBookIterator` mit den Methoden `HasNext()`, `Next()`, `Reset()`.

**ConcreteIterator** (Ordner `Iterators/`):
- `ForwardBookIterator` – vom ersten bis zum letzten Buch;
- `ReverseBookIterator` – vom letzten bis zum ersten;
- `AuthorFilterIterator` – überspringt Bücher anderer Autoren:

```csharp
public bool HasNext()
{
    while (_position < library.Count && library.GetAt(_position).Author != author)
        _position++;
    return _position < library.Count;
}
```

Die Iteratoren greifen über die `internal`-Methode `GetAt` auf die Sammlung zu – für Clients ist sie nicht sichtbar.

**Aggregate** – `IBookAggregate` deklariert die Fabrikmethoden für Iteratoren; `Library` implementiert sie.

**Client** – die Methode `Print` in der Demo nimmt **jeden beliebigen** `IBookIterator` entgegen und durchläuft ihn mit ein und derselben Schleife `while (iterator.HasNext())`.

Außerdem implementiert `Library` `IEnumerable<Book>` über `yield return`:

```csharp
public IEnumerator<Book> GetEnumerator()
{
    for (var i = 0; i < _count; i++)
        yield return _books[i];
}
```

Das liefert sofort `foreach` und das gesamte LINQ (`library.Where(...).OrderBy(...)`).

## Beispiel 2. Durchlauf eines Kategoriebaums (`Tree/`)

**ConcreteAggregate** – `CategoryNode`: ein Knoten des Katalogbaums. Die Liste der Kinder ist privat.

**ConcreteIterator** – zwei Durchlaufalgorithmen, geschrieben mit `yield return`:
- `DepthFirst()` – **Tiefensuche** (wie ein eingerücktes Menü). Es wird ein expliziter `Stack` statt Rekursion verwendet, damit ein tiefer Baum keine `StackOverflowException` auslöst;
- `BreadthFirst()` – **Breitensuche**, Ebene für Ebene, mit einer `Queue`.

```csharp
public IEnumerable<(CategoryNode Node, int Depth)> BreadthFirst()
{
    var queue = new Queue<(CategoryNode, int)>();
    queue.Enqueue((this, 0));
    while (queue.Count > 0)
    {
        var (node, depth) = queue.Dequeue();
        yield return (node, depth);
        foreach (var child in node._children)
            queue.Enqueue((child, depth + 1));
    }
}
```

**Client** – durchläuft denselben Baum auf zwei Arten mit einem gewöhnlichen `foreach`.

Eine wichtige Eigenschaft von `yield`-Iteratoren ist ihre **Lazy-Auswertung**: Die Elemente werden einzeln und bei Bedarf berechnet. In der Demo stoppt die Suche `First(n => n.Name == "Laptops")` den Durchlauf, sobald der Knoten gefunden ist, und besucht nur einen Teil des Baums.

---

## Wann einsetzen

- Der innere Aufbau einer komplexen Sammlung soll vor dem Client verborgen werden.
- Für eine Sammlung werden mehrere Durchlaufvarianten benötigt.
- Man braucht eine einheitliche Durchlaufschnittstelle für verschiedene Sammlungen.
- Ein Lazy-Durchlauf ist nötig (große oder unendliche Folgen, seitenweises Laden aus der Datenbank).

## Wann nicht nötig

- Die Sammlung ist einfach (`List<T>`, Array) – verwenden Sie das eingebaute `foreach`.

## Vor- und Nachteile

**Vorteile**
- Vereinfacht Sammlungen: Die Durchlauflogik ist ausgelagert (SRP).
- Verschiedene Durchlaufalgorithmen ohne Änderung der Sammlung (Open/Closed).
- Mehrere unabhängige Durchläufe gleichzeitig.
- Lazy-Auswertung.

**Nachteile**
- Bei einfachen Sammlungen eine unnötige Verkomplizierung.
- Änderungen an der Sammlung während des Durchlaufs sind ein eigenes Problem (die Standard-Collections von .NET werfen eine `InvalidOperationException`).

## Beziehung zu anderen Mustern

- **Composite** – Iteratoren werden oft zum Durchlaufen von Composite-Bäumen verwendet.
- **Factory Method** – `CreateIterator()` in der Sammlung ist eine Fabrikmethode, die die passende Iterator-Unterklasse liefert.
- **Memento** – der Zustand einer Iteration kann gespeichert und später wiederhergestellt werden.
- **Visitor** + Iterator: Der Iterator durchläuft die Struktur, der Besucher führt an jedem Element eine Operation aus.
