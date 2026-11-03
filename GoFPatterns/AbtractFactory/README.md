🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/AbtractFactory) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/AbtractFactory) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/AbtractFactory) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/AbtractFactory)

# Abstract Factory (Abstrakte Fabrik)

**Typ:** Erzeugungsmuster.

**Abstract Factory** ermöglicht es, **Familien verwandter Objekte** zu erzeugen, ohne sich an die konkreten Klassen dieser Objekte zu binden. Der Client arbeitet mit der Fabrik und den Produkten nur über Schnittstellen.

---

## Welches Problem wird gelöst

Ein Zahlungsdienst arbeitet mit Zahlungsanbietern (im Beispiel die fiktiven **PoolPay** und **Strope**). Für jeden Anbieter wird ein **Satz** von Objekten benötigt:
- ein Zahlungsprozessor;
- ein Erstattungsprozessor;
- ein Zahlungsvalidator.

Diese Objekte bilden eine **Familie**: Der PoolPay-Prozessor muss mit dem Validator und den Erstattungen von PoolPay zusammenarbeiten. Den PoolPay-Prozessor mit Strope-Erstattungen zu mischen ist ein Fehler (die Transaktion des einen Anbieters existiert beim anderen nicht).

Wenn der Zahlungsdienst die Objekte selbst erzeugt:

```csharp
if (provider == "PoolPay") { processor = new PoolPayPaymentProcess(); refund = new PoolPayRefundProcessor(); ... }
else if (provider == "Strope") { processor = new StropePaymentProcessor(); refund = new StropeRefundProcessor(); ... }
```

- hängt der Dienst von **allen** konkreten Klassen aller Anbieter ab;
- erfordert jeder neue Anbieter eine Änderung am Dienst;
- ist es leicht, versehentlich einen „inkompatiblen Satz“ zu erzeugen.

## Lösung

1. Für jede Produktart eine **Schnittstelle** definieren (`IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator`).
2. Eine **abstrakte Fabrik** deklarieren – eine Schnittstelle mit Erzeugungsmethoden für jedes Produkt der Familie.
3. Für jede Familie (jeden Anbieter) eine **konkrete Fabrik** anlegen, die nur Produkte ihrer eigenen Familie erzeugt.
4. Der Client erhält die Fabrik von außen und erzeugt Produkte nur über sie – so kennt er die konkreten Klassen nicht und **kann die Familien nicht mischen**.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **AbstractFactory** | `IPaymentProviderFactory` | Deklariert die Erzeugungsmethoden für alle Produkte der Familie |
| **ConcreteFactory** | `PoolPayPaymentFactory`, `StropePaymentFactory` | Erzeugt die Produkte einer Familie (eines Anbieters) |
| **AbstractProduct** | `IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator` | Schnittstellen der Produkte |
| **ConcreteProduct** | `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`, `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator` | Konkrete Implementierungen der Produkte für jeden Anbieter |
| **Client** | `PaymentService` (+ `IPaymentService`) | Verwendet nur die Schnittstellen von Fabrik und Produkten |

## Schema

```text
                       «interface» IPaymentProviderFactory
                         CreatePaymentProcess(): IPaymentProcessor
                         CreateRefundProcess():  IRefundProcessor
                         CreatePaymentValidator(): IPaymentValidator
                              ▲                         ▲
                              │                         │
               PoolPayPaymentFactory            StropePaymentFactory
                 │      │       │                 │      │       │
                 ▼      ▼       ▼                 ▼      ▼       ▼
         PoolPayPayment PoolPay PoolPay      StropePayment Strope Strope
           Process      Refund  Validator      Processor   Refund Validator

 PaymentService(IPaymentProviderFactory factory)   ← Client
   Pay(amount):  validator.ValidatePayment → processor.ProcessPayment
   Refund(id):   refundProcessor.ProcessRefund
```

---

## Beispiel. Zahlungsanbieter (`Payments/`)

**AbstractProduct** (Ordner `Interfaces/Abstractions/`):

```csharp
public interface IPaymentProcessor { Guid ProcessPayment(decimal amount); }
public interface IRefundProcessor  { void ProcessRefund(Guid transactionId); }
public interface IPaymentValidator { bool ValidatePayment(decimal amount); }
```

**AbstractFactory** (Ordner `Interfaces/AbstractFactories/`):

```csharp
public interface IPaymentProviderFactory
{
    IPaymentProcessor CreatePaymentProcess();
    IRefundProcessor CreateRefundProcess();
    IPaymentValidator CreatePaymentValidator();
}
```

**ConcreteFactory + ConcreteProduct** – jeder Anbieter hat seinen eigenen Ordner:
- `PoolPays/` – `PoolPayPaymentFactory` erzeugt `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`;
- `Stopes/` – `StropePaymentFactory` erzeugt `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator`.

