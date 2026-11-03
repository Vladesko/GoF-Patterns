🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Facade) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Facade) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Facade) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Facade)

# Facade (Fassade)

**Typ:** Strukturmuster.

**Facade** bietet eine **einfache Schnittstelle** zu einem komplexen Subsystem (einer Gruppe von Klassen, einer Bibliothek, einem Framework). Die Fassade weiß selbst, an welche Objekte des Subsystems und in welcher Reihenfolge eine Anfrage weiterzugeben ist.

---

## Welches Problem wird gelöst

Um im Heimkino einen Film anzusehen, muss man: das Licht dimmen, den Projektor einschalten, den Eingang wählen, den Breitbildmodus aktivieren, den Ton einschalten, Surround wählen, die Lautstärke einstellen, den Player einschalten und den Film starten. Und am Ende alles in der richtigen Reihenfolge wieder ausschalten.

Um in einem Onlineshop eine Bestellung aufzugeben: Bestand prüfen, Ware reservieren, Versandkosten berechnen, Geld abbuchen, bei Ablehnung die Reservierung aufheben, die Sendung anlegen, den Kunden benachrichtigen.

Erledigt der Client-Code (UI, Controller) das selbst:
- ist er **eng gekoppelt** an ein Dutzend Klassen des Subsystems und kennt die Details ihrer Arbeit;
- wird dieselbe Abfolge an allen Stellen, an denen sie gebraucht wird, **dupliziert**;
- bricht jede Änderung am Subsystem alle Clients.

## Lösung

Man erstellt eine **Fassaden**-Klasse, die:
- Referenzen auf die Objekte des Subsystems hält;
- einige High-Level-Methoden bereitstellt (`WatchMovie`, `PlaceOrder`);
- intern die benötigten Objekte des Subsystems in der richtigen Reihenfolge aufruft.

Der Client hängt nur von der Fassade ab. Das Subsystem wird dabei **nicht endgültig versteckt**: Braucht man etwas Ungewöhnliches, kann man seine Klassen direkt ansprechen.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Facade** | `HomeTheaterFacade`, `OrderFacade` | Bietet eine einfache Schnittstelle, weiß, an wen und in welcher Reihenfolge die Arbeit zu delegieren ist |
| **Subsystem classes** | `Projector`, `SoundSystem`, `MediaPlayer`, `Lights`, `InventoryService`, `PaymentGateway`, `ShippingService`, `NotificationService` | Erledigen die eigentliche Arbeit. **Wissen nichts von der Fassade** |
| **Client** | Demo-Klassen | Arbeitet über die Fassade mit dem Subsystem |

## Schema

```text
            Client
              │  WatchMovie("Inception")
              ▼
   ┌──────────────────────┐
   │  HomeTheaterFacade   │
   └──────────────────────┘
     │       │       │       │
     ▼       ▼       ▼       ▼
 Projector Sound  MediaPlayer Lights      ← Subsystem
            System
```

---

## Beispiel 1. Heimkino (`HomeTheater/`)

**Subsystem** – die Klassen im Ordner `Subsystems/` (`Projector`, `SoundSystem`, `MediaPlayer`, `Lights`). Jede hat ihre eigenen Low-Level-Methoden. Sie hängen weder voneinander noch von der Fassade ab.

**Facade** – `HomeTheaterFacade` erhält die Objekte des Subsystems über den Konstruktor und verbindet sie zu Szenarien:

```csharp
public void WatchMovie(string movie)
{
    lights.Dim(10);
    projector.On();
    projector.SetInput("HDMI-1");
    projector.WideScreenMode();
    sound.On();
    sound.SetSurroundSound();
    sound.SetVolume(25);
    player.On();
    player.Play(movie);
}
```

`EndMovie()` schaltet alles in umgekehrter Reihenfolge aus.

