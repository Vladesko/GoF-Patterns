🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/FactoryMethod) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/FactoryMethod) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/FactoryMethod) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/FactoryMethod)

# Factory Method (Fabrikmethode)

**Typ:** Erzeugungsmuster.

**Factory Method** definiert in der Basisklasse eine Methode zur Erzeugung eines Objekts, überlässt es aber **den Unterklassen zu entscheiden, welche Klasse genau erzeugt wird**. Der Code, der das Objekt verwendet, hängt nur von dessen Schnittstelle ab, nicht von der konkreten Klasse.

---

## Welches Problem wird gelöst

Ein Benachrichtigungsdienst konnte anfangs nur SMS versenden, und der Code sah so aus:

```csharp
var sms = new SmsNotification();
sms.Notify(message);
```

Dann wurden E-Mail und Push benötigt. Macht man so weiter, tauchen im Code `switch`/`if` mit `new` für jeden Typ auf – an allen Stellen, an denen Benachrichtigungen versendet werden. Der Code ist fest an konkrete Klassen gebunden, und ein neuer Kanal erfordert Änderungen im gesamten Projekt.

Dasselbe gilt für Mitgliedschaften in einem Fitnessclub: „Fitnessstudio“, „Fitnessstudio + Schwimmbad“, „Personal Training“. Ruft der Client-Code selbst `new GymMembership()` auf, hängt er von allen Mitgliedschaftsarten ab.

## Lösung

Der direkte Aufruf von `new` wird durch den Aufruf einer speziellen **Fabrikmethode** ersetzt. Die Methode ist in der Basisklasse (dem Erzeuger) deklariert und gibt die **Produktschnittstelle** zurück; die erzeugenden Unterklassen überschreiben sie und liefern konkrete Produkte.

Wichtig: Der Erzeuger hat meist auch **eigene Logik**, die über die Schnittstelle mit dem Produkt arbeitet und nicht weiß, welches Produkt genau erzeugt wurde (im Benachrichtigungsbeispiel die Methode `Notify`).

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Product** | `INotificationService`, `IMembership` | Gemeinsame Schnittstelle der erzeugten Objekte |
| **ConcreteProduct** | `SmsNotification`, `UmailNotification`, `PushNotification`, `GymMembership`, `GymPoolMembership`, `PersonalTrainingMembership` | Konkrete Implementierungen des Produkts |
| **Creator** | `NotificationFactory`, `MembershipFactory` | Deklariert die Fabrikmethode; kann Logik enthalten, die mit dem Produkt arbeitet |
| **ConcreteCreator** | `SmsNotificator`, `UmailNotificator`, `PushNotificator`, `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` | Überschreibt die Fabrikmethode und liefert ein konkretes Produkt |
| **Client** | `DemondtrsteFactoryMethod` | Arbeitet mit Erzeugern und Produkten |

## Schema

```text
 ┌───────────────────────────────────────────┐          ┌─────────────────────────────┐
 │ «abstract» NotificationFactory (Creator)  │          │ «interface»                 │
 │ # CreateNotification(): INotificationService ──────►│ INotificationService        │
 │ + Notify(message):                        │ erzeugt  │ Notify(message)             │
 │     var s = CreateNotification();         │          └─────────────────────────────┘
 │     s.Notify(message);                    │                ▲        ▲        ▲
 └───────────────────────────────────────────┘                │        │        │
        ▲                ▲                ▲          SmsNotification  Umail   Push
        │                │                │                          Notification Notification
  SmsNotificator   UmailNotificator  PushNotificator
  Create… => new SmsNotification()  ...
```

---

## Beispiel 1. Benachrichtigungssystem (`NotifySystem/`) – Erzeuger mit eigener Logik

**Product** – `INotificationService` mit der Methode `Notify(string message)`.

**ConcreteProduct** (Ordner `NotifiationServices/`) – `SmsNotification`, `UmailNotification`, `PushNotification`: Jede „versendet“ die Nachricht über ihren eigenen Kanal.

**Creator** – die abstrakte Klasse `NotificationFactory` (Ordner `Factories/`):

```csharp
public abstract class NotificationFactory
{
    protected abstract INotificationService CreateNotification();   // Fabrikmethode

    public void Notify(string message)                               // Logik des Erzeugers
    {
        var notificationService = CreateNotification();
        notificationService.Notify(message);
    }
}
```