Alle konkreten Klassen sind als `internal` markiert – außerhalb der Assembly weiß niemand von ihnen, zugänglich sind nur die Schnittstellen.

```csharp
internal class PoolPayPaymentFactory : IPaymentProviderFactory
{
    public IPaymentProcessor CreatePaymentProcess() => new PoolPayPaymentProcess();
    public IPaymentValidator CreatePaymentValidator() => new PoolPayPaymentValidator();
    public IRefundProcessor CreateRefundProcess() => new PoolPayRefundProcessor();
}
```

**Client** – `PaymentService` (Ordner `Services/`) erhält die Fabrik im Konstruktor und erzeugt über sie **einmalig** den gesamten Produktsatz. Danach arbeitet er nur noch mit Schnittstellen:

```csharp
internal class PaymentService(IPaymentProviderFactory paymentProviderFactory) : IPaymentService
{
    private readonly IPaymentProcessor _paymentProcessor = paymentProviderFactory.CreatePaymentProcess();
    private readonly IRefundProcessor _refundProcessor = paymentProviderFactory.CreateRefundProcess();
    private readonly IPaymentValidator _paymentValidator = paymentProviderFactory.CreatePaymentValidator();

    public Guid Pay(decimal amount)
    {
        if (!_paymentValidator.ValidatePayment(amount))
            throw new InvalidOperationException("Ungültiger Zahlungsbetrag.");
        return _paymentProcessor.ProcessPayment(amount);
    }

    public void Refund(Guid transactionId) => _refundProcessor.ProcessRefund(transactionId);
}
```

Da alle drei Produkte von derselben Fabrik stammen, gehören sie **garantiert zur selben Familie**.

**Demonstration** – `DemontrateAbstractFactory` erzeugt `PaymentService` zuerst mit der PoolPay-Fabrik, dann mit Strope, und führt jeweils dieselben Operationen `Pay` + `Refund` aus. Der Code des Dienstes ist derselbe – es ändert sich nur die übergebene Fabrik.

Um einen neuen Anbieter anzubinden, genügt es, einen Ordner mit einer Fabrik und drei Produkten hinzuzufügen. `PaymentService` und die vorhandenen Anbieter bleiben unverändert.

---

## Wann einsetzen

- Das System soll mit verschiedenen **Familien** verwandter Objekte arbeiten, die **gemeinsam** verwendet werden müssen.
- Die konkreten Produktklassen sollen vor dem Client verborgen werden.
- Die Familie wird einmal gewählt (per Konfiguration, Einstellungen) und dann überall verwendet.
- Beispiele: Zahlungsanbieter, UI-Themes (Button + Checkbox + Fenster für Windows/macOS), Datenbankanbieter (`DbProviderFactory` in ADO.NET: Verbindung + Befehl + Parameter), Cloud-Anbieter (Speicher + Queue + Cache).

## Wann nicht nötig

- Es gibt nur ein Produkt statt einer Familie – Factory Method genügt.
- Es gibt nur eine Familie, und sie wird nicht erweitert.

## Vor- und Nachteile

**Vorteile**
- Garantiert die Kompatibilität der erzeugten Produkte.
- Der Client hängt nicht von konkreten Klassen ab.
- Der Erzeugungscode der Produkte ist an einer Stelle gebündelt (SRP).
- Eine neue Familie kommt ohne Änderung am Client hinzu (Open/Closed).

**Nachteile**
- Viele Schnittstellen und Klassen.
- Eine **neue Produktart** (z. B. `ISubscriptionProcessor`) hinzuzufügen ist aufwendig: Man muss die Fabrikschnittstelle und **alle** konkreten Fabriken ändern.

## Beziehung zu anderen Mustern

- **Factory Method** – eine abstrakte Fabrik besteht oft aus mehreren Fabrikmethoden. Factory Method erzeugt **ein** Produkt und stützt sich auf Vererbung; Abstract Factory erzeugt eine **Familie** und wird als Objekt übergeben (Komposition).
- **Builder** baut **ein komplexes** Objekt schrittweise; Abstract Factory liefert sofort einen **Satz** einfacher Objekte.
- **Prototype** – die Fabrik kann Produkte durch Klonen von Prototypen erzeugen.
- **Singleton** – eine konkrete Fabrik wird meist nur in einer Instanz benötigt.
- **Facade** – eine abstrakte Fabrik kann statt einer Fassade verwendet werden, um zu verbergen, wie die Objekte des Subsystems erzeugt werden.
- **Strategy** – aus Sicht von `PaymentService` sieht die Fabrik wie eine Strategie „welcher Anbieter verwendet wird“ aus.
