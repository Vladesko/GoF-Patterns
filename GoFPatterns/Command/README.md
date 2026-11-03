🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Command) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Command) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Command) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Command)

# Command (Befehl)

**Typ:** Verhaltensmuster.

**Command** macht aus einer Anfrage (Aktion) ein **eigenständiges Objekt**, das alle Informationen über die Anfrage enthält. Ein solches Objekt kann als Parameter übergeben, in eine Warteschlange gestellt, protokolliert, später ausgeführt und **rückgängig gemacht** werden.

---

## Welches Problem wird gelöst

**Texteditor.** Es gibt die Operationen „Einfügen“, „Löschen“, „Ersetzen“, die aus dem Menü, über Tastenkürzel und aus der Werkzeugleiste aufgerufen werden. Sie sollen **rückgängig gemacht und wiederholt** werden können (Undo/Redo). Ruft eine Schaltfläche direkt `document.Delete(...)` auf, dann:
- wird der Aufrufcode an allen Stellen dupliziert (Menü, Schaltfläche, Tastenkürzel);
- ist unklar, wie die Aktion rückgängig gemacht werden soll: Der gelöschte Text ist bereits verloren;
- gibt es keinen zentralen Ort, an dem der Verlauf gespeichert wird.

**Smart Home.** Eine Fernbedienung mit Tasten, denen der Benutzer selbst Aktionen zuweist: „Licht an“, „Haus verlassen“ (alles aus + Alarmanlage scharf). Die Fernbedienung soll nicht alle Geräte kennen, und Szenarien sollen sich aus einfachen Aktionen zusammensetzen lassen.

## Lösung

Jede Aktion wird als Objekt mit gemeinsamer Schnittstelle modelliert (`Execute()`, bei Bedarf `Undo()`). Der Befehl speichert:
- eine Referenz auf den **Empfänger** – das Objekt, das die Arbeit tatsächlich erledigt;
- alle **Parameter** des Aufrufs;
- Daten zum **Rückgängigmachen** (z. B. den gelöschten Text).

Der **Aufrufer** (Schaltfläche, Fernbedienung, Verlauf) arbeitet nur mit der Befehlsschnittstelle und kennt weder den Empfänger noch die Details der Operation.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Command** | `ICommand`, `IHomeCommand` | Befehlsschnittstelle: `Execute()`, manchmal `Undo()` |
| **ConcreteCommand** | `InsertTextCommand`, `DeleteTextCommand`, `ReplaceTextCommand`, `ActionCommand`, `MacroCommand` | Hält Empfänger und Parameter, implementiert `Execute` durch Aufruf der Methoden des Empfängers |
| **Receiver** | `Document`, `Light`, `Thermostat`, `SecuritySystem` | Enthält die Geschäftslogik. Weiß nichts über Befehle |
| **Invoker** | `CommandHistory`, `RemoteControl` | Startet Befehle, kann ihren Verlauf oder eine Warteschlange speichern |
| **Client** | Demo-Klassen | Erzeugt Befehle, verbindet sie mit Empfängern und übergibt sie dem Aufrufer |

## Schema

```text
 Client ── erzeugt ──► InsertTextCommand(document, pos, text)
                               │
                               ▼
 CommandHistory (Invoker)   «interface» ICommand          Document (Receiver)
   _undo: Stack<ICommand> ─► Execute()  ──── ruft ────►   Insert(pos, text)
   _redo: Stack<ICommand>    Undo()     ──── ruft ────►   Delete(pos, len)
```

---

## Beispiel 1. Texteditor mit Undo/Redo (`TextEditor/`)

**Receiver** – `Document`: ein Wrapper um `StringBuilder` mit den Methoden `Insert`, `Delete`, `GetText`. Weiß nichts über Befehle und Verlauf.

**Command** – `ICommand` mit `Execute()`, `Undo()` und `Name` (für die Ausgabe).

**ConcreteCommand** (Ordner `Commands/`):

`InsertTextCommand` – Einfügen rückgängig machen = das Eingefügte löschen:

```csharp
public void Execute() => document.Insert(position, text);
public void Undo() => document.Delete(position, text.Length);
```

`DeleteTextCommand` – **merkt sich** vor dem Löschen den zu löschenden Text, sonst wäre das Rückgängigmachen unmöglich:

```csharp
public void Execute()
{
    _deletedText = document.GetText(position, length);
    document.Delete(position, length);
}
public void Undo() => document.Insert(position, _deletedText);
```

`ReplaceTextCommand` – ein **zusammengesetzter** Befehl aus Löschen und Einfügen; macht beide in umgekehrter Reihenfolge rückgängig.