Das ist die klassische Form des Musters: Die Methode `Notify` ist **einmal** in der Basisklasse implementiert und arbeitet mit dem Produkt nur über die Schnittstelle. Die Fabrikmethode ist `protected` – der Client erzeugt das Produkt nicht selbst, sondern ruft einfach `Notify` auf.

**ConcreteCreator** – `SmsNotificator`, `UmailNotificator`, `PushNotificator` überschreiben nur die Fabrikmethode:

```csharp
internal class SmsNotificator : NotificationFactory
{
    protected override INotificationService CreateNotification() => new SmsNotification();
}
```

**Client** – `DemondtrsteFactoryMethod.DemonstrateNotify()` erzeugt den passenden Erzeuger und ruft `Notify` auf. Um zum Beispiel Telegram hinzuzufügen, braucht man zwei Klassen – `TelegramNotification` und `TelegramNotificator`; der vorhandene Code bleibt unverändert.

## Beispiel 2. Mitgliedschaften im Fitnessclub (`Fitnes/`) – Produktfabriken

**Product** – `IMembership` (Name, Beschreibung, Preis).

**ConcreteProduct** (Ordner `Memberships/`):
- `GymMembership` – Fitnessstudio, 50;
- `GymPoolMembership` – Fitnessstudio + Schwimmbad, 75;
- `PersonalTrainingMembership` – Personal Training, 100.

**Creator** – die abstrakte Klasse `MembershipFactory` mit einer öffentlichen Fabrikmethode:

```csharp
internal abstract class MembershipFactory
{
    public abstract IMembership CreateMembership();
}
```

**ConcreteCreator** (Ordner `Factories/`) – `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` – jede liefert ihre eigene Mitgliedschaft.

**Client** – `DemondtrsteFactoryMethod.DemonstrateFitnes()` wählt die passende Fabrik, erhält die Mitgliedschaft über `CreateMembership()` und arbeitet danach mit ihr als `IMembership`.

Hier hat der Erzeuger keine eigene Logik – das ist eine vereinfachte Form des Musters, die in der Praxis häufig verwendet wird: Eine Fabrik (z. B. anhand der Einstellungen oder der Benutzerauswahl gewählt) wird an Code übergeben, der Objekte erzeugt, ohne ihre Klassen zu kennen.

---

## Wann einsetzen

- Es ist vorab nicht bekannt, Objekte welcher Klassen erzeugt werden müssen.
- Die erzeugten Objekte sollen über Vererbung erweiterbar sein (Frameworks, Bibliotheken).
- Die Basisklasse enthält einen allgemeinen Algorithmus, und das konkrete Produkt dafür bestimmen die Unterklassen.
- Die Objekterzeugung soll zentralisiert werden, um Objekte bei Bedarf wiederzuverwenden (Cache, Pool).

## Wann nicht nötig

- Es gibt nur ein Produkt, und es wird sich nicht ändern – ein einfaches `new` ist verständlicher.
- Es sollen **Familien** zueinander passender Objekte erzeugt werden – das ist Abstract Factory.

## Vor- und Nachteile

**Vorteile**
- Befreit den Code von der Bindung an konkrete Produktklassen.
- Der Erzeugungscode der Produkte liegt an einer Stelle (SRP).
- Neue Produkte kommen ohne Änderung am vorhandenen Code hinzu (Open/Closed).

**Nachteile**
- Für jedes Produkt eine eigene Erzeuger-Unterklasse, die Hierarchien wachsen parallel.

## Beziehung zu anderen Mustern

- **Abstract Factory** wird oft durch eine Reihe von Fabrikmethoden umgesetzt, erzeugt aber eine **Familie** von Produkten und wird als Objekt übergeben.
- **Template Method** – eine Fabrikmethode ist oft ein Schritt einer Schablonenmethode: `NotificationFactory.Notify()` ist eine Schablonenmethode, die die Fabrikmethode `CreateNotification()` aufruft.
- **Prototype** braucht keine Erzeuger-Unterklassen, erfordert aber die Initialisierung der Prototypen. Bei Factory Method ist es umgekehrt.
- **Iterator** – `GetEnumerator()` in .NET-Collections ist eine Fabrikmethode, die einen Iterator liefert.
- Factory Method entwickelt sich oft zu **Builder** oder **Abstract Factory**, wenn die Erzeugung komplizierter wird.
