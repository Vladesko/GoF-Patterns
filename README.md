🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa)

# GoF-Entwurfsmuster in C#

Ein Lernprojekt mit allen 23 Entwurfsmustern der „Gang of Four“ (GoF), umgesetzt in C# (.NET 10). Jedes Muster liegt in einem eigenen Ordner mit einer README-Datei (Problem, Lösung, Beteiligte, Schema, Vor- und Nachteile, Beziehungen zu anderen Mustern) und ein bis zwei praxisnahen Beispielen.

## Starten

```bash
dotnet run --project GoFPatterns
```

Es erscheint ein Konsolenmenü mit allen Demos. Geben Sie die Nummer der gewünschten Demo ein und drücken Sie Enter; mit `0` wird das Programm beendet.

## Muster

### Erzeugungsmuster

| Muster | Beschreibung |
|--------|--------------|
| [Abstract Factory](GoFPatterns/AbtractFactory/README.md) | Erzeugt Familien zusammengehöriger Objekte, ohne an konkrete Klassen gebunden zu sein |
| [Builder](GoFPatterns/Builder/README.md) | Baut komplexe Objekte Schritt für Schritt auf |
| [Factory Method](GoFPatterns/FactoryMethod/README.md) | Überlässt Unterklassen die Entscheidung, welches Objekt erzeugt wird |
| [Prototype](GoFPatterns/Prototype/README.md) | Erzeugt neue Objekte durch Kopieren vorhandener |
| [Singleton](GoFPatterns/Singleton/README.md) | Garantiert genau eine Instanz mit globalem Zugriffspunkt |

### Strukturmuster

| Muster | Beschreibung |
|--------|--------------|
| [Adapter](GoFPatterns/Adapter/README.md) | Lässt Objekte mit inkompatiblen Schnittstellen zusammenarbeiten |
| [Bridge](GoFPatterns/Bridge/README.md) | Trennt Abstraktion und Implementierung in unabhängige Hierarchien |
| [Composite](GoFPatterns/Composite/README.md) | Fasst Objekte zu Baumstrukturen zusammen und behandelt sie einheitlich |
| [Decorator](GoFPatterns/Decorator/README.md) | Fügt Objekten dynamisch neues Verhalten durch Umhüllen hinzu |
| [Facade](GoFPatterns/Facade/README.md) | Bietet eine einfache Schnittstelle zu einem komplexen Subsystem |
| [Flyweight](GoFPatterns/Flyweight/README.md) | Spart Speicher, indem gemeinsamer Zustand geteilt wird |
| [Proxy](GoFPatterns/Proxy/README.md) | Stellvertreter, der den Zugriff auf ein Objekt kontrolliert |

### Verhaltensmuster

| Muster | Beschreibung |
|--------|--------------|
| [Chain of Responsibility](GoFPatterns/ChainOfResponsibility/README.md) | Reicht eine Anfrage durch eine Kette von Handlern |
| [Command](GoFPatterns/Command/README.md) | Macht aus einer Anfrage ein Objekt (Warteschlange, Undo/Redo) |
| [Interpreter](GoFPatterns/Interpreter/README.md) | Stellt die Grammatik einer einfachen Sprache als Klassen dar und interpretiert sie |
| [Iterator](GoFPatterns/Iterator/README.md) | Durchläuft eine Sammlung, ohne ihren Aufbau offenzulegen |
| [Mediator](GoFPatterns/Mediator/README.md) | Lässt Objekte über einen Vermittler statt direkt kommunizieren |
| [Memento](GoFPatterns/Memento/README.md) | Speichert und stellt Zustände wieder her, ohne die Kapselung zu verletzen |
| [Observer](GoFPatterns/Observer/README.md) | Benachrichtigt Abonnenten automatisch über Änderungen |
| [State](GoFPatterns/State/README.md) | Ändert das Verhalten eines Objekts abhängig von seinem Zustand |
| [Strategy](GoFPatterns/Strategy/README.md) | Kapselt austauschbare Algorithmen in eigenen Klassen |
| [Template Method](GoFPatterns/TemplateMethod/README.md) | Definiert das Gerüst eines Algorithmus, Unterklassen liefern einzelne Schritte |
| [Visitor](GoFPatterns/Visitor/README.md) | Fügt einer Klassenhierarchie neue Operationen hinzu, ohne sie zu ändern |
