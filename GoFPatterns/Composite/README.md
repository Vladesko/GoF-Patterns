🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Composite) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Composite) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Composite) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Composite)

# Composite (Kompositum)

**Typ:** Strukturmuster.

**Composite** ermöglicht es, Objekte zu einer **Baumstruktur** zusammenzufassen und mit ihr so zu arbeiten, als wäre sie ein einzelnes Objekt. Der Client spricht ein einzelnes Element und eine ganze Gruppe auf dieselbe Weise an.

---

## Welches Problem wird gelöst

Es gibt eine „Teil-Ganzes“-Hierarchie: Dateien und Ordner, Mitarbeiter und Abteilungen, Waren und Kartons mit Waren, Menüeinträge und Untermenüs.

Man möchte zum Beispiel die Größe eines Ordners berechnen. Ohne das Muster muss der Client:
- den Typ jedes Elements prüfen (`if (item is File) ... else if (item is Folder) ...`);
- den Baumdurchlauf mit Verschachtelung selbst schreiben;
- diesen Code jedes Mal umschreiben, wenn ein neuer Elementtyp hinzukommt (z. B. eine Verknüpfung oder ein Archiv).

## Lösung

Man führt eine **gemeinsame Schnittstelle** für einfache und zusammengesetzte Elemente ein. Diese Schnittstelle hat eine Methode, die für beide sinnvoll ist (`GetSize()`):

- ein **Blatt** (Datei) gibt einfach seine Größe zurück;
- ein **Container** (Ordner) ruft `GetSize()` bei allen Kindern auf und summiert das Ergebnis.

Da auch die Kinder die gemeinsame Schnittstelle implementieren, ergibt sich die Rekursion von selbst, und der Client ruft eine Methode an der Wurzel auf, ohne sich darum zu kümmern, was darin steckt.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Component** | `FileSystemItem`, `IOrgUnit` | Gemeinsame Schnittstelle für einfache und zusammengesetzte Elemente |
| **Leaf** | `FileItem`, `Employee` | Einfaches Element ohne Kinder. Erledigt die eigentliche Arbeit |
| **Composite** | `FolderItem`, `Department` | Container. Hält Kinder (vom Typ Component), delegiert die Arbeit an sie und fasst das Ergebnis zusammen |
| **Client** | Demo-Klassen | Arbeitet mit allen Elementen über Component |

## Schema

```text
           ┌──────────────────────────┐
 Client ──►│ «abstract» FileSystemItem │◄──────────────┐
           │ + GetSize()              │                │ Kinder
           │ + Print()                │                │ 0..*
           └──────────────────────────┘                │
                 ▲                ▲                    │
                 │                │                    │
          ┌──────────┐     ┌──────────────────────┐    │
          │ FileItem │     │ FolderItem           │◆───┘
          │ (Leaf)   │     │ + Add(), Remove()    │
          └──────────┘     │ GetSize() = Σ Kinder │
                           └──────────────────────┘
```

---

## Beispiel 1. Dateisystem (`FileSystem/`)

**Component** – die abstrakte Klasse `FileSystemItem` mit den Operationen `GetSize()`, `Print()` und `Find()`.

**Leaf** – `FileItem` (Ordner `Items/`) kennt seine Größe und prüft bei der Suche nur sich selbst:

```csharp
public override long GetSize() => size;

public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate) =>
    predicate(this) ? [this] : [];
```

**Composite** – `FolderItem` hält eine Liste von Kindern und **delegiert** alle Operationen an sie:

```csharp
public override long GetSize() => _children.Sum(c => c.GetSize());

public override void Print(string indent = "")
{
    Console.WriteLine($"{indent}+ {Name}/ ({GetSize()} B)");
    foreach (var child in _children)
        child.Print(indent + "    ");
}
```

Die Methode `Add` gibt `this` zurück, daher lässt sich der Baum in der Demo bequem „treppenförmig“ aufbauen.

**Client** – die Demo ruft `GetSize()` sowohl für eine einzelne Datei als auch für das gesamte Projekt auf – der Aufrufcode ist identisch. `Find(i => i.Name.EndsWith(".cs"))` findet rekursiv alle C#-Dateien in beliebiger Tiefe.

