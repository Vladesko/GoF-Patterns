🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/State) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/State) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/State) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/State)

# State (Zustand)

**Typ:** Verhaltensmuster.

**State** ermöglicht es einem Objekt, **sein Verhalten zu ändern, wenn sich sein innerer Zustand ändert**. Von außen sieht es so aus, als hätte das Objekt seine Klasse gewechselt.

---

## Welches Problem wird gelöst

Eine Bestellung in einem Onlineshop durchläuft Phasen: *Neu → Bezahlt → Versandt → Zugestellt* (oder *Storniert*). Dieselbe Aktion verhält sich je nach Phase unterschiedlich: Eine neue Bestellung zu stornieren heißt einfach stornieren; eine bezahlte – Geld erstatten; eine versandte – nicht möglich.

Die typische Umsetzung ist ein Feld `Status` und ein `switch` in jeder Methode:

```csharp
public void Cancel()
{
    switch (Status)
    {
        case Status.New: Status = Status.Cancelled; break;
        case Status.Paid: Refund(); Status = Status.Cancelled; break;
        case Status.Shipped: throw ...;
        case Status.Delivered: throw ...;
        ...
    }
}
```

Solche `switch`-Anweisungen gibt es in `Pay`, `Ship`, `Deliver`, `Cancel`... Kommt ein neuer Status hinzu („Rücksendung“), muss man alle Methoden durchgehen. Die Übergangsregeln sind über die ganze Klasse verteilt, und Fehler passieren leicht.

## Lösung

Jeder Zustand wird in eine **eigene Klasse** mit gemeinsamer Schnittstelle ausgelagert, die alle Aktionen enthält. Das ursprüngliche Objekt (der **Kontext**) hält eine Referenz auf das aktuelle Zustandsobjekt und **delegiert** alle Aktionen an es. Der Übergang in einen anderen Zustand ist einfach der Austausch des Zustandsobjekts.

Statt eines großen `switch` über alle Zustände erhält man mehrere kleine Klassen, von denen jede **das Verhalten in einem Zustand** beschreibt und **wohin von dort gewechselt werden kann**.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Context** | `Order`, `VendingMachine` | Hält eine Referenz auf den aktuellen Zustand und delegiert die Aktionen an ihn. Bietet eine Methode zum Zustandswechsel |
| **State** | `IOrderState`, `IVendingState` | Gemeinsame Schnittstelle aller Zustände – alle Aktionen, die dem Kontext zur Verfügung stehen |
| **ConcreteState** | `NewState`, `PaidState`, `ShippedState`, `DeliveredState`, `CancelledState`, `IdleState`, `HasMoneyState`, `SoldOutState` | Implementiert das Verhalten in einem konkreten Zustand und entscheidet, wann der Kontext umgeschaltet wird |

## Schema

```text
 ┌───────────────────────┐        ┌────────────────────────┐
 │ Order (Context)       │◆──────►│ «interface» IOrderState│
 │ - State               │        │ Pay(), Ship(),         │
 │ + Pay() => State.Pay()│        │ Deliver(), Cancel()    │
 │ + TransitionTo(state) │        └────────────────────────┘
 └───────────────────────┘                    ▲
                                ┌─────────────┼──────────────┬────────────┐
                             NewState     PaidState     ShippedState   ...

 Übergänge:  New ──Pay──► Paid ──Ship──► Shipped ──Deliver──► Delivered
              │            │
              └──Cancel──► Cancelled ◄──Cancel (Erstattung)
```

---

## Beispiel 1. Lebenszyklus einer Bestellung (`OrderLifecycle/`)

**Context** – `Order` enthält kein einziges `switch`. Alle öffentlichen Aktionen werden einfach an den aktuellen Zustand delegiert:

```csharp
public void Pay() => State.Pay(this);
public void Cancel() => State.Cancel(this);

internal void TransitionTo(IOrderState state) => State = state;
```

`TransitionTo` ist als `internal` markiert – den Zustand dürfen nur die Zustandsklassen ändern, nicht externer Code.

**State** – `IOrderState` listet alle Aktionen mit einer Bestellung auf.

**Basiszustand** – `OrderStateBase` (Ordner `States/`) **verbietet** standardmäßig alle Aktionen. Deshalb überschreiben die konkreten Zustände nur das, was in ihnen erlaubt ist – man muss nicht in jeder Klasse für die übrigen Aktionen „nicht möglich“ schreiben.