**Client** – ruft `theater.WatchMovie("Inception")` auf – eine Zeile statt zehn. Am Ende der Demo wird gezeigt, dass der direkte Zugriff auf das Subsystem (`new Lights().Dim(50)`) weiterhin möglich ist.

## Beispiel 2. Bestellabwicklung (`OrderPlacement/`)

**Aufgabe:** Ein Geschäftsprozess, der mehrere Dienste betrifft – mit Prüfungen und Rollback im Fehlerfall.

**Subsystem** – `InventoryService` (Bestand und Reservierung), `PaymentGateway` (Abbuchung; Karten mit Endung `0000` werden abgelehnt), `ShippingService` (Kosten und Anlegen der Sendung), `NotificationService` (E-Mails).

**Facade** – `OrderFacade.PlaceOrder(OrderRequest)` kapselt den gesamten Prozess:

```csharp
if (!inventory.IsAvailable(order.Product, order.Count))
    return new OrderResult(false, $"{order.Product} ist nicht vorrätig");

inventory.Reserve(order.Product, order.Count);

var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
if (!payments.Charge(order.CardNumber, total))
{
    inventory.Release(order.Product, order.Count);   // Rollback
    return new OrderResult(false, "Zahlung wurde abgelehnt");
}
```

Die Fassade kennt nicht nur die Reihenfolge der Aufrufe, sondern auch die **Abstimmungsregeln** der Subsysteme: zum Beispiel, dass die Reservierung aufgehoben werden muss, wenn die Zahlung fehlschlägt. Ein- und Ausgabedaten sind in einfache `record`s verpackt (`OrderRequest`, `OrderResult`), damit der Client die Typen des Subsystems nicht kennen muss.

**Client** – die Demo schickt drei Bestellungen ab: eine erfolgreiche, eine mit nicht vorrätiger Ware und eine mit abgelehnter Karte. Der Client sieht nur das Ergebnis.

In einer echten ASP.NET-Core-Anwendung heißt eine solche Fassade meist „Anwendungsdienst“ (`OrderService`, `CheckoutService`), und der Controller hängt nur von ihm ab.

---

## Wann einsetzen

- Man braucht eine einfache Schnittstelle zu einem komplexen Subsystem oder einer Fremdbibliothek.
- Die Kopplung des Clients an viele Klassen soll verringert werden.
- Das System soll in Schichten aufgeteilt werden: Die Fassade ist der Einstiegspunkt in jede Schicht bzw. jedes Modul.

## Wann nicht nötig

- Das Subsystem ist ohnehin einfach – dann wird die Fassade zur überflüssigen Zwischenschicht.
- Die Clients brauchen ständig unterschiedliche Low-Level-Funktionen – dann stört die Fassade nur.

## Vor- und Nachteile

**Vorteile**
- Isoliert die Clients von den Komponenten des Subsystems.
- Verringert die Kopplung: Das Subsystem kann geändert werden, ohne die Clients anzufassen.
- Beseitigt duplizierte Szenarien.

**Nachteile**
- Gefahr, zu einem **„Gottobjekt“** zu werden, das alles weiß und alles tut. Besser mehrere kleine, aufgabenbezogene Fassaden.

## Beziehung zu anderen Mustern

- **Adapter** passt **eine** vorhandene Schnittstelle an die benötigte an; **Facade** entwirft eine **neue, vereinfachte** Schnittstelle für ein ganzes Subsystem.
- **Mediator** zentralisiert ebenfalls die Interaktion, aber die Objekte des Subsystems **kennen** den Vermittler und kommunizieren über ihn; bei Facade weiß das Subsystem nichts von der Fassade, und die Verbindung geht nur in eine Richtung.
- **Abstract Factory** kann zusammen mit Facade verwendet werden, um zu verbergen, wie die Objekte des Subsystems erzeugt werden.
- Eine Fassade wird oft als **Singleton** umgesetzt, da meist eine Instanz genügt.
- **Proxy** hat dieselbe Schnittstelle wie das Objekt; **Facade** hat eine eigene.
