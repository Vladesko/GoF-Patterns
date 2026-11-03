🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Memento) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Memento) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Memento) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Memento)

# Memento (Erinnerung)

**Typ:** Verhaltensmuster.

**Memento** ermöglicht es, einen früheren Zustand eines Objekts zu **speichern und wiederherzustellen**, **ohne** Details seiner Implementierung **offenzulegen** und ohne die Kapselung zu verletzen.

---

## Welches Problem wird gelöst

Man möchte in einem Editor „Rückgängig“ oder in einem Spiel „Speichern/Laden“ umsetzen. Dafür müssen irgendwo Kopien des Objektzustands abgelegt werden.

Die naive Variante: alle Felder des Objekts von außen auslesen und speichern. Aber:
- die Felder sind meist **privat** – man müsste sie öffentlich machen und die Kapselung brechen;
- der Code, der den Zustand speichert, **hängt** dann **vom inneren Aufbau** des Objekts **ab**: Kommt ein Feld hinzu, muss der Verlaufscode geändert werden;
- von außen kann der Zustand versehentlich **verändert** werden, und ein Rollback versetzt das Objekt in einen ungültigen Zustand.

## Lösung

Das Erstellen des Schnappschusses wird **dem Objekt selbst** übertragen – nur es kennt seinen vollständigen Zustand. Das Objekt erzeugt einen **Schnappschuss** (Memento) – ein unveränderliches Objekt mit einer Kopie des Zustands – und kann sich daraus wiederherstellen.

Um die Aufbewahrung der Schnappschüsse kümmert sich ein eigenes Objekt – der **Verwalter** (Caretaker). Er entscheidet, *wann* gespeichert und wiederhergestellt wird, **kann** aber **nicht in** den Schnappschuss **hineinsehen** (oder darf nur Metadaten lesen).

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Originator** | `TextEditor`, `Hero` | Das Objekt, dessen Zustand gespeichert wird. Erzeugt Schnappschüsse und stellt sich daraus wieder her |
| **Memento** | `TextEditor.Snapshot` (über `IEditorMemento`), `HeroMemento` | Unveränderlicher Schnappschuss des Zustands |
| **Caretaker** | `EditorHistory`, `SaveManager` | Bewahrt die Schnappschüsse auf und entscheidet, wann gespeichert/wiederhergestellt wird. Ändert den Inhalt der Schnappschüsse nicht |

## Schema

```text
 ┌──────────────────────┐   Save()    ┌────────────────────────────┐
 │ TextEditor           │────────────►│ Snapshot  «private nested» │
 │ (Originator)         │             │ Text, Cursor, Font         │
 │ - _text, _cursor,    │◄────────────│ implements IEditorMemento  │
 │   _font              │  Restore()  └────────────────────────────┘
 └──────────────────────┘                       ▲
            ▲                                   │ sieht nur
            │ Save / Restore                    │ IEditorMemento
 ┌──────────────────────────┐                   │ (Label, CreatedAt)
 │ EditorHistory (Caretaker)│───────────────────┘
 │ Stack<IEditorMemento>    │
 └──────────────────────────┘
```

---

## Beispiel 1. Verlauf im Texteditor (`TextEditorHistory/`) – strikte Kapselung

**Originator** – `TextEditor` mit privatem Zustand: Text, Cursorposition, Schriftart. Nach außen gibt es für diese Felder keine Getter.

**Memento** – `Snapshot` ist als **private verschachtelte Klasse** des Editors deklariert. Die verschachtelte Klasse sieht alles im Editor, aber von außen ist ihr Typ nicht zugänglich – Clients erhalten den Schnappschuss nur über die **schmale Schnittstelle** `IEditorMemento` (Bezeichnung und Erstellungszeit):

```csharp
public IEditorMemento Save(string label) => new Snapshot(label, _text, _cursor, _font);

public void Restore(IEditorMemento memento)
{
    if (memento is not Snapshot snapshot)
        throw new ArgumentException("Unbekannter Memento-Typ.");
    _text = snapshot.Text;
    _cursor = snapshot.Cursor;
    _font = snapshot.Font;
}

private sealed class Snapshot(...) : IEditorMemento { ... }
```

