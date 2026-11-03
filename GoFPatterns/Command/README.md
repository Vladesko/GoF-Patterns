🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Command) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Command) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Command) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Command)

# Command (Comando)

**Tipo:** pattern comportamentale.

**Command** trasforma una richiesta (un'azione) in un **oggetto autonomo**, che contiene tutte le informazioni sulla richiesta. Un oggetto del genere può essere passato come parametro, messo in coda, registrato nel log, eseguito in seguito e **annullato**.

---

## Quale problema risolve

**Editor di testo.** Ci sono le operazioni «inserisci», «elimina», «sostituisci», richiamate dal menu, dalle scorciatoie da tastiera e dalla barra degli strumenti. Bisogna poterle **annullare e ripetere** (Undo/Redo). Se il pulsante chiama direttamente `document.Delete(...)`:
- il codice di chiamata viene duplicato ovunque (menu, pulsante, scorciatoia);
- non è chiaro come annullare l'azione: il testo eliminato è già perso;
- non c'è un unico punto in cui conservare la cronologia.

**Casa intelligente.** Un telecomando con pulsanti a cui l'utente assegna le azioni: «accendi la luce», «esci di casa» (spegni tutto + attiva l'allarme). Il telecomando non deve conoscere tutti i dispositivi, e gli scenari devono essere composti da azioni semplici.

## Soluzione

Rappresentare ogni azione come un oggetto con un'interfaccia comune (`Execute()` e, se serve, `Undo()`). Il comando conserva:
- un riferimento al **receiver**, l'oggetto che esegue davvero il lavoro;
- tutti i **parametri** della chiamata;
- i dati per l'**annullamento** (ad esempio, il testo eliminato).

Il **mittente** (pulsante, telecomando, cronologia) lavora solo con l'interfaccia del comando e non conosce né il receiver né i dettagli dell'operazione.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Command** | `ICommand`, `IHomeCommand` | Interfaccia del comando: `Execute()`, a volte `Undo()` |
| **ConcreteCommand** | `InsertTextCommand`, `DeleteTextCommand`, `ReplaceTextCommand`, `ActionCommand`, `MacroCommand` | Conserva il receiver e i parametri, implementa `Execute` chiamando i metodi del receiver |
| **Receiver** | `Document`, `Light`, `Thermostat`, `SecuritySystem` | Contiene la logica di business. Non sa nulla dei comandi |
| **Invoker** | `CommandHistory`, `RemoteControl` | Avvia i comandi, può conservarne la cronologia o la coda |
| **Client** | classi demo | Crea i comandi, li collega ai receiver e li passa all'invoker |

## Schema

```text
 Client ── crea ──► InsertTextCommand(document, pos, text)
                               │
                               ▼
 CommandHistory (Invoker)   «interface» ICommand          Document (Receiver)
   _undo: Stack<ICommand> ─► Execute()  ──── chiama ───►   Insert(pos, text)
   _redo: Stack<ICommand>    Undo()     ──── chiama ───►   Delete(pos, len)
```

---

## Esempio 1. Editor di testo con Undo/Redo (`TextEditor/`)

**Receiver**: `Document`, un wrapper attorno a `StringBuilder` con i metodi `Insert`, `Delete`, `GetText`. Non sa nulla di comandi e cronologia.

