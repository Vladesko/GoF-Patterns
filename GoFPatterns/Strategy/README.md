🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Strategy) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Strategy) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Strategy) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Strategy)

# Strategy (Strategie)

**Typ:** Verhaltensmuster.

**Strategy** definiert eine **Familie austauschbarer Algorithmen**, lagert jeden in eine eigene Klasse aus und ermöglicht es, sie zur Laufzeit auszutauschen. Der Client arbeitet mit dem Algorithmus über eine gemeinsame Schnittstelle.

---

## Welches Problem wird gelöst

Ein Onlineshop berechnet die Versandkosten: per Post, per Kurier, an eine Abholstation. Steckt alles in einer Klasse:

```csharp
public decimal Calculate(Parcel parcel, string type)
{
    if (type == "post") return 3 + parcel.Weight * 1.2m;
    else if (type == "courier") return 10 + parcel.Distance * 0.05m + ...;
    else if (type == "pickup") return parcel.Value >= 100 ? 0 : 2.5m;
    ...
}
```

- bedeutet jede neue Versandart eine Änderung an dieser Klasse und das Risiko, die übrigen kaputtzumachen;
- wächst die Klasse, und die Algorithmen sind vermischt;
- ist es schwierig, einen einzelnen Algorithmus getrennt zu testen;
- tauchen dieselben `if`s in der Berechnung der Lieferzeit, in der Anzeige usw. auf.

## Lösung

Jeder Algorithmus wird in eine eigene Klasse ausgelagert – eine **Strategie** – mit gemeinsamer Schnittstelle. Die **Kontext**-Klasse hält eine Referenz auf die Strategie und delegiert die Arbeit an sie, ohne zu wissen, welche Strategie ihr übergeben wurde. Der Client wählt die passende Strategie und übergibt sie dem Kontext; sie kann jederzeit ausgetauscht werden.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Strategy** | `IDeliveryStrategy`, `ISortStrategy` | Gemeinsame Schnittstelle aller Varianten des Algorithmus |
| **ConcreteStrategy** | `PostDeliveryStrategy`, `CourierDeliveryStrategy`, `PickupPointStrategy`, `BubbleSortStrategy`, `QuickSortStrategy`, `BuiltInSortStrategy` | Konkrete Implementierungen des Algorithmus |
| **Context** | `DeliveryCalculator`, `DataSorter` | Hält die Strategie (oder erhält sie als Parameter) und ruft sie über die Schnittstelle auf |
| **Client** | Demo-Klassen | Wählt die Strategie und übergibt sie dem Kontext |

## Schema

```text
 Client ── wählt ──► PostDeliveryStrategy
   │                          │
   ▼                          ▼  implementiert
 DeliveryCalculator ◆────► «interface» IDeliveryStrategy
 (Context)                    CalculateCost(parcel)
   SetStrategy(s)             EstimateDays(parcel)
   GetQuote(parcel)                 ▲        ▲        ▲
                                    │        │        │
                                  Post   Courier   PickupPoint
```

---

## Beispiel 1. Lieferkosten (`Delivery/`)

**Strategy** – `IDeliveryStrategy` mit den Methoden `CalculateCost` und `EstimateDays`. Die Eingabedaten sind ein `record Parcel` (Gewicht, Entfernung, angegebener Warenwert).

**ConcreteStrategy** (Ordner `Strategies/`) – jede enthält nur ihre eigene Formel:
- `PostDeliveryStrategy` – günstig und langsam, der Preis hängt vom Gewicht ab;
- `CourierDeliveryStrategy` – teurer, hängt von der Entfernung ab, 1–2 Tage;
- `PickupPointStrategy` – kostenlos für Bestellungen ab 100:

```csharp
public decimal CalculateCost(Parcel parcel) =>
    parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;
```

**Context** – `DeliveryCalculator` erhält die Strategie im Konstruktor, kann sie über `SetStrategy` wechseln und erstellt ein Angebot, ohne zu wissen, welche Strategie darin steckt:

```csharp
public string GetQuote(Parcel parcel) =>
    $"{_strategy.Name} Kosten: {_strategy.CalculateCost(parcel)}, Tage: {_strategy.EstimateDays(parcel)}";
```

**Client** – die Demo schickt ein Paket durch alle Strategien und **wählt** anschließend **automatisch** die günstigste und die schnellste aus – Strategien lassen sich vergleichen, weil sie eine gemeinsame Schnittstelle haben.

