🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Observer) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Observer) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Observer) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Observer)

# Observer (Beobachter)

**Typ:** Verhaltensmuster.

**Observer** schafft einen **Abonnement**-Mechanismus: Einige Objekte (Beobachter) abonnieren Änderungen eines anderen Objekts (des Herausgebers) und werden automatisch benachrichtigt, wenn darin etwas passiert. Eine „eins-zu-viele“-Beziehung.

---

## Welches Problem wird gelöst

Der Preis eines Produkts ändert sich. Darauf sollen verschiedene Teile des Systems reagieren:
- der **Käufer** wartet, bis der Preis weit genug fällt, um zu kaufen;
- der **Rabattdienst** meldet eine Aktion, wenn der Preis unter einer Schwelle liegt;
- der **Statistiksammler** zählt alle Preisänderungen.

Varianten ohne das Muster:
- **Polling**: Jedes interessierte Objekt fragt regelmäßig den Preis ab. Unnötige Last und verzögerte Reaktion.
- **Das Produkt ruft alle selbst auf**: `buyer.Check(price); discounts.Check(price); stats.Add(price);` – das Produkt kennt jede konkrete Klasse. Jeder neue Interessent erfordert eine Änderung am Produkt. Jemanden zur Laufzeit zu entfernen ist unmöglich.

## Lösung

Der Herausgeber (das Produkt) hält eine **Liste von Abonnenten** und bietet die Methoden `Subscribe`/`Unsubscribe` an. Alle Abonnenten implementieren eine gemeinsame Schnittstelle mit der Methode `Update`. Ändert sich der Zustand, durchläuft der Herausgeber die Liste und ruft bei jedem `Update` auf.

Der Herausgeber weiß über die Abonnenten nur, dass sie die Schnittstelle implementieren. Abonnenten können zur Laufzeit hinzugefügt und entfernt werden – ein Abonnent kann sich auch selbst abmelden.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Subject (Observable)** | `IObservable` | Schnittstelle des Herausgebers: Anmelden und Abmelden |
| **ConcreteSubject** | `Product` | Hält den Zustand (den Preis) und die Liste der Abonnenten, benachrichtigt sie über Änderungen |
| **Observer** | `IObserver` | Schnittstelle des Abonnenten mit der Methode `Update(decimal price)` |
| **ConcreteObserver** | `Buyer`, `DiscountNotifier`, `StatisticsCollector` | Reagieren jeweils auf ihre Weise auf Benachrichtigungen |
| **Client** | `ProductObserverDemonstrate` | Erzeugt Herausgeber und Abonnenten und ändert den Zustand |

## Schema

```text
 ┌───────────────────────────┐        ┌──────────────────────────┐
 │ Product (ConcreteSubject) │        │ «interface» IObserver    │
 │ - _observers: HashSet ────┼──────► │ Update(price)            │
 │ + Subscribe(o)            │  0..*  └──────────────────────────┘
 │ + Unsubscribe(o)          │            ▲         ▲          ▲
 │ + ChangePrice(p):         │            │         │          │
 │     NotifyObservers()     │         Buyer  DiscountNotifier  StatisticsCollector
 └───────────────────────────┘     (≤ 50: kaufen (≤ 100:          (zählt jede
              ▲                     & abmelden)   Rabatt)          Änderung)
              │ implementiert
   «interface» IObservable
```

---

## Beispiel. Preisänderung eines Produkts (`ProductObserver/`)

**Subject** – `IObservable` mit den Methoden `Subscribe(IObserver)` und `Unsubscribe(IObserver)`.

**Observer** – `IObserver` mit der Methode `Update(decimal price)`. Der Herausgeber übergibt den neuen Wert direkt in der Benachrichtigung (das sogenannte **Push-Modell**).

**ConcreteSubject** – `Product`:

```csharp
public class Product(decimal initialPrice) : IObservable
{
    private readonly HashSet<IObserver> _observers = [];
    private decimal _price = initialPrice;

    public void ChangePrice(decimal newPrice)
    {
        if (_price == newPrice) return;      // nicht benachrichtigen, wenn sich nichts geändert hat
        _price = newPrice;
        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers.ToArray())
            observer.Update(_price);
    }
}
```

