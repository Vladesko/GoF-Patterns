🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Decorator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Decorator) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Decorator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Decorator)

# Decorator (Dekorierer)

**Typ:** Strukturmuster.

**Decorator** ermöglicht es, einem Objekt dynamisch neues Verhalten hinzuzufügen, indem man es in Hüllobjekte mit derselben Schnittstelle „einwickelt“. Die Hüllen lassen sich in beliebiger Reihenfolge und Anzahl ineinander verschachteln.

---

## Welches Problem wird gelöst

Ein Café verkauft Getränke, zu denen man Milch, Sirup, eine größere Portion ... hinzufügen kann. Löst man das über Vererbung:

```text
Espresso
 ├── EspressoWithMilk
 ├── EspressoWithSyrup
 ├── EspressoWithMilkAndSyrup
 ├── LargeEspressoWithMilkAndSyrup
 └── ...  (und alles noch einmal für Tee)
```

Die Zahl der Unterklassen wächst kombinatorisch. Außerdem ist Vererbung **statisch**: Man kann einem bereits erzeugten Objekt zur Laufzeit keine „Milch hinzufügen“. Und doppelte Milch lässt sich ohne eine weitere Klasse gar nicht ausdrücken.

Dasselbe beim Datenspeicher: Mal braucht man Komprimierung, mal Verschlüsselung, mal beides, mal zusätzlich Logging – für jede Kombination eine Unterklasse anzulegen ist nicht machbar.

## Lösung

Vererbung wird durch **Komposition** ersetzt. Der Dekorierer:
- implementiert **dieselbe Schnittstelle** wie das umhüllte Objekt;
- hält eine Referenz auf das umhüllte Objekt;
- delegiert die Aufrufe an dieses und ergänzt sein eigenes Verhalten **davor oder danach**.

Da der Dekorierer für den Client wie eine gewöhnliche Komponente aussieht, kann man ihn in einen weiteren Dekorierer einwickeln – es entsteht eine „Matroschka“. Der Client weiß nicht, ob er mit dem ursprünglichen Objekt oder mit einer Kette von Hüllen arbeitet.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Component** | `IBeverage`, `IDataSource` | Gemeinsame Schnittstelle für Hüllen und umhüllte Objekte |
| **ConcreteComponent** | `Espresso`, `BlackTea`, `InMemoryDataSource` | Basisobjekt, dem Verhalten hinzugefügt wird |
| **BaseDecorator** | `BeverageDecorator`, `DataSourceDecorator` | Hält eine Referenz auf Component und delegiert die Aufrufe standardmäßig einfach an sie |
| **ConcreteDecorator** | `MilkDecorator`, `SyrupDecorator`, `LargeSizeDecorator`, `CompressionDecorator`, `EncryptionDecorator`, `LoggingDecorator` | Ergänzen konkretes Verhalten vor/nach dem Aufruf des umhüllten Objekts |
| **Client** | Demo-Klassen | Stellt die gewünschte Kombination von Hüllen zusammen und arbeitet mit ihr über Component |

## Schema

```text
            ┌───────────────────────┐
 Client ───►│ «interface» IBeverage │◄─────────────────┐
            │ GetDescription()      │                  │ umhüllt
            │ GetCost()             │                  │
            └───────────────────────┘                  │
               ▲                 ▲                     │
               │                 │                     │
          Espresso    ┌──────────────────────────┐     │
          BlackTea    │ «abstract» BeverageDecorator│◆──┘
                      │ - Beverage: IBeverage    │
                      └──────────────────────────┘
                         ▲          ▲           ▲
                   MilkDecorator  SyrupDecorator  LargeSizeDecorator

  new LargeSize( new Syrup( new Milk( new Espresso() ) ) )
       │             │          │          └─ 2.00
       │             │          └─ +0.50
       │             └─ +0.70
       └─ ×1.5
```

---

## Beispiel 1. Café (`Coffee/`)

**Component** – `IBeverage` mit den Methoden `GetDescription()` und `GetCost()`.

**ConcreteComponent** – `Espresso` und `BlackTea` (Ordner `Beverages/`) kennen nur ihren Grundpreis.

**BaseDecorator** – `BeverageDecorator` (Ordner `AddOns/`) hält das umhüllte Getränk und delegiert alles über `virtual`-Methoden an dieses:

```csharp
public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
{
    protected IBeverage Beverage { get; } = beverage;

    public virtual string GetDescription() => Beverage.GetDescription();
    public virtual decimal GetCost() => Beverage.GetCost();
}
```

**ConcreteDecorator** – jedes Extra überschreibt die Methoden, ruft `base` auf und ergänzt das Ergebnis:

```csharp
public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
{
    public override string GetDescription() => $"{base.GetDescription()} + Milch";
    public override decimal GetCost() => base.GetCost() + 0.50m;
}
```

