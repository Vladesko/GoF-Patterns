🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Flyweight) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Flyweight) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Flyweight) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Flyweight)

# Flyweight (Fliegengewicht)

**Typ:** Strukturmuster.

**Flyweight** ermöglicht es, eine große Anzahl von Objekten im Speicher unterzubringen, indem **gemeinsamer Zustand** zwischen ihnen **geteilt** wird, statt dieselben Daten in jedem Objekt zu speichern.

---

## Welches Problem wird gelöst

In einem Spiel soll ein Wald aus 100 000 Bäumen gezeichnet werden. Jeder Baum ist ein Objekt mit Koordinaten, Baumart, Farbe und **Textur** (sagen wir 1 MB). Speichert jeder Baum seine eigene Kopie der Textur, ergibt das ~100 GB Speicher, obwohl es nur vier verschiedene Baumarten gibt.

Dasselbe in einem Texteditor: Jedes Zeichen hat Schriftart, Größe, Farbe und Fettdruck. Tausende Zeichen, aber nur eine Handvoll eindeutiger Stile.

Das Problem: **Der Großteil der Daten wiederholt sich** in vielen Objekten.

## Lösung

Der Zustand eines Objekts wird in zwei Teile aufgeteilt:

- **Intrinsisch** – für viele Objekte gleich und unabhängig vom Kontext: Baumart, Farbe, Textur; Schriftart, Größe. Er wird in ein **Flyweight**-Objekt ausgelagert, das **einmal** erzeugt und von allen geteilt wird.
- **Extrinsisch** – für jedes Objekt eindeutig: die Koordinaten des Baums, das Zeichen und seine Position im Text. Ihn hält der **Kontext** (oder der Client) und übergibt ihn dem Flyweight als Methodenparameter.

Flyweights müssen **unveränderlich** sein – schließlich wird ein Objekt von Tausenden Kontexten genutzt. Damit keine Duplikate entstehen, werden Flyweights von einer **Fabrik** mit Pool (Cache) ausgegeben.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Flyweight** | `TreeType`, `CharacterStyle` | Hält den intrinsischen (geteilten) Zustand. Unveränderlich. Den extrinsischen Zustand erhält es über Parameter |
| **FlyweightFactory** | `TreeTypeFactory`, `StyleFactory` | Pool von Flyweights: liefert ein vorhandenes oder erzeugt ein neues |
| **Context** | `Tree`, `FormattedCharacter` | Hält den extrinsischen Zustand + eine Referenz auf das Flyweight. Von diesen Objekten gibt es viele, aber sie sind klein |
| **Client** | `ForestMap`, `TextDocument` | Erzeugt Kontexte und bezieht Flyweights ausschließlich über die Fabrik |

## Schema

```text
 ForestMap (Client)
   │ PlantTree(x, y, "Eiche", ...)
   ▼
 TreeTypeFactory ── _types: "Eiche|dunkelgrün|oak.png" ─► TreeType ◄──┐
                                                         (1 MB)        │
                                                                       │ geteilt
   Tree(x=10, y=20) ───────────────────────────────────────────────────┤
   Tree(x=55, y=13) ───────────────────────────────────────────────────┤
   Tree(x=91, y=77) ───────────────────────────────────────────────────┘
   ... 100 000 kleine Kontexte, 4 Flyweights
```

---

## Beispiel 1. Wald (`Forest/`)

**Flyweight** – `TreeType`. Hält Name, Farbe, Texturpfad und die „schweren“ Texturdaten selbst (ein Array von 1 MB). Die Klasse ist `sealed`, alle Eigenschaften sind schreibgeschützt. Koordinaten werden darin **nicht gespeichert** – sie werden an die Methode übergeben:

```csharp
public string Draw(int x, int y) => $"{Name} ({Color}) bei [{x}, {y}] mit {TexturePath}";
```