Wichtige Implementierungsdetails:
- `HashSet` – derselbe Abonnent wird nicht doppelt angemeldet;
- `ArgumentNullException.ThrowIfNull` in `Subscribe`/`Unsubscribe` – Schutz vor `null`;
- `NotifyObservers` ist `private`: Nur das Produkt selbst darf die Abonnenten benachrichtigen;
- durchlaufen wird eine **Kopie** der Liste (`_observers.ToArray()`), weil sich ein Abonnent **mitten in der Benachrichtigung abmelden** kann (so macht es `Buyer`). Ein `HashSet` während der Iteration über dasselbe `HashSet` zu ändern ist nicht erlaubt – das würde eine `InvalidOperationException` auslösen.

**ConcreteObserver** (Ordner `Observers/`) – jeder meldet sich im Konstruktor an (`_observable.Subscribe(this)`) und reagiert auf seine Weise auf den Preis:
- `StatisticsCollector` – zählt jede Preisänderung und gibt den Zähler aus;
- `DiscountNotifier` – meldet bei einem Preis ≤ 100 einen Rabatt;
- `Buyer` – kauft bei einem Preis ≤ 50 das Produkt und **meldet sich selbst ab**, da er kein Interesse mehr hat:

```csharp
public void Update(decimal price)
{
    if (price > 50) return;
    Console.WriteLine("Produkt wird gekauft!");
    _observable.Unsubscribe(this);
}
```

**Client** – `ProductObserverDemonstrate` erzeugt ein Produkt mit dem Preis 150 und drei Abonnenten und ändert dann den Preis: 120 → 90 → 40. Bei jeder Änderung reagiert die Statistik; bei 90 erscheint der Rabatt; bei 40 Rabatt und Kauf.

---

## Push und Pull

- **Push** (wie hier) – der Herausgeber übergibt die Daten in `Update(price)`. Einfach, aber die Schnittstelle ist an konkrete Daten gebunden.
- **Pull** – der Herausgeber übergibt sich selbst (`Update(IObservable sender)`), und der Abonnent liest die benötigten Eigenschaften selbst aus. Flexibler, aber der Abonnent hängt vom Herausgeber ab.

## Observer in .NET

- **C#-Events** (`event EventHandler<T>`) – eine eingebaute Umsetzung des Musters: `+=` meldet an, `-=` meldet ab, das Auslösen des Events ist die Benachrichtigung.
- Die Schnittstellen `IObservable<T>` / `IObserver<T>` in der BCL und die Bibliothek **Rx.NET** – reaktive Streams.
- `INotifyPropertyChanged` in WPF/MAUI – Bindung der UI an das Modell.

## Wann einsetzen

- Die Änderung eines Objekts erfordert eine Reaktion anderer, und es ist vorab nicht bekannt, welcher und wie vieler.
- Abonnenten sollen zur Laufzeit hinzugefügt und entfernt werden.
- Die Kopplung zwischen der Ereignisquelle und den Reaktionen darauf soll gelockert werden.

## Wann nicht nötig

- Es gibt genau einen Abonnenten, und er ist vorab bekannt – ein direkter Aufruf ist einfacher.
- Eine strikte Reihenfolge und Transaktionalität der Reaktionen ist wichtig – bei Beobachtern ist die Reihenfolge der Benachrichtigungen meist nicht garantiert.

## Vor- und Nachteile

**Vorteile**
- Der Herausgeber hängt nicht von den konkreten Klassen der Abonnenten ab (Open/Closed).
- An- und Abmelden zur Laufzeit.
- Ein Ereignis – beliebig viele Reaktionen.

**Nachteile**
- Die Reihenfolge der Benachrichtigungen ist nicht garantiert.
- **Speicherlecks**: Vergisst ein Abonnent, sich abzumelden, hält der Herausgeber eine Referenz auf ihn, und der Garbage Collector entfernt ihn nicht (ein klassisches Problem mit Events in .NET).
- Kaskaden von Benachrichtigungen sind manchmal schwer zu debuggen.

## Beziehung zu anderen Mustern

- **Mediator** – wird oft über Observer umgesetzt: Komponenten veröffentlichen Ereignisse, der Vermittler hat sie abonniert. Die Ziele unterscheiden sich: Observer ist dynamisches Verteilen, Mediator zentralisierte Interaktionslogik.
- **Chain of Responsibility**, **Command**, **Mediator**, **Observer** – verschiedene Wege, Absender und Empfänger von Anfragen zu verbinden.
- **Singleton** – manchmal wird ein zentraler Ereignis-Herausgeber als Singleton umgesetzt (Event Bus).
