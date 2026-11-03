🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Composite) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Composite) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Composite) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Composite)

# Composite (Composto)

**Tipo:** pattern strutturale.

**Composite** permette di raggruppare oggetti in una **struttura ad albero** e di lavorarci come se fosse un unico oggetto. Il client accede allo stesso modo sia a un singolo elemento sia a un intero gruppo.

---

## Quale problema risolve

C'è una gerarchia «parte-tutto»: file e cartelle, dipendenti e reparti, prodotti e scatole di prodotti, voci di menu e sottomenu.

Bisogna calcolare, ad esempio, la dimensione di una cartella. Senza il pattern il client è costretto a:
- verificare il tipo di ogni elemento (`if (item is File) ... else if (item is Folder) ...`);
- scrivere da solo l'attraversamento dell'albero tenendo conto dell'annidamento;
- riscrivere questo codice ogni volta che compare un nuovo tipo di elemento (ad esempio un collegamento o un archivio).

## Soluzione

Introdurre un'**interfaccia comune** per gli elementi semplici e composti. Questa interfaccia ha un metodo che ha senso per entrambi (`GetSize()`):

- la **foglia** (file) restituisce semplicemente la propria dimensione;
- il **contenitore** (cartella) chiama `GetSize()` su tutti i figli e somma il risultato.

Poiché anche i figli implementano l'interfaccia comune, la ricorsione viene da sé, e il client chiama un solo metodo sulla radice senza preoccuparsi di cosa ci sia dentro.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Component** | `FileSystemItem`, `IOrgUnit` | Interfaccia comune per gli elementi semplici e composti |
| **Leaf** | `FileItem`, `Employee` | Elemento semplice senza figli. Esegue il lavoro reale |
| **Composite** | `FolderItem`, `Department` | Contenitore. Conserva i figli (di tipo Component), delega loro il lavoro e combina il risultato |
| **Client** | classi demo | Lavora con tutti gli elementi tramite Component |

## Schema

```text
           ┌──────────────────────────┐
 Client ──►│ «abstract» FileSystemItem │◄──────────────┐
           │ + GetSize()              │                │ figli
           │ + Print()                │                │ 0..*
           └──────────────────────────┘                │
                 ▲                ▲                    │
                 │                │                    │
          ┌──────────┐     ┌──────────────────────┐    │
          │ FileItem │     │ FolderItem           │◆───┘
          │ (Leaf)   │     │ + Add(), Remove()    │
          └──────────┘     │ GetSize() = Σ figli  │
                           └──────────────────────┘
```

---

## Esempio 1. File system (`FileSystem/`)

**Component**: la classe astratta `FileSystemItem` con le operazioni `GetSize()`, `Print()` e `Find()`.

**Leaf**: `FileItem` (cartella `Items/`) conosce la propria dimensione e nella ricerca verifica solo se stesso:

```csharp
public override long GetSize() => size;

public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate) =>
    predicate(this) ? [this] : [];
```

**Composite**: `FolderItem` conserva la lista dei figli e **delega** loro tutte le operazioni:

```csharp
public override long GetSize() => _children.Sum(c => c.GetSize());

public override void Print(string indent = "")
{
    Console.WriteLine($"{indent}+ {Name}/ ({GetSize()} B)");
    foreach (var child in _children)
        child.Print(indent + "    ");
}
```

Il metodo `Add` restituisce `this`, quindi l'albero si assembla comodamente «a scaletta» direttamente nella demo.

**Client**: la demo chiama `GetSize()` sia su un singolo file sia sull'intero progetto, e il codice di chiamata è identico. `Find(i => i.Name.EndsWith(".cs"))` trova ricorsivamente tutti i file C# a qualsiasi profondità.

### Dove mettere `Add`/`Remove`

Qui i metodi di gestione dei figli esistono **solo in `FolderItem`**: è la variante «sicura», perché su un file non si può chiamare `Add`. Il libro della GoF descrive anche la variante «trasparente», in cui `Add`/`Remove` sono dichiarati in Component: in quel caso il client non distingue affatto gli elementi, ma la foglia è costretta a lanciare un'eccezione su `Add`.

## Esempio 2. Struttura aziendale (`OrgStructure/`)

**Problema:** calcolare l'organico e il monte stipendi per qualsiasi unità, da un singolo dipendente all'intera azienda.

**Component**: l'interfaccia `IOrgUnit` (`GetSalaryBudget()`, `GetHeadcount()`, `Display()`).

**Leaf**: `Employee`: organico 1, il budget è il suo stipendio.

**Composite**: `Department`: somma i valori di tutti i dipendenti e i reparti annidati. Il reparto `Sviluppo` contiene sia un dipendente (CTO) sia due sotto-reparti: foglie e contenitori si possono mescolare allo stesso livello.

```csharp
public decimal GetSalaryBudget() => _units.Sum(u => u.GetSalaryBudget());
public int GetHeadcount() => _units.Sum(u => u.GetHeadcount());
```

**Client**: nella demo l'array `IOrgUnit[] units = [company, development, backend, new Employee(...)]` viene elaborato con un unico ciclo, anche se contiene sia l'intera azienda sia un singolo stagista.

---

## Quando usarlo

- Bisogna rappresentare una gerarchia «parte-tutto» sotto forma di albero.
- Il client deve lavorare in modo uniforme sia con oggetti semplici sia con oggetti composti.
- Esempi dalla realtà: file system, componenti UI (contenitori ed elementi), menu, DOM XML/HTML, espressioni in una calcolatrice, un ordine composto da prodotti e confezioni.

## Quando non serve

- La struttura è piatta, senza annidamento.
- Foglie e contenitori hanno comportamenti troppo diversi, e l'interfaccia comune risulta artificiosa.

## Pro e contro

**Pro**
- Semplifica il client: non serve verificare il tipo degli elementi.
- È facile aggiungere nuovi tipi di elementi (Open/Closed).
- Le operazioni complesse sull'albero si scrivono in modo ricorsivo e conciso.

**Contro**
- L'interfaccia comune può diventare troppo generica: finisce per contenere metodi che non hanno senso per alcuni elementi.
- È difficile limitare quali elementi possono essere inseriti in un contenitore (ad esempio «nella cartella solo file di un certo tipo»): servono controlli a runtime.

## Relazioni con altri pattern

- **Builder** è comodo per assemblare passo dopo passo alberi Composite complessi.
- **Iterator**: per attraversare un albero Composite (vedi l'esempio `Iterator/Tree`).
- **Visitor**: per eseguire operazioni su tutti i nodi dell'albero senza modificarne le classi.
- **Chain of Responsibility** spesso lavora sopra un Composite: la richiesta passa dalla foglia verso l'alto, ai genitori.
- **Flyweight**: le foglie dell'albero possono essere flyweight per risparmiare memoria.
- **Decorator** è simile nella struttura (composizione ricorsiva), ma il decorator ha un solo oggetto annidato e lo scopo è aggiungere comportamento, mentre il composite ha molti figli e lo scopo è combinare il risultato.