**FlyweightFactory** – `TreeTypeFactory` hält ein Dictionary `Schlüssel → TreeType`. Ein neues Objekt wird nur erzeugt, wenn es noch keines gibt (in der Demo sieht man, dass die Meldung „neuer Baumtyp erstellt“ bei 100 000 Bäumen genau 4-mal ausgegeben wird):

```csharp
if (!_types.TryGetValue(key, out var type))
{
    type = new TreeType(name, color, texturePath);
    _types[key] = type;
}
return type;
```

**Context** – `Tree`: zwei `int` und eine Referenz auf `TreeType`. Die Methode `Draw()` übergibt ihre Koordinaten an das Flyweight.

**Client** – `ForestMap` pflanzt Bäume über die Fabrik und erzeugt `TreeType` nie selbst.

Die Demo schätzt den Speicherbedarf: ohne Flyweight ~100 GB, mit Flyweight ~5,5 MB (4 Texturen + 100 000 kleine Kontexte).

## Beispiel 2. Textformatierung (`TextFormatting/`)

**Flyweight** – `CharacterStyle` ist ein `sealed record` (Schriftart, Größe, Fettdruck, Farbe). Ein Record ist unveränderlich und wird nach Wert verglichen – daher eignet er sich gut als Dictionary-Schlüssel.

**FlyweightFactory** – `StyleFactory` hält einen Pool `Dictionary<CharacterStyle, CharacterStyle>`: Gibt es bereits einen wertgleichen Stil, wird **dasselbe Objekt** zurückgegeben, andernfalls wird der Kandidat in den Pool aufgenommen.

**Context** – `FormattedCharacter` ist ein `readonly record struct` aus Zeichen, Position und Referenz auf den Stil.

**Client** – `TextDocument.Append(...)` bezieht den Stil von der Fabrik und erzeugt für jedes Zeichen einen Kontext. Die Demo zeigt, dass auf 158 Zeichen nur 3 Stilobjekte kommen, und `CountDistinctStyleObjects()` prüft das per Vergleich **nach Referenz** (`ReferenceEqualityComparer`).

Am Ende wird das in .NET eingebaute Flyweight gezeigt – **String-Interning**: `string.Intern` liefert für gleiche Zeichenketten ein und dasselbe Objekt. String-Literale werden in C# automatisch interniert.

---

## Wann einsetzen

- Das Programm hat **sehr viele** gleichartige Objekte, die nicht in den Speicher passen.
- Der Großteil des Zustands dieser Objekte lässt sich auslagern oder teilen.
- Die Objekte können unveränderlich gemacht werden.
- Beispiele: Partikel und Kacheln in Spielen, Zeichen in Editoren, Markierungen auf einer Karte, Icon-Cache.

## Wann nicht nötig

- Es gibt wenige Objekte – die Ersparnis lohnt den zusätzlichen Aufwand nicht.
- Der Zustand der Objekte ist fast vollständig eindeutig – es gibt nichts zu teilen.

## Vor- und Nachteile

**Vorteile**
- Erhebliche Speicherersparnis.

**Nachteile**
- Kostet Rechenzeit für die Suche im Pool und die Übergabe des extrinsischen Zustands.
- Der Code wird komplizierter: Der Zustand ist zwischen Kontext und Flyweight „verteilt“.
- Flyweights müssen unveränderlich sein – sonst wirkt sich eine Änderung auf alle Kontexte aus.

## Beziehung zu anderen Mustern

- **Composite** – die Blätter eines Baums werden oft als Flyweights umgesetzt (z. B. Zeichen in einem Dokument).
- **Singleton** ähnelt ihm darin, dass ein Objekt geteilt wird, aber ein Singleton ist eine Instanz pro Klasse, während es viele Flyweights gibt (je eines pro eindeutigem intrinsischem Zustand), und diese sind unveränderlich.
- **Factory Method** / Objektpool – die Flyweight-Fabrik ist im Grunde ein Cache.
- **State** und **Strategy** – Zustands- und Strategieobjekte ohne eigene Felder werden oft als Flyweights umgesetzt.