**Command**: `ICommand` con `Execute()`, `Undo()` e `Name` (per l'output).

**ConcreteCommand** (cartella `Commands/`):

`InsertTextCommand`: annullare un inserimento = eliminare il testo inserito:

```csharp
public void Execute() => document.Insert(position, text);
public void Undo() => document.Delete(position, text.Length);
```

`DeleteTextCommand`: prima dell'eliminazione **memorizza** il testo da eliminare, altrimenti l'annullamento sarebbe impossibile:

```csharp
public void Execute()
{
    _deletedText = document.GetText(position, length);
    document.Delete(position, length);
}
public void Undo() => document.Insert(position, _deletedText);
```

`ReplaceTextCommand`: un comando **composto** da eliminazione e inserimento; li annulla in ordine inverso.

**Invoker**: `CommandHistory` conserva due stack:

```csharp
public void Execute(ICommand command)
{
    command.Execute();
    _undo.Push(command);
    _redo.Clear();          // una nuova azione invalida il «futuro»
}

public void Undo()
{
    if (!_undo.TryPop(out var command)) return;
    command.Undo();
    _redo.Push(command);
}
```

**Client**: la demo esegue una serie di modifiche, poi Undo/Redo. Alla fine si vede che dopo una nuova azione il Redo non è più possibile: è così che funzionano tutti gli editor.

## Esempio 2. Telecomando della casa intelligente (`SmartHome/`)

**Receiver**: i dispositivi nella cartella `Devices/`: `Light`, `Thermostat`, `SecuritySystem`.

**Command**: `IHomeCommand` (`Description`, `Execute()`), senza annullamento.

**ConcreteCommand** (cartella `Commands/`):
- `ActionCommand`: un comando basato su un **delegate**. In C# non è obbligatorio creare una classe separata per ogni azione semplice: basta una lambda o un riferimento a un metodo: `new ActionCommand("Attiva l'allarme", security.Arm)`. In sostanza un `Action` è proprio un «comando senza annullamento».
- `MacroCommand`: un **macro comando**: conserva una lista di comandi e li esegue uno dopo l'altro. Poiché anch'esso è un `IHomeCommand`, può contenere altre macro: lo scenario «Esci di casa» contiene la macro «Spegni tutte le luci» (è contemporaneamente anche un Composite).

**Invoker**: `RemoteControl`:
- i pulsanti (`Dictionary<int, IHomeCommand>`) vengono configurati a runtime tramite `SetCommand`;
- i comandi possono essere **messi in coda** (`Schedule`) ed eseguiti in seguito (`RunScheduled`): è possibile proprio perché l'azione è diventata un oggetto.

**Client**: la demo crea i dispositivi, assembla gli scenari e li assegna ai pulsanti. Il telecomando non accede mai direttamente ai dispositivi.

---

## Quando usarlo

- Bisogna parametrizzare oggetti con azioni (pulsanti, voci di menu, scorciatoie da tastiera).
- Servono annullamento/ripetizione delle operazioni.
- Bisogna mettere le operazioni in coda, eseguirle secondo una pianificazione o da remoto (code di messaggi, attività in background).
- Bisogna registrare le operazioni nel log per poterle «rieseguire» dopo un guasto.
- Servono transazioni/scenari composti da più azioni (macro comandi).

## Quando non serve

- L'azione è una sola, viene chiamata da un solo punto e non richiede annullamento né coda: una semplice chiamata di metodo è più chiara.

## Pro e contro

**Pro**
- Il mittente è separato dal receiver.
- Annullamento, ripetizione, esecuzione differita, code, logging.
- Comandi semplici si combinano in comandi complessi (macro).
- Nuovi comandi si aggiungono senza modificare il codice esistente (Open/Closed).

**Contro**
- Molte piccole classi (in parte risolto con i delegate, come in `ActionCommand`).
- L'annullamento può essere complesso: il comando deve salvare dati sufficienti per ripristinare lo stato.

## Relazioni con altri pattern

- **Memento**: per un annullamento complesso il comando può salvare uno snapshot dello stato del receiver prima dell'esecuzione.
- **Chain of Responsibility**: la richiesta nella catena viene spesso rappresentata come un comando.
- **Composite**: `MacroCommand` è un composite di comandi.
- Anche **Strategy** racchiude un comportamento in un oggetto, ma le strategie sono **modi diversi** di fare la stessa cosa, mentre i comandi sono **azioni diverse** che si possono conservare, rimandare e annullare.
- **Prototype**: i comandi da mettere nella cronologia a volte vengono copiati.
- In .NET sono comandi `ICommand` in WPF/MAUI (MVVM) e le richieste `IRequest` in MediatR (CQRS).
