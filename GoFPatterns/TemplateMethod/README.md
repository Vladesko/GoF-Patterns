🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/TemplateMethod) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/TemplateMethod) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/TemplateMethod) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/TemplateMethod)

# Template Method (Schablonenmethode)

**Typ:** Verhaltensmuster.

**Template Method** definiert das **Gerüst eines Algorithmus** in einer Methode der Basisklasse und überlässt die Implementierung einzelner Schritte den Unterklassen. Unterklassen können Schritte überschreiben, aber **die Struktur** des Algorithmus **nicht ändern**.

---

## Welches Problem wird gelöst

Kunden sollen aus CSV und aus JSON importiert werden. Der Ablauf ist in beiden Fällen gleich: Daten vorbereiten → einlesen → prüfen → speichern → (nach dem Import etwas tun). Nur der Schritt des **Einlesens** unterscheidet sich.

Schreibt man zwei unabhängige Klassen `CsvImporter` und `JsonImporter`:
- werden Prüfung und Speicherung **dupliziert**;
- korrigiert man einen Validierungsfehler in einer Klasse, vergisst man ihn in der anderen;
- kommt XML hinzu, muss man alles ein drittes Mal kopieren;
- garantiert niemand, dass alle Importer die Schritte in derselben Reihenfolge ausführen.

## Lösung

Der gemeinsame Algorithmus wird in eine **Basisklasse** ausgelagert, und zwar als Methode (die Schablonenmethode), die die Schritte der Reihe nach aufruft. Es gibt drei Arten von Schritten:

- **abstrakte** – jede Unterklasse muss sie implementieren (`Parse`);
- **gemeinsame** – in der Basisklasse implementiert und für alle gleich (`Save`);
- **Hooks** – `virtual`-Methoden mit Standardimplementierung (oft leer), die eine Unterklasse überschreiben **kann**, um sich in den Algorithmus „einzuklinken“ (`Preprocess`, `AfterImport`).

Die Schablonenmethode selbst ist **nicht virtuell** – Unterklassen können die Reihenfolge der Schritte nicht ändern.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **AbstractClass** | `CustomerImporter`, `HotBeverage` | Enthält die Schablonenmethode (`Import`, `Prepare`) und deklariert die Schritte: abstrakte, gemeinsame und Hooks |
| **ConcreteClass** | `CsvCustomerImporter`, `JsonCustomerImporter`, `PartnerCsvCustomerImporter`, `Tea`, `GreenTea`, `Coffee` | Implementiert die abstrakten Schritte und überschreibt bei Bedarf Hooks. Den Algorithmus selbst fasst sie nicht an |

## Schema

```text
 ┌──────────────────────────────────────────┐
 │ «abstract» CustomerImporter              │
 │ + Import(raw)       ← Schablonenmethode  │
 │     Preprocess()       ← Hook (virtual)  │
 │     Parse()            ← abstract        │
 │     Validate()         ← virtual         │
 │     Save()             ← gemeinsam (priv)│
 │     AfterImport()      ← Hook (leer)     │
 └──────────────────────────────────────────┘
          ▲                         ▲
 ┌──────────────────────┐  ┌──────────────────────┐
 │ CsvCustomerImporter  │  │ JsonCustomerImporter │
 │ Preprocess(): Kopf-  │  │ Parse(): JSON        │
 │   zeile überspringen │  │ AfterImport(): E-Mail│
 │ Parse(): CSV         │  └──────────────────────┘
 └──────────────────────┘
          ▲
 ┌────────────────────────────┐
 │ PartnerCsvCustomerImporter │
 │ Validate(): strengere Regeln│
 └────────────────────────────┘
```

---

## Beispiel 1. Kundenimport (`DataImport/`)

**AbstractClass** – `CustomerImporter`. Die Schablonenmethode `Import`:

```csharp
public IReadOnlyList<Customer> Import(string rawData)
{
    var content = Preprocess(rawData);          // Hook
    var records = Parse(content);               // abstrakt
    var valid = records.Where(Validate).ToList();   // virtuell
    Save(valid);                                // gemeinsamer Schritt
    AfterImport(valid);                         // Hook
    return valid;
}
```

- `Parse` – abstrakt, jedes Format hat sein eigenes.
- `Validate` – die gemeinsame Prüfung (E-Mail enthält `@`, Alter in vernünftigen Grenzen), die sich aber verschärfen lässt.
- `Save` – `private`: Diesen Schritt kann eine Unterklasse weder überschreiben noch einzeln aufrufen.
- `Preprocess` und `AfterImport` – Hooks.