**Invoker** – `CommandHistory` hält zwei Stacks:

```csharp
public void Execute(ICommand command)
{
    command.Execute();
    _undo.Push(command);
    _redo.Clear();          // eine neue Aktion macht die „Zukunft“ ungültig
}

public void Undo()
{
    if (!_undo.TryPop(out var command)) return;
    command.Undo();
    _redo.Push(command);
}
```

**Client** – die Demo führt eine Reihe von Bearbeitungen aus, danach Undo/Redo. Am Ende wird gezeigt, dass nach einer neuen Aktion kein Redo mehr möglich ist – so funktionieren alle Editoren.

## Beispiel 2. Smart-Home-Fernbedienung (`SmartHome/`)

**Receiver** – die Geräte im Ordner `Devices/`: `Light`, `Thermostat`, `SecuritySystem`.

**Command** – `IHomeCommand` (`Description`, `Execute()`), ohne Rückgängigmachen.

**ConcreteCommand** (Ordner `Commands/`):
- `ActionCommand` – ein Befehl auf Basis eines **Delegaten**. In C# muss ein einfacher Befehl nicht für jede Aktion als eigene Klasse modelliert werden – ein Lambda oder eine Methodenreferenz genügt: `new ActionCommand("Alarmanlage scharf schalten", security.Arm)`. Im Grunde ist `Action` genau ein „Befehl ohne Undo“.
- `MacroCommand` – ein **Makrobefehl**: hält eine Liste von Befehlen und führt sie nacheinander aus. Da er selbst ein `IHomeCommand` ist, kann man andere Makros hineinschachteln – das Szenario „Haus verlassen“ enthält das Makro „Alle Lichter aus“ (das ist zugleich ein Composite).

**Invoker** – `RemoteControl`:
- die Tasten (`Dictionary<int, IHomeCommand>`) werden zur Laufzeit über `SetCommand` belegt;
- Befehle können **in eine Warteschlange gestellt** (`Schedule`) und später ausgeführt werden (`RunScheduled`) – das ist gerade deshalb möglich, weil die Aktion zu einem Objekt geworden ist.

**Client** – die Demo erzeugt die Geräte, baut die Szenarien auf und legt sie auf die Tasten. Die Fernbedienung greift kein einziges Mal direkt auf die Geräte zu.

---

## Wann einsetzen

- Objekte sollen mit Aktionen parametrisiert werden (Schaltflächen, Menüpunkte, Tastenkürzel).
- Operationen sollen rückgängig gemacht/wiederholt werden können.
- Operationen sollen in Warteschlangen gestellt, zeitgesteuert oder entfernt ausgeführt werden (Message Queues, Hintergrundjobs).
- Operationen sollen protokolliert werden, um sie nach einem Absturz erneut „abspielen“ zu können.
- Transaktionen/Szenarien aus mehreren Aktionen werden gebraucht (Makrobefehle).

## Wann nicht nötig

- Es gibt nur eine Aktion, die von einer Stelle aus aufgerufen wird und weder Undo noch Warteschlange braucht – ein einfacher Methodenaufruf ist verständlicher.

## Vor- und Nachteile

**Vorteile**
- Der Aufrufer ist vom Empfänger getrennt.
- Rückgängigmachen, Wiederholen, verzögerte Ausführung, Warteschlangen, Protokollierung.
- Einfache Befehle lassen sich zu komplexen zusammensetzen (Makros).
- Neue Befehle kommen ohne Änderung des vorhandenen Codes hinzu (Open/Closed).

**Nachteile**
- Viele kleine Klassen (teilweise lösbar mit Delegaten, wie bei `ActionCommand`).
- Das Rückgängigmachen kann kompliziert sein: Der Befehl muss genug Daten speichern, um den Zustand wiederherzustellen.

## Beziehung zu anderen Mustern

- **Memento** – für komplexes Rückgängigmachen kann ein Befehl vor der Ausführung einen Schnappschuss des Empfängerzustands speichern.
- **Chain of Responsibility** – die Anfrage in der Kette wird oft als Befehl modelliert.
- **Composite** – `MacroCommand` ist ein Kompositum aus Befehlen.
- **Strategy** verpackt ebenfalls Verhalten in ein Objekt, aber Strategien sind **verschiedene Wege**, dasselbe zu tun, während Befehle **verschiedene Aktionen** sind, die man speichern, aufschieben und rückgängig machen kann.
- **Prototype** – Befehle, die in den Verlauf gelegt werden, werden manchmal kopiert.
- In .NET sind `ICommand` in WPF/MAUI (MVVM) und die Anfragen `IRequest` in MediatR (CQRS) Befehle.
