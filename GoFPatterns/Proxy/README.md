🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Proxy) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Proxy) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Proxy) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Proxy)

# Proxy (Stellvertreter)

**Typ:** Strukturmuster.

**Proxy** ist ein Stellvertreterobjekt, das **dieselbe Schnittstelle** wie das echte Objekt implementiert und den **Zugriff** darauf **kontrolliert**: Es fängt Aufrufe ab und kann vor oder nach der Weitergabe an das Original etwas tun (oder den Aufruf gar nicht weitergeben).

---

## Welches Problem wird gelöst

Es gibt ein Objekt, auf das man nicht direkt zugreifen kann oder sollte:

- es ist **schwer** – eine Galerie mit 100 Fotos zu je 20 MB sollte beim Öffnen nicht alle laden, wenn der Benutzer sich nur zwei ansieht;
- es braucht eine **Zugriffskontrolle** – nur der Inhaber darf Geld vom Konto abheben;
- seine Aufrufe sind **teuer**, und das Ergebnis sollte **gecacht** werden – die Kontostandsabfrage beim Kernbankensystem;
- Zugriffe sollen **protokolliert/auditiert** werden;
- das Objekt befindet sich **entfernt** (ein anderer Dienst, ein anderer Rechner).

All das in die Klasse selbst einzubauen ist schlecht: Sie wäre dann für Sicherheit, Caching und Logging statt für ihre Geschäftslogik zuständig. Und oft kann man die Klasse gar nicht ändern (Fremdcode).

## Lösung

Man erstellt eine Stellvertreterklasse mit derselben Schnittstelle. Der Stellvertreter hält eine Referenz auf das echte Objekt (oder erzeugt es bei Bedarf selbst) und delegiert die Arbeit daran, ergänzt um seine eigene Logik. Der Client arbeitet mit der Schnittstelle und **bemerkt den Austausch nicht**.

### Arten von Stellvertretern

| Art | Was er tut | Beispiel im Projekt |
|-----|-----------|------------------|
| **Virtual Proxy** | Verschiebt die Erzeugung eines schweren Objekts bis zum ersten Zugriff | `LazyImageProxy` |
| **Protection Proxy** | Prüft Zugriffsrechte | `ProtectionAccountProxy` |
| **Caching Proxy** | Cacht Ergebnisse | `CachingAccountProxy` |
| **Logging Proxy** | Protokolliert Zugriffe | `LoggingAccountProxy` |
| **Remote Proxy** | Repräsentiert ein Objekt aus einem anderen Adressraum (gRPC-/WCF-Clients, `HttpClient`-Wrapper) | — |
| **Smart Reference** | Referenzzählung, Freigabe von Ressourcen, wenn das Objekt nicht mehr gebraucht wird | — |

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Subject** | `IImage`, `IBankAccount` | Gemeinsame Schnittstelle von echtem Objekt und Stellvertreter. Dank ihr kann der Stellvertreter anstelle des Originals eingesetzt werden |
| **RealSubject** | `HighResolutionImage`, `BankAccount` | Das echte Objekt mit der eigentlichen Geschäftslogik |
| **Proxy** | `LazyImageProxy`, `ProtectionAccountProxy`, `CachingAccountProxy`, `LoggingAccountProxy` | Hält eine Referenz auf RealSubject, kontrolliert den Zugriff darauf und kann seinen Lebenszyklus verwalten |
| **Client** | Demo-Klassen | Arbeitet über Subject und weiß nicht, ob er das Original oder einen Stellvertreter vor sich hat |

## Schema

```text
               ┌──────────────────────┐
  Client ─────►│ «interface» IImage   │
               │ Display()            │
               └──────────────────────┘
                    ▲             ▲
                    │             │
     ┌──────────────────┐   ┌───────────────────────┐
     │ LazyImageProxy   │──►│ HighResolutionImage   │
     │ - _realImage?    │   │ (schwer, wird im      │
     │ Display():       │   │  Konstruktor geladen) │
     │  erzeugen falls  │   └───────────────────────┘
     │  null, dann      │
     │  delegieren      │
     └──────────────────┘
```

---

## Beispiel 1. Verzögertes Laden von Bildern (`ImageLoading/`) – Virtual Proxy

**Subject** – `IImage` (`FileName`, `Display()`).

**RealSubject** – `HighResolutionImage` lädt das Bild direkt im Konstruktor von der Festplatte (simuliert über `Thread.Sleep`). Schon das Erzeugen eines solchen Objekts ist teuer.

**Proxy** – `LazyImageProxy` speichert nur den Dateinamen und erzeugt das echte Objekt beim **ersten** Aufruf von `Display()`:

