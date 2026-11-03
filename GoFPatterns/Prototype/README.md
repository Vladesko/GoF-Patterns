🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Prototype) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Prototype) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Prototype) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Prototype)

# Prototype (Prototyp)

**Typ:** Erzeugungsmuster.

**Prototype** ermöglicht es, neue Objekte durch Kopieren bereits vorhandener Objekte (Prototypen) zu erzeugen, ohne von deren konkreten Klassen abhängig zu sein.

---

## Welches Problem wird gelöst

Man braucht eine Kopie eines Objekts. Der naive Weg: ein Objekt derselben Klasse erzeugen und alle Felder hinüberkopieren. Die Probleme:

- manche Felder können **privat** und von außen nicht zugänglich sein;
- der Client muss die **konkrete Klasse** des Objekts kennen, hat aber oft nur eine Schnittstelle (`IDocumentPrototype`, `Unit`);
- das Objekt kann **aufwendig zu konfigurieren** sein (lange Initialisierung, Laden der Konfiguration), und das für jede Kopie zu wiederholen ist teuer.

## Lösung

Das Kopieren wird dem Objekt selbst übertragen. Alle kopierbaren Objekte implementieren eine gemeinsame Schnittstelle mit der Methode `Clone()`. Das Objekt kennt seine Felder (auch die privaten) und entscheidet selbst, wie sie kopiert werden.

Oft gibt es zusätzlich eine **Prototyp-Registry** – einen Speicher vorkonfigurierter Objekte, aus dem der Client Kopien per Schlüssel bezieht.

### Flache und tiefe Kopie

- **Shallow Copy** (`MemberwiseClone()`) kopiert die Felder „wie sie sind“: Werttypen werden kopiert, Referenzfelder von Kopie und Original zeigen dagegen auf **dieselben** Objekte.
- **Deep Copy** kopiert auch die verschachtelten Objekte, die Kopie ist völlig unabhängig.

Bei unveränderlichen Objekten (`string`, `record` mit init-Eigenschaften) spielt der Unterschied keine Rolle – sie können gefahrlos geteilt werden.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Prototype** | `IDocumentPrototype`, `Unit` | Deklariert die Klonmethode `Clone()` |
| **ConcretePrototype** | `Contract`, `Invoice`, `Archer`, `Mage` | Implementiert `Clone()` und kopiert dabei seinen Zustand |
| **Prototype Registry** | `DocumentTemplateRegistry` | Speichert konfigurierte Prototypen und liefert Kopien per Schlüssel (optionaler Beteiligter) |
| **Client** | `Spawner`, `DemonstratePrototype` | Erzeugt neue Objekte, indem er `Clone()` am Prototyp aufruft |

## Schema

```text
 Client ───────────► «interface» IDocumentPrototype
                        Clone(): IDocumentPrototype
                              ▲              ▲
                              │              │
                          Contract        Invoice
                     Clone() => new Contract(this)

 DocumentTemplateRegistry
   _templates: Schlüssel ─► Prototyp
   Create(key) => _templates[key].Clone()
```

---

## Beispiel 1. Dokumentvorlagen (`Documents/`)

**Aufgabe:** Ein Dokumentenmanagementsystem hat Vorlagen (NDA, EU-Rechnung). Ein neues Dokument entsteht durch Kopieren einer Vorlage, danach werden die konkreten Daten eingetragen.

**Prototype** – `IDocumentPrototype` mit der Methode `Clone()`.

**ConcretePrototype** – `Contract` enthält die Referenzfelder `DocumentStyle Style` und `List<string> Sections`. Die tiefe Kopie ist über einen **privaten Kopierkonstruktor** umgesetzt – so hat die Klasse Zugriff auf alle ihre Felder:

```csharp
private Contract(Contract source)
{
    Title = source.Title;
    Company = source.Company;
    Style = source.Style.Copy();       // Kopie des verschachtelten Objekts
    Sections = [.. source.Sections];   // Kopie der Liste
}

public IDocumentPrototype Clone() => new Contract(this);
```

