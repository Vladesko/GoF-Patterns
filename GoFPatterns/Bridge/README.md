🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Bridge) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Bridge) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Bridge) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Bridge)

# Bridge (Brücke)

**Typ:** Strukturmuster.

**Bridge** teilt eine große Klasse (oder Hierarchie) in zwei unabhängige Hierarchien – **Abstraktion** und **Implementierung** –, die sich getrennt voneinander weiterentwickeln lassen. Die Verbindung zwischen ihnen ist eine Referenz (die „Brücke“) von der Abstraktion auf die Implementierung.

---

## Welches Problem wird gelöst

Es gibt Fernbedienungen (einfach, erweitert) und Geräte (Fernseher, Radio). Löst man die Aufgabe mit Vererbung, entsteht eine Klasse pro Kombination:

```text
Remote
 ├── TvRemote
 ├── RadioRemote
 ├── AdvancedTvRemote
 └── AdvancedRadioRemote
```

Kommt ein Projektor hinzu – zwei weitere Klassen. Kommt eine „Sprachfernbedienung“ hinzu – je eine weitere pro Gerät. Die Zahl der Klassen wächst als **Produkt** (N Fernbedienungen × M Geräte), und Code wird dupliziert.

Der Grund: Die Klasse versucht, sich gleichzeitig **in zwei unabhängige Richtungen** zu erweitern (Art der Fernbedienung und Art des Geräts).

## Lösung

Vererbung wird durch **Komposition** ersetzt. Eine Richtung („was wir tun“ – Fernbedienung/Bericht) wird zur Hierarchie der **Abstraktion**, die andere („wie wir es tun“ – Gerät/Format) zur Hierarchie der **Implementierung**. Die Abstraktion hält eine Referenz auf das Implementierungsobjekt und delegiert die Arbeit daran.

Jetzt gibt es N + M Klassen, und jede Fernbedienung funktioniert mit jedem Gerät.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Abstraction** | `Remote`, `Report` | High-Level-Steuerlogik. Hält eine Referenz auf den Implementor und delegiert die Arbeit an ihn |
| **RefinedAbstraction** | `AdvancedRemote`, `SalesReport`, `InventoryReport` | Erweitert die Abstraktion, ohne die Implementierungen anzufassen |
| **Implementor** | `IDevice`, `IReportRenderer` | Schnittstelle der Implementierung – eine Reihe primitiver Operationen. Muss nicht mit der Schnittstelle der Abstraktion übereinstimmen |
| **ConcreteImplementor** | `Tv`, `Radio`, `PlainTextRenderer`, `HtmlRenderer` | Plattformabhängige Implementierungen der Primitive |
| **Client** | Demo-Klassen | Verbindet die Abstraktion mit der passenden Implementierung |

## Schema

```text
        Abstraktion                         Implementor
 ┌──────────────────────┐   Brücke   ┌────────────────────────┐
 │ Remote               │◆─────────► │ «interface» IDevice    │
 │ - Device: IDevice    │            │ Enable(), Disable()    │
 │ + TogglePower()      │            │ SetVolume(), SetChannel│
 │ + VolumeUp()         │            └────────────────────────┘
 └──────────────────────┘                  ▲          ▲
            ▲                              │          │
 ┌──────────────────────┐                 Tv        Radio
 │ AdvancedRemote       │
 │ + Mute(), Unmute()   │
 └──────────────────────┘
```

---

## Beispiel 1. Fernbedienungen und Geräte (`RemoteControl/`)

**Implementor** – `IDevice` beschreibt die **primitiven** Operationen eines Geräts: einschalten, ausschalten, Lautstärke und Kanal setzen.

**ConcreteImplementor** – `Tv` und `Radio` (Ordner `Devices/`) implementieren sie unterschiedlich: Beim Fernseher reicht die Lautstärke von 0–100, beim Radio von 0–20, und die Kanäle sind eine im Kreis durchlaufene Liste von Sendern.

**Abstraction** – `Remote` (Ordner `Remotes/`) setzt **High-Level**-Aktionen über die Primitive des Geräts um:

```csharp
public class Remote(IDevice device)
{
    protected readonly IDevice Device = device;   // das ist die „Brücke“

    public void VolumeUp() => Device.SetVolume(Device.Volume + 5);
    public void ChannelUp() => Device.SetChannel(Device.Channel + 1);
}
```

**RefinedAbstraction** – `AdvancedRemote` ergänzt `Mute`, `Unmute`, `SetFavoriteChannel`. Weder `Tv` noch `Radio` mussten dafür geändert werden: „Stumm“ ist einfach `SetVolume(0)` mit gemerkter vorheriger Lautstärke.

**Client** – die Demo baut die Kombinationen `Remote + Tv`, `AdvancedRemote + Radio`, `AdvancedRemote + Tv` – 4 mögliche Kombinationen aus 4 Klassen statt aus 4 eigenen Kombinationsklassen plus Basis.

## Beispiel 2. Berichte und Formate (`Reports/`)

**Aufgabe:** Es gibt Berichtstypen (Umsatz, Lager) und Ausgabeformate (reiner Text, HTML). Morgen kommen vielleicht ein Kundenbericht und das Format PDF hinzu.

**Implementor** – `IReportRenderer` kann Primitive darstellen: Überschrift, Zeile „Name – Wert“, Fußzeile.

**ConcreteImplementor** – `PlainTextRenderer` (spaltenweise Ausrichtung) und `HtmlRenderer` (Tags) im Ordner `Renderers/`.

**Abstraction** – die abstrakte Klasse `Report` hält einen `IReportRenderer` und deklariert `Build()`.

**RefinedAbstraction** – `SalesReport` und `InventoryReport` (Ordner `ReportTypes/`) wissen, **was** ausgegeben wird: Der Umsatzbericht berechnet die Summe, der Lagerbericht markiert knapp werdende Artikel. **Wie** das aussieht, entscheidet der Renderer:

```csharp
var lines = new List<string> { Renderer.RenderTitle("Lagerbestand") };
foreach (var (item, count) in stock)
{
    var value = count < LowStockLimit ? $"{count} (knapp!)" : count.ToString();
    lines.Add(Renderer.RenderRow(item, value));
}
```

**Client** – die Demo durchläuft alle Renderer und alle Berichte; die Implementierung kann zur Laufzeit gewählt werden (z. B. anhand des `Accept`-Headers einer Anfrage).

---

## Wann einsetzen

- Eine Klasse soll in zwei (oder mehr) unabhängige Richtungen erweitert werden.
- Die Implementierung soll zur Laufzeit austauschbar sein.
- Eine „monolithische“ Klasse mit mehreren Varianten derselben Funktionalität soll aufgeteilt werden (z. B. Arbeit mit verschiedenen Datenbanken/Plattformen).
- Plattformübergreifender Code: Abstraktion – GUI/Logik, Implementierung – API des konkreten Betriebssystems.

## Wann nicht nötig

- Es gibt nur eine Implementierung, und eine weitere ist nicht absehbar – dann ist es eine überflüssige Abstraktionsebene.

## Vor- und Nachteile

**Vorteile**
- Die Zahl der Klassen wächst als Summe, nicht als Produkt.
- Abstraktion und Implementierung entwickeln sich unabhängig (Open/Closed).
- Der Client arbeitet nur mit der High-Level-Abstraktion und sieht keine Plattformdetails.
- Die Implementierung lässt sich zur Laufzeit austauschen.

**Nachteile**
- Macht den Code komplizierter, wenn es in Wahrheit nur eine Hierarchie gibt.

## Beziehung zu anderen Mustern

- **Adapter** wird auf bereits vorhandene, inkompatible Klassen angewendet, damit sie zusammenarbeiten. **Bridge** wird von vornherein entworfen, damit sich die Teile unabhängig entwickeln.
- **Strategy** ist ähnlich aufgebaut (ein Objekt delegiert Arbeit über eine Schnittstelle an ein anderes), löst aber eine andere Aufgabe: einen Algorithmus austauschen. Bei Bridge ist auch die Abstraktion selbst eine Hierarchie.
- **Abstract Factory** kann zueinander passende Paare „Abstraktion + Implementierung“ erzeugen.
- **Builder** + **Bridge**: Der Director spielt die Rolle der Abstraktion, die Builder die der Implementierungen.
