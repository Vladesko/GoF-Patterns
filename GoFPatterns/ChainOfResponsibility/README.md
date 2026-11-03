🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/ChainOfResponsibility) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/ChainOfResponsibility) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/ChainOfResponsibility) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/ChainOfResponsibility)

# Chain of Responsibility (Zuständigkeitskette)

**Typ:** Verhaltensmuster.

**Chain of Responsibility** ermöglicht es, eine Anfrage nacheinander durch eine **Kette von Handlern** zu reichen. Jeder Handler entscheidet: die Anfrage selbst bearbeiten, sie entlang der Kette weitergeben – oder beides.

---

## Welches Problem wird gelöst

**Kundensupport.** Ein Ticket kann vom Bot (bei einer häufigen Frage), vom Operator (einfaches Problem), vom Ingenieur (Bug) oder vom Manager (Störung) geschlossen werden. Schreibt man das in einer Methode:

```csharp
if (ticket.Severity == Low && IsFaq(ticket)) Bot(...);
else if (ticket.Severity <= Medium) Operator(...);
else if (ticket.Severity == High) Engineer(...);
else Manager(...);
```

…dann erfordert jede neue Support-Ebene, jede Änderung der Reihenfolge oder eine Nachtschicht „ohne Bot und Manager“ eine Änderung dieser Methode. Der Absender ist fest an alle Empfänger gekoppelt.

**Verarbeitung einer HTTP-Anfrage.** Bevor die Anfrage die Geschäftslogik erreicht, muss sie protokolliert, das Anfragelimit geprüft, authentifiziert und validiert werden. Jede Prüfung kann die Verarbeitung **stoppen**. Schreibt man all das in den Controller, wächst er an und vermischt verschiedene Zuständigkeiten.

## Lösung

Jede Prüfung/Verarbeitung wird zu einem eigenen **Handler**-Objekt mit gemeinsamer Schnittstelle. Die Handler werden zu einer Kette verbunden: Jeder kennt nur den **nächsten**. Der Client schickt die Anfrage an den **ersten** Handler und weiß nicht, wer sie letztlich bearbeitet.

Es gibt zwei Verhaltensvarianten der Kette:
1. **Klassisch (GoF):** Die Anfrage wird von **einem** Handler bearbeitet – dem ersten, der es kann; die übrigen geben sie einfach weiter (Beispiel `SupportTickets`).
2. **Pipeline/Middleware:** **Jeder** Handler erledigt seinen Teil der Arbeit und entscheidet, ob der nächste aufgerufen wird; jeder kann die Kette unterbrechen (Beispiel `Middleware`).

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Handler** | `SupportHandler`, `IRequestHandler` | Gemeinsame Schnittstelle der Handler. Enthält oft eine Referenz auf den nächsten Handler (oder erhält ihn als Delegaten) |
| **ConcreteHandler** | `ChatBotHandler`, `OperatorHandler`, `EngineerHandler`, `ManagerHandler`, `LoggingHandler`, `RateLimitHandler`, `AuthenticationHandler`, `ValidationHandler`, `EndpointHandler` | Entscheidet, ob er die Anfrage bearbeiten kann; falls nicht, gibt er sie weiter |
| **Request** | `SupportTicket`, `HttpRequestContext` | Die Daten, die durch die Kette wandern |
| **Client** | Demo-Klassen, `RequestPipeline` | Baut die Kette auf und schickt die Anfrage an das erste Glied |

## Schema

```text
 Client ──► ChatBot ──► Operator ──► Engineer ──► Manager ──► (null)
              │            │            │            │
          Low+FAQ?     ≤ Medium?     High?       Critical?
         bearbeiten   bearbeiten   bearbeiten   bearbeiten
```

---

## Beispiel 1. Kundensupport (`SupportTickets/`) – klassische Kette

**Request** – `SupportTicket(Id, Description, Severity)` (`record`), die Dringlichkeit ist `TicketSeverity`.

**Handler** – die abstrakte Klasse `SupportHandler` hält eine Referenz auf den nächsten Handler und implementiert das Standardverhalten – weitergeben:

```csharp
public SupportHandler SetNext(SupportHandler next)
{
    _next = next;
    return next;           // next zurückgeben, um die Kette „in einer Linie“ aufzubauen
}

public virtual void Handle(SupportTicket ticket)
{
    if (_next is not null)
        _next.Handle(ticket);
    else
        Console.WriteLine($"Niemand konnte Ticket Nr. {ticket.Id} bearbeiten");
}
```

**ConcreteHandler** (Ordner `Handlers/`) – jeder prüft „seine“ Bedingung; trifft sie zu, bearbeitet er die Anfrage und **stoppt** die Kette (`return`), andernfalls ruft er `base.Handle(ticket)` auf:

```csharp
public override void Handle(SupportTicket ticket)
{
    if (ticket.Severity <= TicketSeverity.Medium)
    {
        Console.WriteLine($"Operator: Ticket Nr. {ticket.Id} gelöst");
        return;
    }
    base.Handle(ticket);
}
```

**Client** – baut die Kette `Bot → Operator → Ingenieur → Manager` auf und schickt alle Tickets **nur an das erste Glied**. In der Ausgabe sieht man, wie weit jedes Ticket gekommen ist. Am Ende wird eine andere Konfiguration gezeigt („Nachtschicht“ – nur der Operator): Das kritische Ticket übernimmt niemand – das ist für das Muster eine normale Situation, die man einplanen muss.

## Beispiel 2. Pipeline zur Anfrageverarbeitung (`Middleware/`) – wie in ASP.NET Core

**Request** – `HttpRequestContext`: Eingabedaten (Pfad, IP, Token, Body) und Ergebnis (Statuscode, Text). Handler können den Kontext **anreichern** – zum Beispiel schreibt die Authentifizierung den `UserName` hinein.

**Handler** – `IRequestHandler.Handle(context, Action next)`. Der Handler hält keine Referenz auf den nächsten, sondern erhält ihn als **Delegaten** – so sind die Handler völlig unabhängig.

**ConcreteHandler** (Ordner `Handlers/`):
- `LoggingHandler` – arbeitet **vor und nach** dem Rest der Kette (schreibt die Anfrage, ruft `next()` auf, schreibt die Antwort);
- `RateLimitHandler` – antwortet nach N Anfragen von derselben IP mit `429` und ruft `next()` **nicht** auf;
- `AuthenticationHandler` – prüft das Token (`401`), andernfalls schreibt er den Benutzer in den Kontext;
- `ValidationHandler` – prüft den Body (`400`);
- `EndpointHandler` – der Endpunkt, erledigt die Arbeit (`201`).

**Client** – `RequestPipeline` baut die Kette mit der Methode `Use(...)` auf und verknüpft die Handler rekursiv:

```csharp
private void Invoke(int index, HttpRequestContext context)
{
    if (index >= _handlers.Count) return;
    _handlers[index].Handle(context, () => Invoke(index + 1, context));
}
```

Das ist eine vereinfachte Kopie dessen, wie `app.Use(...)` in ASP.NET Core funktioniert: Jede Middleware erhält `next` und entscheidet selbst, ob sie die Anfrage weitergibt. Ketten sind auch `DelegatingHandler` in `HttpClient` und die Pipeline Behaviors in MediatR.

---

## Wann einsetzen

- Eine Anfrage kann von verschiedenen Objekten bearbeitet werden, und es ist vorab nicht bekannt, von welchem.
- Menge und Reihenfolge der Handler sollen dynamisch konfigurierbar sein.
- Mehrere unabhängige Prüfungen/Schritte sollen nacheinander ausgeführt werden, mit der Möglichkeit abzubrechen.
- Der Absender einer Anfrage soll von den Empfängern entkoppelt werden.

## Wann nicht nötig

- Es gibt immer genau einen Handler, und er ist vorab bekannt.
- Reihenfolge und Menge der Schritte ändern sich nie – einfacher sequenzieller Code ist leichter zu lesen.

## Vor- und Nachteile

**Vorteile**
- Verringert die Kopplung zwischen Absender und Empfängern.
- Jeder Handler ist für eine Sache zuständig (SRP).
- Neue Handler kommen ohne Änderung der vorhandenen hinzu (Open/Closed).
- Die Kette lässt sich zur Laufzeit umbauen.

**Nachteile**
- Eine Anfrage kann unbearbeitet bleiben.
- Schwieriger zu debuggen: Der Weg der Anfrage ist im Code nicht an einer Stelle sichtbar.
- Die Reihenfolge der Glieder ist wichtig (z. B. muss Logging zuerst stehen und die Authentifizierung vor der Validierung).

## Beziehung zu anderen Mustern

- **Decorator** ist ähnlich aufgebaut (eine Kette von Hüllen), aber ein Dekorierer gibt den Aufruf **immer** weiter und ergänzt Verhalten; ein Kettenglied kann die Verarbeitung **abbrechen**.
- **Command** – die Anfrage in der Kette wird oft als Befehl modelliert.
- **Composite** – die Kette wird oft entlang eines Baums aufgebaut: Die Anfrage geht von einem Element zu seinem Elternelement (z. B. Ereignisverarbeitung in UIs).
- **Mediator** und **Observer** – andere Wege, Absender und Empfänger zu verbinden: über einen Vermittler oder über ein Abonnement.