### Wo `Add`/`Remove` hingehören

Hier gibt es die Methoden zur Verwaltung der Kinder **nur in `FolderItem`** – das ist die „sichere“ Variante: Bei einer Datei kann man `Add` nicht aufrufen. Im GoF-Buch wird auch die „transparente“ Variante beschrieben, bei der `Add`/`Remove` in Component deklariert sind: Dann unterscheidet der Client die Elemente überhaupt nicht, aber das Blatt muss bei `Add` eine Ausnahme werfen.

## Beispiel 2. Unternehmensstruktur (`OrgStructure/`)

**Aufgabe:** Personalstärke und Gehaltsbudget für jede beliebige Organisationseinheit berechnen – von einem einzelnen Mitarbeiter bis zum ganzen Unternehmen.

**Component** – die Schnittstelle `IOrgUnit` (`GetSalaryBudget()`, `GetHeadcount()`, `Display()`).

**Leaf** – `Employee`: Personalstärke 1, Budget – sein Gehalt.

**Composite** – `Department`: summiert die Werte aller enthaltenen Mitarbeiter und Abteilungen. Die Abteilung „Entwicklung“ enthält sowohl einen Mitarbeiter (CTO) als auch zwei Unterabteilungen – Blätter und Container dürfen auf einer Ebene gemischt werden.

```csharp
public decimal GetSalaryBudget() => _units.Sum(u => u.GetSalaryBudget());
public int GetHeadcount() => _units.Sum(u => u.GetHeadcount());
```

**Client** – in der Demo wird das Array `IOrgUnit[] units = [company, development, backend, new Employee(...)]` in einer einzigen Schleife verarbeitet, obwohl darin sowohl das ganze Unternehmen als auch ein einzelner Praktikant stecken.

---

## Wann einsetzen

- Eine „Teil-Ganzes“-Hierarchie soll als Baum dargestellt werden.
- Der Client soll einheitlich mit einfachen und zusammengesetzten Objekten arbeiten.
- Beispiele aus der Praxis: Dateisystem, UI-Komponenten (Container und Elemente), Menüs, XML/HTML-DOM, Ausdrücke in einem Taschenrechner, eine Bestellung aus Waren und Sets.

## Wann nicht nötig

- Die Struktur ist flach, es gibt keine Verschachtelung.
- Blätter und Container verhalten sich zu unterschiedlich, und die gemeinsame Schnittstelle wäre künstlich.

## Vor- und Nachteile

**Vorteile**
- Vereinfacht den Client: Elementtypen müssen nicht geprüft werden.
- Neue Elementarten lassen sich leicht hinzufügen (Open/Closed).
- Komplexe Operationen auf dem Baum lassen sich rekursiv und kurz schreiben.

**Nachteile**
- Die gemeinsame Schnittstelle kann zu allgemein werden: Sie enthält Methoden, die für manche Elemente keinen Sinn ergeben.
- Es ist schwer einzuschränken, welche Elemente in einen Container dürfen (z. B. „in den Ordner nur Dateien eines bestimmten Typs“) – dafür sind Prüfungen zur Laufzeit nötig.

## Beziehung zu anderen Mustern

- **Builder** eignet sich gut für den schrittweisen Aufbau komplexer Composite-Bäume.
- **Iterator** – zum Durchlaufen eines Composite-Baums (siehe Beispiel `Iterator/Tree`).
- **Visitor** – um Operationen auf allen Knoten des Baums auszuführen, ohne ihre Klassen zu ändern.
- **Chain of Responsibility** arbeitet oft auf einem Composite: Eine Anfrage wird vom Blatt nach oben an die Eltern weitergegeben.
- **Flyweight** – die Blätter des Baums können als Flyweights umgesetzt werden, um Speicher zu sparen.
- **Decorator** ist strukturell ähnlich (rekursive Komposition), aber ein Dekorierer hat ein einziges umhülltes Objekt und das Ziel, Verhalten hinzuzufügen, während ein Kompositum viele Kinder hat und das Ziel, das Ergebnis zusammenzufassen.