**ConcreteClass** (Ordner `Importers/`):
- `CsvCustomerImporter` – zerlegt die Zeilen anhand von `;` und überschreibt den Hook `Preprocess`, um die Kopfzeile abzuschneiden;
- `JsonCustomerImporter` – liest JSON mit `System.Text.Json` ein und überschreibt den Hook `AfterImport` (Versand von Begrüßungs-E-Mails);
- `PartnerCsvCustomerImporter` – erbt vom **CSV-Importer** und verschärft nur `Validate` (Volljährige mit Firmen-E-Mail), wobei es `base.Validate` aufruft. Das Einlesen von CSV und den gesamten Algorithmus bekommt es geschenkt.

**Client** – die Demo arbeitet mit allen Importern über die Basisklasse und ruft dasselbe `Import` auf. An der Ausgabe sieht man: Die Schritte sind gleich, nur die Details unterscheiden sich.

## Beispiel 2. Heißgetränke (`Beverages/`) – das klassische Beispiel mit Hooks

**AbstractClass** – `HotBeverage` mit der Schablonenmethode `Prepare()`:

```csharp
public void Prepare()
{
    BoilWater();
    Brew();
    PourInCup();
    if (CustomerWantsCondiments())
        AddCondiments();
}
```

- `BoilWater`, `PourInCup` – gemeinsame Schritte;
- `Brew`, `AddCondiments` – abstrakt;
- `WaterTemperature` und `CustomerWantsCondiments()` – **Hooks**: standardmäßig 100 °C und „ja, Zutaten erwünscht“.

**ConcreteClass** (Ordner `Drinks/`):
- `Tea` – nur die Pflichtschritte;
- `GreenTea` – überschreibt die Hooks: Wasser mit 80 °C und keine Zutaten;
- `Coffee` – der Hook `CustomerWantsCondiments()` hängt von der Wahl des Kunden ab (Konstruktorparameter).

Hooks zeigen, wie eine Unterklasse den **Ablauf** des Algorithmus **steuern** kann (einen Schritt überspringen), ohne den Algorithmus selbst zu ändern.

---

## Das „Hollywood-Prinzip“

*„Rufen Sie uns nicht an, wir rufen Sie an.“* Die Basisklasse ruft die Methoden der Unterklassen auf, nicht umgekehrt. Die Unterklasse steuert den Algorithmus nicht – sie liefert nur Details, wenn sie darum gebeten wird.

## Wann einsetzen

- Mehrere Klassen tun dasselbe und unterscheiden sich nur in einzelnen Schritten.
- Nur bestimmte Teile eines Algorithmus sollen erweiterbar sein, ohne seine Struktur ändern zu lassen.
- Duplizierter gemeinsamer Code in ähnlichen Klassen soll beseitigt werden.
- Beispiele: Datenimport/-export, Berichtserstellung, Anfrageverarbeitung (in ASP.NET Core `ControllerBase`, `BackgroundService.ExecuteAsync`), Test-Frameworks (`SetUp` → Test → `TearDown`).

## Wann nicht nötig

- Die Varianten des Algorithmus sollen **zur Laufzeit** gewechselt werden – dann ist Strategy besser.
- Es gibt viele Schritte, und die Unterklassen überschreiben fast alles – einen gemeinsamen Algorithmus gibt es faktisch nicht.

## Vor- und Nachteile

**Vorteile**
- Gemeinsamer Code an einer Stelle, keine Duplikate.
- Die Struktur des Algorithmus ist vor Änderungen geschützt.
- Unterklassen überschreiben nur das, was sich tatsächlich unterscheidet.

**Nachteile**
- Feste Kopplung über **Vererbung**: Änderungen an der Basisklasse betreffen alle Nachfahren.
- Durch die Struktur des Gerüsts begrenzt – ein neuer Schritt in der Mitte erfordert eine Änderung der Basisklasse.
- Das Liskovsche Substitutionsprinzip kann verletzt werden, wenn eine Unterklasse einen Schritt „unerwartet“ überschreibt (z. B. einen Pflichtschritt unterdrückt).
- Je mehr Schritte, desto schwerer die Wartung.

## Beziehung zu anderen Mustern

- **Strategy** löst eine ähnliche Aufgabe über **Komposition** und ermöglicht es, den Algorithmus zur Laufzeit vollständig auszutauschen. **Template Method** tut es über **Vererbung**, die Variante wird auf Klassenebene festgelegt.
- **Factory Method** ist ein Spezialfall von Template Method: Ein Schritt des Algorithmus ist die Erzeugung eines Objekts. In diesem Projekt ist `NotificationFactory.Notify()` aus `FactoryMethod/NotifySystem` eine Schablonenmethode, die die Fabrikmethode `CreateNotification()` aufruft.