**ConcreteState**:
- `NewState` – erlaubt sind Bezahlen (→ `Paid`) und Stornieren (→ `Cancelled`);
- `PaidState` – Versand (→ `Shipped`) und Stornierung **mit Erstattung**:

```csharp
public override void Cancel(Order order)
{
    Console.WriteLine($"Erstattung von {order.Amount:F2} an den Kunden");
    order.TransitionTo(new CancelledState());
}
```

- `ShippedState` – nur Zustellung (→ `Delivered`);
- `DeliveredState`, `CancelledState` – Endzustände, überschreiben nichts.

Man sieht, dass sich die Aktion „Stornieren“ in zwei Zuständen unterschiedlich verhält und in den übrigen verboten ist – und dafür gibt es kein einziges `if` auf den Status.

**Client** – die Demo versucht, Aktionen in falscher Reihenfolge auszuführen (eine unbezahlte Bestellung versenden, eine versandte stornieren) – die Zustände lehnen das korrekt ab.

## Beispiel 2. Verkaufsautomat (`Vending/`)

**Context** – `VendingMachine` hält die **Daten** (Warenbestand, Preis, Guthaben) und den aktuellen Zustand.

**ConcreteState** (Ordner `States/`):
- `IdleState` – wartet auf Geld; Münze → `HasMoney`;
- `HasMoneyState` – sammelt Geld, gibt Ware und Wechselgeld aus; ist die Ware ausverkauft → `SoldOut`, sonst → `Idle`;
- `SoldOutState` – gibt jede Münze sofort zurück.

Hier wird ein Kniff aus dem GoF-Buch gezeigt: Die Zustände **haben keine eigenen Felder** – alle Daten liegen im Kontext und werden als Parameter übergeben. Deshalb kann jeder Zustand einmal erzeugt und **geteilt** werden (das sind Flyweights, siehe Flyweight):

```csharp
internal static readonly IVendingState Idle = new IdleState();
internal static readonly IVendingState HasMoney = new HasMoneyState();
internal static readonly IVendingState SoldOut = new SoldOutState();
```

**Client** – die Demo simuliert Käufer: ein Kaufversuch ohne Geld, zu wenig Geld, ein Kauf mit Wechselgeld, eine Rückgabe, der Kauf des letzten Artikels und der Versuch, eine Münze in einen leeren Automaten zu werfen. Nach jeder Aktion wird der aktuelle Zustand ausgegeben.

---

## Wann einsetzen

- Das Verhalten eines Objekts hängt stark von seinem Zustand ab, und es gibt viele Zustände.
- Der Code ist voller `switch`/`if` auf ein Statusfeld, die sich in verschiedenen Methoden wiederholen.
- Die Übergangsregeln zwischen den Zuständen sind komplex und ändern sich häufig.
- Beispiele: Bestellungen, Dokumentenworkflow (Entwurf → in Abstimmung → freigegeben), TCP-Verbindungen, Mediaplayer, Spielfiguren, endliche Automaten.

## Wann nicht nötig

- Es gibt wenige Zustände, und sie ändern sich selten – ein `switch` über ein `enum` ist dann einfacher und übersichtlicher.

## Vor- und Nachteile

**Vorteile**
- Beseitigt große bedingte Anweisungen.
- Der Code jedes Zustands steckt in einer eigenen Klasse (SRP).
- Neue Zustände kommen ohne Änderung der vorhandenen hinzu (Open/Closed).
- Die Übergänge sind explizit: Man sieht, aus welchem Zustand man wohin gelangen kann.

**Nachteile**
- Viele Klassen bei wenigen Zuständen.
- Die Übergangslogik ist auf die Zustandsklassen verteilt – den gesamten Automaten auf einen Blick zu erfassen ist schwieriger (ein Diagramm in der Dokumentation hilft).

## Beziehung zu anderen Mustern

- **Strategy** – die Struktur ist fast identisch (der Kontext delegiert an ein Objekt mit Schnittstelle). Der Unterschied liegt in der Absicht: Eine Strategie **wählt der Client**, und Strategien wissen nichts voneinander; Zustände **schalten sich gegenseitig selbst um** und kennen die Übergänge.
- **Flyweight** – Zustände ohne Felder können zwischen Kontexten geteilt werden (Beispiel `Vending`).
- **Singleton** – Zustandsobjekte werden manchmal als Singletons umgesetzt.
- **Bridge** und **Strategy** beruhen ebenfalls auf Komposition und Delegation, lösen aber andere Aufgaben.