Um zum Beispiel eine Drohnenlieferung hinzuzufügen, genügt eine einzige neue Klasse – weder der Kontext noch die anderen Strategien müssen geändert werden.

## Beispiel 2. Sortieralgorithmen (`Sorting/`)

**Strategy** – `ISortStrategy.Sort(int[] items)`.

**ConcreteStrategy** (Ordner `Strategies/`):
- `BubbleSortStrategy` – einfach, O(n²);
- `QuickSortStrategy` – Quicksort, im Mittel O(n log n);
- `BuiltInSortStrategy` – ein Wrapper um `Array.Sort` (das Introsort von .NET).

**Context** – `DataSorter`:
- `Sort(items, strategy)` – die Strategie wird als **Methodenparameter** übergeben, nicht in einem Feld gespeichert. Auch das ist Strategy: Wichtig ist die Möglichkeit, einen Algorithmus einzusetzen, nicht wo die Referenz gespeichert wird;
- `ChooseFor(items)` – der Kontext kann die Strategie anhand der Daten **selbst wählen** (bei einem winzigen Array Bubblesort, sonst die eingebaute).

**Client** – die Demo sortiert dieselben 5000 Zahlen mit allen Strategien und vergleicht die Zeit – so sieht man anschaulich, warum Algorithmen austauschbar sein sollten.

Am Ende wird gezeigt, dass eine einfache Strategie in C# einfach ein **Delegat** sein kann: `Array.Sort(names, byLength)`, wobei `byLength` ein `Comparison<string>` ist. Genauso funktionieren `IComparer<T>`, `IEqualityComparer<T>` und Lambdas in LINQ (`OrderBy(x => ...)`).

---

## Wann einsetzen

- Es gibt mehrere Varianten eines Algorithmus, zwischen denen umgeschaltet werden soll.
- Es gibt viele ähnliche Klassen, die sich nur im Verhalten unterscheiden.
- Eine große bedingte Anweisung, die die Variante des Algorithmus wählt, soll entfernt werden.
- Die Details eines Algorithmus sollen vom Code, der ihn nutzt, isoliert werden.
- Beispiele: Zahlungs-/Versandarten, Berechnung von Rabatten und Steuern, Komprimierung, Sortierung, Routing, Validierung.

## Wann nicht nötig

- Es gibt zwei Algorithmen, und sie ändern sich fast nie – ein `if` ist einfacher.
- Der Client muss die Varianten nicht kennen – dann ist eventuell Template Method oder einfach Polymorphie besser.

## Vor- und Nachteile

**Vorteile**
- Algorithmen lassen sich zur Laufzeit austauschen.
- Der Code der Algorithmen ist vom übrigen Code isoliert (SRP).
- Vererbung wird durch Komposition ersetzt.
- Neue Strategien kommen ohne Änderung am Kontext hinzu (Open/Closed).
- Jede Strategie lässt sich leicht einzeln testen.

**Nachteile**
- Der Client muss die Unterschiede zwischen den Strategien kennen, um die richtige zu wählen.
- Zusätzliche Klassen, wenn es nur wenige Algorithmen gibt (in C# durch Delegaten abgemildert).

## Beziehung zu anderen Mustern

- **State** – fast dieselbe Struktur, aber eine Strategie **wählt der Client**, und die Strategien sind unabhängig; Zustände **schalten sich selbst** untereinander **um**.
- **Template Method** ändert Teile eines Algorithmus über **Vererbung** (statisch, auf Klassenebene); **Strategy** ersetzt den Algorithmus vollständig über **Komposition** (dynamisch, auf Objektebene).
- **Decorator** ändert die „Hülle“ eines Objekts, **Strategy** sein „Inneres“.
- **Bridge** beruht ebenfalls auf Delegation über eine Schnittstelle, aber dort bildet auch die Abstraktion eine Hierarchie.
- **Command** verpackt ebenfalls Verhalten in ein Objekt, aber ein Befehl ist eine **Aktion** (die man aufschieben, rückgängig machen kann), eine Strategie ein **Weg**, dieselbe Aufgabe zu erledigen.
- **Abstract Factory** aus diesem Projekt (`IPaymentProviderFactory`) nutzt eine ähnliche Idee: `PaymentService` arbeitet über die Schnittstelle mit jedem beliebigen Anbieter.
