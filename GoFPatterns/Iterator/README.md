🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Iterator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Iterator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Iterator) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Iterator)

# Iterator (Iteratore)

**Tipo:** pattern comportamentale.

**Iterator** offre un modo per **attraversare in sequenza gli elementi** di un oggetto composto (una collezione) **senza esporne la struttura interna**, che sia un array, una lista, un albero o altro.

---

## Quale problema risolve

Le collezioni possono essere diverse: array, lista collegata, albero, grafo, risultato di una query al database. Il client vuole semplicemente «passare su tutti gli elementi», ma:

- se l'attraversamento è scritto nel client, il client è **legato alla struttura** della collezione (indici per l'array, ricorsione per l'albero). Se si sostituisce l'array con un albero, bisogna riscrivere tutti i client;
- gli attraversamenti possono essere **diversi** (in avanti, all'indietro, con filtro, in profondità, in ampiezza). Se si aggiungono alla collezione stessa, questa si carica di responsabilità che non le appartengono;
- a volte servono **più attraversamenti indipendenti contemporaneamente**, quindi la posizione dell'attraversamento non può essere conservata nella collezione.

## Soluzione

Spostare la logica di attraversamento in un oggetto separato, l'**iterator**. L'iterator:
- conserva la **posizione corrente**;
- offre un'interfaccia semplice: «c'è un elemento successivo?» e «dammi il successivo»;
- conosce la struttura interna della collezione, il client no.

La collezione (**aggregate**) sa creare iterator per se stessa. Ogni chiamata crea un nuovo iterator con la propria posizione.

### Iterator in .NET

In C# il pattern è integrato nel linguaggio:

| GoF | .NET |
|-----|------|
| Aggregate | `IEnumerable<T>` (`GetEnumerator()`) |
| Iterator | `IEnumerator<T>` (`MoveNext()`, `Current`, `Reset()`) |
| Ciclo del client | `foreach` |
| Scrittura dell'iterator | `yield return`: il compilatore genera da solo la classe iterator |

Per questo nel codice reale di solito non si scrive un proprio `IBookIterator`, ma si implementa `IEnumerable<T>`. Entrambi i modi sono mostrati qui sotto.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Iterator** | `IBookIterator`, `IEnumerator<T>` | Interfaccia di attraversamento: `HasNext()`, `Next()`, `Reset()` |
| **ConcreteIterator** | `ForwardBookIterator`, `ReverseBookIterator`, `AuthorFilterIterator`, gli iterator generati da `yield return` | Implementa un algoritmo di attraversamento specifico e conserva la posizione corrente |
| **Aggregate** | `IBookAggregate`, `IEnumerable<T>` | Interfaccia della collezione che crea gli iterator |
| **ConcreteAggregate** | `Library`, `CategoryNode` | La collezione che restituisce gli iterator richiesti |
| **Client** | classi demo | Attraversa la collezione solo tramite l'iterator |

## Schema

```text
 Client ─────► «interface» IBookAggregate        «interface» IBookIterator
                 CreateIterator()        ─────►    HasNext(), Next(), Reset()
                        ▲                                   ▲
                        │                ┌──────────────────┼──────────────────┐
                     Library             │                  │                  │
                  (Book[] interno) ◄─ ForwardBookIterator  ReverseBookIterator  AuthorFilterIterator
                                         _position          _position           _position, author
```

---

## Esempio 1. Biblioteca di libri (`BookCollection/`): Iterator classico e `IEnumerable<T>`

**ConcreteAggregate**: `Library` conserva i libri in un **array** con espansione manuale (`Array.Resize`). I client non lo sanno: l'array si può sostituire con qualsiasi altra cosa senza toccarli.

**Iterator**: `IBookIterator` con i metodi `HasNext()`, `Next()`, `Reset()`.

**ConcreteIterator** (cartella `Iterators/`):
- `ForwardBookIterator`: dal primo libro all'ultimo;
- `ReverseBookIterator`: dall'ultimo al primo;
- `AuthorFilterIterator`: salta i libri degli altri autori:

```csharp
public bool HasNext()
{
    while (_position < library.Count && library.GetAt(_position).Author != author)
        _position++;
    return _position < library.Count;
}
```

Gli iterator accedono alla collezione tramite il metodo `internal` `GetAt`, che i client non vedono.

**Aggregate**: `IBookAggregate` dichiara i factory method degli iterator; `Library` li implementa.

**Client**: il metodo `Print` della demo accetta **qualsiasi** `IBookIterator` e lo attraversa con lo stesso ciclo `while (iterator.HasNext())`.

Inoltre `Library` implementa `IEnumerable<Book>` tramite `yield return`:

```csharp
public IEnumerator<Book> GetEnumerator()
{
    for (var i = 0; i < _count; i++)
        yield return _books[i];
}
```

Questo fornisce subito `foreach` e tutto LINQ (`library.Where(...).OrderBy(...)`).

## Esempio 2. Attraversamento di un albero di categorie (`Tree/`)

**ConcreteAggregate**: `CategoryNode`, un nodo dell'albero del catalogo. La lista dei figli è privata.

**ConcreteIterator**: due algoritmi di attraversamento scritti con `yield return`:
- `DepthFirst()`: **in profondità** (come un menu con rientri). Si usa uno `Stack` esplicito, non la ricorsione, così un albero profondo non provoca `StackOverflowException`;
- `BreadthFirst()`: **in ampiezza**, per livelli, tramite una `Queue`.

```csharp
public IEnumerable<(CategoryNode Node, int Depth)> BreadthFirst()
{
    var queue = new Queue<(CategoryNode, int)>();
    queue.Enqueue((this, 0));
    while (queue.Count > 0)
    {
        var (node, depth) = queue.Dequeue();
        yield return (node, depth);
        foreach (var child in node._children)
            queue.Enqueue((child, depth + 1));
    }
}
```

**Client**: attraversa lo stesso albero in due modi con un normale `foreach`.

Una proprietà importante degli iterator basati su `yield` è la **pigrizia** (lazy evaluation): gli elementi vengono calcolati uno alla volta, su richiesta. Nella demo la ricerca `First(n => n.Name == "Portatili")` interrompe l'attraversamento appena il nodo viene trovato e visita solo una parte dell'albero.

---

## Quando usarlo

- Bisogna nascondere al client la struttura interna di una collezione complessa.
- Servono più varianti di attraversamento della stessa collezione.
- Serve un'interfaccia di attraversamento unica per collezioni diverse.
- Serve un attraversamento lazy (sequenze grandi o infinite, caricamento a pagine dal database).

## Quando non serve

- La collezione è semplice (`List<T>`, array): usate il `foreach` integrato.

## Pro e contro

**Pro**
- Semplifica le collezioni: la logica di attraversamento è separata (SRP).
- Algoritmi di attraversamento diversi senza modificare la collezione (Open/Closed).
- Più attraversamenti indipendenti contemporaneamente.
- Calcolo lazy.

**Contro**
- Per le collezioni semplici è una complicazione superflua.
- La modifica della collezione durante l'attraversamento è un problema a parte (le collezioni standard di .NET lanciano `InvalidOperationException`).

## Relazioni con altri pattern

- **Composite**: gli iterator sono spesso usati per attraversare alberi Composite.
- **Factory Method**: `CreateIterator()` nella collezione è un factory method che restituisce la sottoclasse di iterator richiesta.
- **Memento**: si può salvare lo stato dell'iterazione e tornarvi in seguito.
- **Visitor** + Iterator: l'iterator attraversa la struttura e il visitor esegue un'operazione su ogni elemento.