```csharp
public void Display()
{
    _realImage ??= new HighResolutionImage(FileName);
    _realImage.Display();
}
```

**Client** – die Demo öffnet eine Galerie mit 5 Bildern auf zwei Arten:
- ohne Stellvertreter – alle 5 Dateien werden sofort geladen (~1 s);
- mit Stellvertreter – die Galerie öffnet sich sofort, und geladen werden nur die 2 Bilder, die der Benutzer geöffnet hat. Eine erneute Anzeige lädt die Datei nicht noch einmal.

In .NET spielt `Lazy<T>` eine ähnliche Rolle, in Entity Framework die **Lazy-Loading-Proxys** für Navigationseigenschaften.

## Beispiel 2. Bankkonto (`BankAccount/`) – Protection, Caching, Logging

**Subject** – `IBankAccount` (`GetBalance()`, `Withdraw()`).

**RealSubject** – `BankAccount` enthält **nur** Geschäftslogik. Die Kontostandsabfrage ist langsam (Simulation eines Zugriffs auf das Kernbankensystem), der Zähler `BalanceRequests` zeigt, wie oft tatsächlich bei der Bank angefragt wurde.

**Proxies** (Ordner `Proxies/`):

`ProtectionAccountProxy` – Geld abheben darf nur `UserRole.Owner`:

```csharp
public void Withdraw(decimal amount)
{
    if (role != UserRole.Owner)
        throw new UnauthorizedAccessException($"{role} darf kein Geld abheben.");
    account.Withdraw(amount);
}
```

`CachingAccountProxy` – merkt sich den Kontostand und **leert den Cache** nach einer Abbuchung:

```csharp
public decimal GetBalance() => _cachedBalance ??= account.GetBalance();
```

`LoggingAccountProxy` – schreibt ein Audit aller Operationen, auch der fehlgeschlagenen.

**Client** – da alle Stellvertreter `IBankAccount` implementieren, lassen sie sich ineinander verschachteln:

```csharp
IBankAccount ownerAccess = new LoggingAccountProxy(
    new ProtectionAccountProxy(
        new CachingAccountProxy(realAccount), UserRole.Owner));
```

Die Demo zeigt: Drei Kontostandsabfragen ergeben **eine** echte Anfrage an die Bank; nach der Abbuchung wird der Cache geleert und der Kontostand ist aktuell; der Gast wird abgewiesen, und das landet im Audit.

---

## Wann einsetzen

- Verzögerte Initialisierung eines schweren Objekts (Virtual Proxy).
- Zugriffskontrolle (Protection Proxy).
- Caching von Ergebnissen (Caching Proxy).
- Logging, Metriken, Audit von Zugriffen.
- Arbeit mit einem entfernten Objekt wie mit einem lokalen (Remote Proxy).

## Wann nicht nötig

- Das Objekt ist leichtgewichtig und braucht keine Kontrolle – dann ist es eine überflüssige Indirektionsebene.
- Es geht nicht um Kontrolle, sondern darum, neues Verhalten **hinzuzufügen**, das der Client selbst wählt und kombiniert – das liegt näher an Decorator.

## Vor- und Nachteile

**Vorteile**
- Kontrolle über das Objekt, unbemerkt vom Client.
- Der Lebenszyklus des echten Objekts lässt sich steuern (später erzeugen, früher freigeben).
- Funktioniert auch, wenn das echte Objekt noch nicht erzeugt oder nicht erreichbar ist.
- Open/Closed: Neue Stellvertreter kommen ohne Änderung an Dienst und Client hinzu.

**Nachteile**
- Eine zusätzliche Indirektionsebene, Antwortverzögerungen sind möglich.
- Die Zahl der Klassen wächst.

## Beziehung zu anderen Mustern

- **Adapter** bietet eine **andere** Schnittstelle; **Proxy** – **dieselbe**; **Decorator** – dieselbe, aber erweiterte.
- **Decorator** und **Proxy** sind fast gleich aufgebaut (Hülle + Delegation), der Unterschied liegt in der Absicht: Der Dekorierer **ergänzt** Verhalten, und seine Zusammensetzung bestimmt der Client; der Stellvertreter **kontrolliert den Zugriff** und verwaltet meist selbst den Lebenszyklus des Objekts (wie `LazyImageProxy`, der `HighResolutionImage` selbst erzeugt).
- **Facade** verbirgt ebenfalls ein Objekt hinter sich, aber eine Fassade hat ihre eigene vereinfachte Schnittstelle, ein Stellvertreter dagegen die Schnittstelle des Originals.