Zum Vergleich gibt es `ShallowClone()` über `MemberwiseClone()`. In der Demo sieht man: Nach der Änderung von `shallow.Style.FontSize` und `shallow.Sections` **ändert sich auch das Original**, weil beide dasselbe Stilobjekt und dieselbe Liste teilen. Änderungen an der tiefen Kopie betreffen das Original nicht.

`Invoice` ist der zweite Prototyp, mit eigenen Feldern und eigener Implementierung von `Clone()`.

**Registry** – `DocumentTemplateRegistry` speichert die Vorlagen per Schlüssel und gibt in `Create(key)` immer eine **Kopie** zurück, daher wird die Vorlage selbst nie verändert:

```csharp
var ndaForAcme = (Contract)registry.Create("nda");
ndaForAcme.Company = "Acme";
```

## Beispiel 2. Spawner für Spieleinheiten (`GameUnits/`)

**Aufgabe:** Ein Spiel soll viele gleichartige Einheiten erzeugen („Elite-Bogenschütze“, „Feuermagier“), deren Einstellungen aus der Konfiguration geladen werden.

**Prototype** – die abstrakte Klasse `Unit` mit `abstract Unit Clone()`.

**ConcretePrototype** – `Archer` und `Mage` (Ordner `Units/`) kopieren ihre spezifischen Felder; `Mage` kopiert die Liste der Zauber, damit sich die Klone diese nicht teilen.

**Client** – `Spawner` erhält **irgendeine** `Unit` und erzeugt neue Instanzen, ohne zu wissen, ob es ein Bogenschütze oder ein Magier ist:

```csharp
public Unit Spawn(Position position)
{
    var unit = prototype.Clone();
    unit.Name = $"{prototype.Name} #{++_counter}";
    unit.Position = position;
    return unit;
}
```

`Position` ist ein `record`. Er ist unveränderlich, daher muss er nicht kopiert werden, sondern kann von den Klonen geteilt werden. Außerdem zeigt die Demo den in C# eingebauten Mechanismus zum Klonen von Records – den Ausdruck `with`: `start with { X = 7 }`.

---

## Wann einsetzen

- Der Code soll nicht von den konkreten Klassen der kopierten Objekte abhängen.
- Die Erzeugung eines Objekts ist teuer, man braucht aber viele ähnliche Instanzen.
- Es gibt eine Reihe typischer Konfigurationen, und statt vieler Unterklassen ist es bequemer, konfigurierte Prototypen vorzuhalten.

## Wann nicht nötig

- Die Objekte sind einfach und billig – ein Konstruktoraufruf ist einfacher.
- Die Objekte sind unveränderlich – sie können ohne Kopie geteilt werden.

## Vor- und Nachteile

**Vorteile**
- Klonen ohne Bindung an konkrete Klassen.
- Weniger wiederholter Initialisierungscode.
- Eine Alternative zur Vererbung, um „Variationen“ eines Objekts zu erhalten.

**Nachteile**
- Objekte mit komplexen Graphen (zyklische Referenzen, Ressourcen wie Verbindungen und Dateien) sind schwer zu klonen.
- Man muss genau auf flache bzw. tiefe Kopie achten.

## Beziehung zu anderen Mustern

- **Abstract Factory** kann eine Reihe von Prototypen speichern und diese klonen, statt Objekte mit `new` zu erzeugen.
- **Memento** – bei einfachen Objekten kann der Zustandsschnappschuss durch Klonen erstellt werden.
- **Composite** und **Decorator** – Klonen hilft, die komplexen Strukturen dieser Muster zu kopieren.
- Die Prototyp-Registry wird oft als **Singleton** umgesetzt.

> In .NET gibt es die Schnittstelle `ICloneable`, ihre Verwendung wird aber nicht empfohlen: Sie gibt `object` zurück und sagt nicht, ob die Kopie tief oder flach ist. Besser ist eine eigene typisierte Schnittstelle wie `IDocumentPrototype`.
