🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Memento) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Memento) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Memento) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Memento)

# Memento (Ricordo)

**Tipo:** pattern comportamentale.

**Memento** permette di **salvare e ripristinare** uno stato precedente di un oggetto **senza esporre** i dettagli della sua implementazione e senza violare l'incapsulamento.

---

## Quale problema risolve

Bisogna implementare «Annulla» in un editor o «Salva/Carica» in un gioco. Per farlo occorre conservare da qualche parte delle copie dello stato dell'oggetto.

La variante ingenua è leggere dall'esterno tutti i campi dell'oggetto e salvarli. Ma:
- i campi di solito sono **privati**: bisognerebbe renderli pubblici e rompere l'incapsulamento;
- il codice che salva lo stato inizia a **dipendere dalla struttura interna** dell'oggetto: se si aggiunge un campo, bisogna modificare il codice della cronologia;
- dall'esterno lo stato può essere **modificato** per errore, e il ripristino riporterebbe l'oggetto in uno stato non valido.

## Soluzione

Affidare la creazione dello snapshot **all'oggetto stesso**: solo lui conosce il proprio stato completo. L'oggetto crea uno **snapshot** (memento), un oggetto immutabile con una copia dello stato, e sa ripristinarsi a partire da esso.

La conservazione degli snapshot è affidata a un oggetto separato, il **caretaker** (custode). Decide *quando* salvare e ripristinare, ma **non può guardare dentro** lo snapshot (o può leggere solo i metadati).

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Originator** | `TextEditor`, `Hero` | L'oggetto il cui stato viene salvato. Crea gli snapshot e si ripristina da essi |
| **Memento** | `TextEditor.Snapshot` (tramite `IEditorMemento`), `HeroMemento` | Snapshot immutabile dello stato |
| **Caretaker** | `EditorHistory`, `SaveManager` | Conserva gli snapshot e decide quando salvare/ripristinare. Non modifica il contenuto degli snapshot |

## Schema

```text
 ┌──────────────────────┐   Save()    ┌────────────────────────────┐
 │ TextEditor           │────────────►│ Snapshot  «private nested» │
 │ (Originator)         │             │ Text, Cursor, Font         │
 │ - _text, _cursor,    │◄────────────│ implements IEditorMemento  │
 │   _font              │  Restore()  └────────────────────────────┘
 └──────────────────────┘                       ▲
            ▲                                   │ vede solo
            │ Save / Restore                    │ IEditorMemento
 ┌──────────────────────────┐                   │ (Label, CreatedAt)
 │ EditorHistory (Caretaker)│───────────────────┘
 │ Stack<IEditorMemento>    │
 └──────────────────────────┘
```

---

## Esempio 1. Cronologia dell'editor di testo (`TextEditorHistory/`): incapsulamento rigoroso

**Originator**: `TextEditor` con stato privato: testo, posizione del cursore, font. Verso l'esterno non ci sono getter per questi campi.

**Memento**: `Snapshot` è dichiarato come **classe annidata privata** dell'editor. La classe annidata vede tutto all'interno dell'editor, ma dall'esterno il suo tipo non è accessibile: i client ricevono lo snapshot solo tramite l'**interfaccia ristretta** `IEditorMemento` (etichetta e ora di creazione):

```csharp
public IEditorMemento Save(string label) => new Snapshot(label, _text, _cursor, _font);

public void Restore(IEditorMemento memento)
{
    if (memento is not Snapshot snapshot)
        throw new ArgumentException("Tipo di memento sconosciuto.");
    _text = snapshot.Text;
    _cursor = snapshot.Cursor;
    _font = snapshot.Font;
}

private sealed class Snapshot(...) : IEditorMemento { ... }
```

È l'equivalente C# delle interfacce «ampia» e «ristretta» della GoF: l'editor vede tutto il contenuto dello snapshot, il caretaker solo i metadati.

**Caretaker**: `EditorHistory` conserva gli snapshot in uno stack. `Backup` prende uno snapshot dall'editor, `Undo` estrae l'ultimo e lo passa all'editor. Il caretaker non legge lo snapshot e non può modificarlo.

**Client**: la demo esegue delle modifiche, salvando lo stato prima di ognuna, e poi torna indietro: insieme al testo vengono ripristinati anche il cursore e il font.

## Esempio 2. Salvataggi di gioco (`GameSave/`): lo snapshot come `record`

**Originator**: `Hero`: livello, salute, oro, luogo, inventario. Le proprietà sono di sola lettura dall'esterno (`private set`) e cambiano tramite le azioni di gioco (`Travel`, `Fight`, `Buy`).

**Memento**: `HeroMemento`, un `sealed record`. In C# un record è un modo comodo per descrivere uno snapshot immutabile. L'inventario viene **copiato** in un nuovo array (`[.. _inventory]`), altrimenti lo snapshot condividerebbe la lista con l'eroe e gli acquisti fatti dopo il salvataggio «filtrerebbero» nel salvataggio.

**Caretaker**: `SaveManager` conserva gli snapshot in slot con nome e sa caricarli. Qui l'incapsulamento è più morbido: lo snapshot è pubblico e il manager ne legge i dati per la lista dei salvataggi. Però può **modificare** l'eroe solo tramite `hero.Load(memento)`, e lo snapshot stesso è immutabile.

**Client**: la demo gioca una parte della partita, salva, perde contro il boss e carica prima il salvataggio automatico e poi uno precedente.

---

## Quando usarlo

- Bisogna salvare snapshot dello stato per l'annullamento, il rollback di una transazione, i salvataggi di gioco, le bozze.
- Ottenere direttamente lo stato dell'oggetto esporrebbe i dettagli dell'implementazione.

## Quando non serve

- Lo stato dell'oggetto è semplice e pubblico: basta copiarlo.
- Lo stato è enorme ma cambia poco alla volta: conservare snapshot completi è costoso; meglio conservare le modifiche (l'approccio di Command con `Undo`).

## Pro e contro

**Pro**
- Non viola l'incapsulamento dell'oggetto originale.
- Semplifica l'oggetto originale: non deve conservare la cronologia.

**Contro**
- Consumo di memoria, se gli snapshot sono molti o grandi.
- Il caretaker deve gestire il ciclo di vita degli snapshot (eliminare quelli obsoleti).
- Nei linguaggi a tipizzazione dinamica è difficile garantire che nessuno modifichi lo snapshot; in C# aiutano le classi annidate `private` e i `record` immutabili.

## Relazioni con altri pattern

- **Command** + **Memento**: il comando salva uno snapshot del receiver prima dell'esecuzione, e `Undo` lo ripristina. Command conserva «come annullare», Memento «com'era».
- **Iterator**: si può salvare uno snapshot della posizione dell'iterazione.
- **Prototype**: se lo stato è semplice, lo snapshot si può ottenere clonando l'oggetto.
- **State**: lo snapshot può includere l'oggetto stato corrente.