Das ist das C#-Gegenstück zur „breiten“ und „schmalen“ Schnittstelle aus dem GoF-Buch: Der Editor sieht den gesamten Inhalt des Schnappschusses, der Verwalter nur die Metadaten.

**Caretaker** – `EditorHistory` legt die Schnappschüsse auf einen Stack. `Backup` holt einen Schnappschuss vom Editor, `Undo` nimmt den letzten heraus und übergibt ihn dem Editor. Den Schnappschuss selbst liest der Verwalter nicht und kann ihn auch nicht ändern.

**Client** – die Demo nimmt Änderungen vor, speichert vor jeder den Zustand und rollt dann zurück – zusammen mit dem Text werden Cursor und Schriftart wiederhergestellt.

## Beispiel 2. Spielstände (`GameSave/`) – Schnappschuss als `record`

**Originator** – `Hero`: Level, Gesundheit, Gold, Ort, Inventar. Die Eigenschaften sind von außen schreibgeschützt (`private set`) und ändern sich durch Spielaktionen (`Travel`, `Fight`, `Buy`).

**Memento** – `HeroMemento` ist ein `sealed record`. Ein Record ist in C# ein bequemer Weg, einen unveränderlichen Schnappschuss zu beschreiben. Das Inventar wird in ein neues Array **kopiert** (`[.. _inventory]`), sonst würde sich der Schnappschuss die Liste mit dem Helden teilen, und Käufe nach dem Speichern würden in den Spielstand „durchsickern“.

**Caretaker** – `SaveManager` speichert die Schnappschüsse in benannten Slots und kann sie laden. Hier ist die Kapselung lockerer: Der Schnappschuss ist öffentlich, und der Manager liest daraus die Daten für die Liste der Spielstände. **Ändern** kann er den Helden aber nur über `hero.Load(memento)`, und der Schnappschuss selbst ist unveränderlich.

**Client** – die Demo spielt einen Teil des Spiels, speichert, verliert gegen den Boss und lädt zuerst den Autosave und dann einen früheren Spielstand.

---

## Wann einsetzen

- Zustandsschnappschüsse sollen gespeichert werden – für Rückgängigmachen, Transaktions-Rollback, Spielstände, Entwürfe.
- Ein direkter Zugriff auf den Zustand des Objekts würde Implementierungsdetails offenlegen.

## Wann nicht nötig

- Der Zustand des Objekts ist einfach und öffentlich – es genügt, ihn zu kopieren.
- Der Zustand ist riesig, ändert sich aber nur in kleinen Schritten – vollständige Schnappschüsse zu speichern ist teuer; besser speichert man die Änderungen (der Command-Ansatz mit `Undo`).

## Vor- und Nachteile

**Vorteile**
- Verletzt die Kapselung des ursprünglichen Objekts nicht.
- Vereinfacht das ursprüngliche Objekt: Es muss keinen Verlauf speichern.

**Nachteile**
- Speicherverbrauch, wenn es viele oder große Schnappschüsse gibt.
- Der Verwalter muss den Lebenszyklus der Schnappschüsse im Blick behalten (veraltete löschen).
- In dynamisch typisierten Sprachen lässt sich schwer garantieren, dass niemand den Schnappschuss ändert; in C# helfen verschachtelte `private`-Klassen und unveränderliche `record`s.

## Beziehung zu anderen Mustern

- **Command** + **Memento**: Der Befehl speichert vor der Ausführung einen Schnappschuss des Empfängers, und `Undo` stellt ihn wieder her. Command speichert, „wie man rückgängig macht“, Memento, „was vorher war“.
- **Iterator** – man kann einen Schnappschuss der Iterationsposition speichern.
- **Prototype** – ist der Zustand einfach, kann der Schnappschuss durch Klonen des Objekts erstellt werden.
- **State** – der Schnappschuss kann das aktuelle Zustandsobjekt enthalten.
