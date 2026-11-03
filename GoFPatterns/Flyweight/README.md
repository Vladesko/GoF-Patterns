🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Flyweight) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Flyweight) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Flyweight) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Flyweight)

# Flyweight (Peso piuma)

**Tipo:** pattern strutturale.

**Flyweight** permette di far stare in memoria un gran numero di oggetti **condividendo lo stato comune** tra di essi, invece di conservare gli stessi dati in ogni oggetto.

---

## Quale problema risolve

In un gioco bisogna disegnare una foresta di 100 000 alberi. Ogni albero è un oggetto con coordinate, nome della specie, colore e **texture** (diciamo 1 MB). Se ogni albero conserva la propria copia della texture, servono ~100 GB di memoria, anche se le specie diverse sono solo quattro.

Lo stesso in un editor di testo: ogni carattere ha font, dimensione, colore, grassetto. Migliaia di caratteri, ma gli stili unici sono pochissimi.

Il problema è che **gran parte dei dati si ripete** in moltissimi oggetti.

## Soluzione

Dividere lo stato dell'oggetto in due parti:

- **Intrinseco (intrinsic)**: uguale per molti oggetti e indipendente dal contesto: specie, colore, texture; font, dimensione. Viene spostato in un oggetto **flyweight**, creato **una sola volta** e condiviso da tutti.
- **Estrinseco (extrinsic)**: unico per ogni oggetto: le coordinate dell'albero, il carattere e la sua posizione nel testo. Lo conserva il **contesto** (o il client) e lo passa al flyweight come parametro dei metodi.

I flyweight devono essere **immutabili**, perché lo stesso oggetto è usato da migliaia di contesti. Per non creare duplicati, i flyweight vengono forniti da una **factory** con un pool (cache).

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Flyweight** | `TreeType`, `CharacterStyle` | Conserva lo stato intrinseco (condiviso). È immutabile. Riceve lo stato estrinseco tramite parametri |
| **FlyweightFactory** | `TreeTypeFactory`, `StyleFactory` | Pool di flyweight: restituisce quello esistente o ne crea uno nuovo |
| **Context** | `Tree`, `FormattedCharacter` | Conserva lo stato estrinseco + un riferimento al flyweight. Questi oggetti sono molti, ma piccoli |
| **Client** | `ForestMap`, `TextDocument` | Crea i contesti, ottenendo i flyweight solo tramite la factory |

## Schema

```text
 ForestMap (Client)
   │ PlantTree(x, y, "Quercia", ...)
   ▼
 TreeTypeFactory ── _types: "Quercia|verde scuro|oak.png" ─► TreeType ◄──┐
                                                            (1 MB)        │
                                                                          │ condiviso
   Tree(x=10, y=20) ──────────────────────────────────────────────────────┤
   Tree(x=55, y=13) ──────────────────────────────────────────────────────┤
   Tree(x=91, y=77) ──────────────────────────────────────────────────────┘
   ... 100 000 piccoli contesti, 4 flyweight
```

---

## Esempio 1. Foresta (`Forest/`)

**Flyweight**: `TreeType`. Conserva nome, colore, percorso della texture e i dati «pesanti» della texture stessa (un array da 1 MB). La classe è `sealed`, tutte le proprietà sono di sola lettura. Le coordinate **non vengono conservate** al suo interno: arrivano al metodo:

```csharp
public string Draw(int x, int y) => $"{Name} ({Color}) in [{x}, {y}] con {TexturePath}";
```

**FlyweightFactory**: `TreeTypeFactory` conserva un dizionario `chiave → TreeType`. Un nuovo oggetto viene creato solo se non esiste ancora (nella demo si vede che il messaggio «creato un nuovo tipo di albero» viene stampato esattamente 4 volte su 100 000 alberi):

```csharp
if (!_types.TryGetValue(key, out var type))
{
    type = new TreeType(name, color, texturePath);
    _types[key] = type;
}
return type;
```

**Context**: `Tree`: due `int` e un riferimento a `TreeType`. Il metodo `Draw()` passa le proprie coordinate al flyweight.

**Client**: `ForestMap` pianta gli alberi tramite la factory e non crea mai `TreeType` da solo.

La demo calcola la quantità approssimativa di memoria: senza flyweight ~100 GB, con flyweight ~5,5 MB (4 texture + 100 000 piccoli contesti).

## Esempio 2. Formattazione del testo (`TextFormatting/`)

**Flyweight**: `CharacterStyle`, un `sealed record` (font, dimensione, grassetto, colore). Un record è immutabile e si confronta per valore, quindi è comodo usarlo come chiave di un dizionario.

**FlyweightFactory**: `StyleFactory` conserva il pool `Dictionary<CharacterStyle, CharacterStyle>`: se esiste già uno stile uguale per valore, viene restituito **lo stesso oggetto**, altrimenti il candidato viene aggiunto al pool.

**Context**: `FormattedCharacter`, un `readonly record struct` composto da carattere, posizione e riferimento allo stile.

**Client**: `TextDocument.Append(...)` ottiene lo stile dalla factory e crea i contesti per ogni carattere. La demo mostra che per tutti i caratteri del testo ci sono solo 3 oggetti stile, e `CountDistinctStyleObjects()` lo verifica tramite il confronto **per riferimento** (`ReferenceEqualityComparer`).

Alla fine viene mostrato un flyweight integrato in .NET: lo **string interning**. `string.Intern` restituisce lo stesso oggetto per stringhe uguali. I letterali stringa in C# vengono internati automaticamente.

---

## Quando usarlo

- Il programma ha **moltissimi** oggetti dello stesso tipo, che non stanno in memoria.
- Gran parte dello stato di questi oggetti può essere spostata all'esterno o condivisa.
- Gli oggetti possono essere resi immutabili.
- Esempi: particelle e tile nei giochi, caratteri negli editor, marker su una mappa, cache di icone.

## Quando non serve

- Gli oggetti sono pochi: il risparmio non vale la complicazione.
- Lo stato degli oggetti è quasi completamente unico: non c'è nulla da condividere.

## Pro e contro

**Pro**
- Notevole risparmio di memoria.

**Contro**
- Si spende tempo di CPU per la ricerca nel pool e per il passaggio dello stato estrinseco.
- Il codice si complica: lo stato è «sparpagliato» tra contesto e flyweight.
- I flyweight devono essere immutabili, altrimenti la modifica di uno si ripercuote su tutti i contesti.

## Relazioni con altri pattern

- **Composite**: le foglie dell'albero sono spesso flyweight (ad esempio i caratteri di un documento).
- **Singleton** è simile perché l'oggetto è condiviso, ma il singleton è un'unica istanza per classe, mentre i flyweight sono molti (uno per ogni stato intrinseco unico) e sono immutabili.
- **Factory Method** / object pool: la factory dei flyweight è in sostanza una cache.
- **State** e **Strategy**: gli oggetti stato e strategia senza campi propri sono spesso flyweight.