- `SyrupDecorator` zeigt, dass ein Dekorierer **eigenen Zustand** haben kann (die Sirupsorte).
- `LargeSizeDecorator` addiert nicht, sondern **multipliziert** den Preis. Deshalb sieht man in der Demo, dass **die Reihenfolge der Hüllen wichtig ist**: „Groß (Espresso + Sirup)“ ist teurer als „Großer Espresso + Sirup“.

**Client** – baut die Getränke als „Matroschka“: `new MilkDecorator(new MilkDecorator(new Espresso()))` – doppelte Milch ohne eigene Klasse. Dieselben Extras funktionieren unverändert auch mit Tee.

## Beispiel 2. Datenquelle (`DataStream/`)

**Aufgabe:** Daten (z. B. Kundendaten mit Kartennummer) werden in einem Speicher abgelegt. Je nach Einstellungen müssen sie komprimiert, verschlüsselt und protokolliert werden – in beliebiger Kombination.

**Component** – `IDataSource` mit `Write`/`Read`.

**ConcreteComponent** – `InMemoryDataSource` speichert die Zeichenkette unverändert (Nachbildung einer Datei oder eines Datenbankeintrags).

**BaseDecorator** – `DataSourceDecorator` (Ordner `Decorators/`).

**ConcreteDecorator** – hier sieht man das Verhalten „davor“ und „danach“ gut:

```csharp
// CompressionDecorator
public override void Write(string data) => base.Write(Compress(data));   // VOR dem Schreiben – komprimieren
public override string Read() => Decompress(base.Read());                // NACH dem Lesen – entpacken
```

- `CompressionDecorator` – GZip + Base64.
- `EncryptionDecorator` – XOR mit Schlüssel (nur zur Demonstration, das ist keine echte Verschlüsselung!).
- `LoggingDecorator` – ändert die Daten nicht, schreibt nur die Größe in die Konsole.

**Client** – die Demo umhüllt den Speicher:

```csharp
IDataSource source = new LoggingDecorator(
    new CompressionDecorator(
        new EncryptionDecorator(protectedStorage, "secret")),
    "Client");
```

Beim Schreiben durchlaufen die Daten die Hüllen von außen nach innen (Log → Komprimierung → Verschlüsselung → Speicher), beim Lesen von innen nach außen. Im Speicher liegt eine komprimierte, verschlüsselte Zeichenkette, und der Client liest den ursprünglichen Text, ohne überhaupt davon zu wissen.

Genau so sind Streams in .NET aufgebaut: `new GZipStream(new CryptoStream(new FileStream(...)))` ist ein klassischer Decorator. Dekorierer sind auch `BufferedStream`, `StreamReader` (über einem `Stream`) und in ASP.NET Core die Middleware-Pipeline sowie das Dekorieren von Diensten im DI-Container (z. B. mit Scrutor).

---

## Wann einsetzen

- Objekten sollen **dynamisch** und für den Client unbemerkt Zuständigkeiten hinzugefügt werden.
- Erweiterung durch Vererbung ist unmöglich (Klasse ist `sealed`) oder führt zu einer Explosion der Unterklassen.
- Unabhängige „Aufsätze“ sollen kombiniert werden: Caching, Logging, Wiederholungsversuche, Validierung, Metriken.

## Wann nicht nötig

- Man braucht nur eine feste Kombination von Verhalten – dann ist eine eigene Klasse einfacher.
- Das Verhalten soll davon abhängen, welche Hüllen bereits vorhanden sind (Dekorierer sollten nichts voneinander wissen).

## Vor- und Nachteile

**Vorteile**
- Flexibler als Vererbung: Verhalten wird zur Laufzeit hinzugefügt und kombiniert.
- Jeder Dekorierer ist für eine Sache zuständig (SRP).
- Kann mehrfach angewendet werden (doppelte Milch).
- `sealed`-Klassen und Fremdcode lassen sich über ihre Schnittstelle erweitern.

**Nachteile**
- Viele kleine Klassen.
- Eine bestimmte Hülle aus der Mitte der Kette zu entfernen ist schwierig.
- Das Verhalten hängt von der Reihenfolge der Hüllen ab – das übersieht man leicht.
- Der Konfigurationscode der „Matroschka“ kann unhandlich werden.

## Beziehung zu anderen Mustern

- **Adapter** ändert die Schnittstelle eines Objekts; **Decorator** behält die Schnittstelle bei und erweitert das Verhalten; **Proxy** behält die Schnittstelle bei und **kontrolliert den Zugriff** (meist verwaltet er den Lebenszyklus des Objekts selbst, während der Dekorierer das Objekt vom Client erhält).
- **Composite** ist strukturell ähnlich (rekursive Komposition), aber ein Dekorierer hat nur ein umhülltes Objekt und das Ziel, Verhalten hinzuzufügen.
- **Chain of Responsibility** reicht eine Anfrage ebenfalls entlang einer Kette weiter, aber ihre Glieder können die Verarbeitung **abbrechen**; ein Dekorierer gibt den Aufruf immer weiter.
- **Strategy** ändert das „Innere“ eines Objekts, **Decorator** seine „Hülle“.
