🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Interpreter) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Interpreter) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Interpreter) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Interpreter)

# Interpreter (Interprete)

**Tipo:** pattern comportamentale.

**Interpreter** definisce la **grammatica di un linguaggio semplice** sotto forma di una gerarchia di classi e interpreta le frasi di questo linguaggio. Ogni regola della grammatica è una classe separata, e una frase del linguaggio è rappresentata da un **albero** di oggetti di queste classi (l'albero sintattico astratto, AST).

---

## Quale problema risolve

A volte lo stesso problema si ripresenta in varianti diverse, che è comodo descrivere con un **piccolo linguaggio** anziché con del codice:

- formule di calcolo di prezzi/stipendi configurate dall'utente: `prezzo * qta * (1 - iva)`;
- query di ricerca e filtri: `(portatile OR tablet) AND usato`;
- regole di business, template, espressioni regolari.

Se si analizzano queste stringhe in modo ad hoc (`Split`, `IndexOf`, `if`), il codice diventa presto un pasticcio illeggibile, e aggiungere una nuova operazione (ad esempio `NOT` o le parentesi) diventa difficile.

## Soluzione

1. Descrivere la **grammatica** del linguaggio.
2. Per ogni regola della grammatica creare una classe con il metodo `Interpret(context)`:
   - le espressioni **terminali** sono le «foglie» (numero, variabile, parola);
   - le espressioni **non terminali** contengono altre espressioni e ne combinano i risultati (addizione, `AND`, `OR`).
3. La frase del linguaggio diventa un albero di questi oggetti, e la chiamata di `Interpret` sulla radice calcola ricorsivamente il risultato.

Il pattern in sé **non descrive come costruire l'albero**: può farlo il programmatore a mano o un parser separato (negli esempi ci sono entrambe le varianti).

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **AbstractExpression** | `IExpression`, `IQueryExpression` | Interfaccia comune dei nodi dell'albero con il metodo di interpretazione |
| **TerminalExpression** | `NumberExpression`, `VariableExpression`, `TagExpression` | Foglie: vengono interpretate senza ricorrere ad altre espressioni |
| **NonterminalExpression** | `BinaryExpression`, `AndExpression`, `OrExpression`, `NotExpression` | Contengono espressioni figlie, le interpretano e ne combinano il risultato |
| **Context** | `Context`, `Product` | Informazioni esterne per l'interpretazione: valori delle variabili, prodotto da verificare |
| **Client** | `ExpressionParser`, `QueryParser`, classi demo | Costruisce l'albero (a mano o con un parser) e avvia l'interpretazione |

## Schema

```text
   "prezzo * qta - sconto"
               │ ExpressionParser
               ▼
          BinaryExpression '-'                 ◄── Nonterminal
           /                \
   BinaryExpression '*'   VariableExpression "sconto"   ◄── Terminal
     /            \
 Variable "prezzo"  Variable "qta"

  tree.Interpret(context)  →  context: prezzo=250, qta=3, sconto=50  →  700
```

---

## Esempio 1. Espressioni matematiche (`MathExpressions/`)

**Grammatica:**

```text
expression := term   (('+' | '-') term)*
term       := factor (('*' | '/') factor)*
factor     := number | variable | '(' expression ')'
```

**AbstractExpression**: `IExpression.Interpret(Context)` restituisce un `double`.

**TerminalExpression** (cartella `Expressions/`):
- `NumberExpression`: restituisce il proprio numero;
- `VariableExpression`: prende il valore dal contesto in base al nome.

**NonterminalExpression**: `BinaryExpression` conserva la sotto-espressione sinistra, quella destra e l'operazione; prima interpreta i figli, poi applica l'operazione:

```csharp
public double Interpret(Context context)
{
    var l = left.Interpret(context);
    var r = right.Interpret(context);
    return operation switch { '+' => l + r, '-' => l - r, '*' => l * r, '/' => l / r, ... };
}
```

(Nella variante «da manuale» si crea una classe separata per ogni operazione, `AddExpression`, `MultiplyExpression`...; qui sono unite per non moltiplicare codice identico.)

**Context**: `Context` conserva i valori delle variabili.

**Client**:
- la demo costruisce prima l'albero `(prezzo * qta) - sconto` **a mano**;
- poi `ExpressionParser` (discesa ricorsiva sulla grammatica qui sopra) costruisce alberi a partire da stringhe. Si vede che la precedenza delle operazioni è rispettata: `2 + 3 * 4` → `(2 + (3 * 4))`;
- lo stesso albero viene interpretato con **contesti diversi**: la formula si analizza una volta e si calcola molte volte.

## Esempio 2. Query di ricerca dei prodotti (`ProductSearch/`)

**Grammatica** (precedenza: `NOT` > `AND` > `OR`):

```text
or   := and ('OR' and)*
and  := not ('AND' not)*
not  := 'NOT' not | atom
atom := word | '(' or ')'
```

**AbstractExpression**: `IQueryExpression.Matches(Product)` restituisce un `bool`.

**TerminalExpression**: `TagExpression`: vero se il prodotto ha quel tag.

**NonterminalExpression**: `AndExpression`, `OrExpression` (due figli), `NotExpression` (un figlio):

```csharp
public bool Matches(Product product) => left.Matches(product) && right.Matches(product);
```

**Context**: `Product` con un insieme di tag. Qui il contesto è l'oggetto che viene verificato.

**Client**: `QueryParser` divide la stringa in token e costruisce l'albero, la demo filtra i prodotti: `products.Where(expression.Matches)`. Per ogni query viene stampato l'albero costruito, così si vede l'ordine delle operazioni. Le parole chiave `AND`, `OR`, `NOT` restano in inglese, mentre i tag dei prodotti sono parole italiane (`portatile`, `usato`, `economico`...).

---

## Quando usarlo

- C'è un linguaggio semplice (formule, filtri, regole) le cui frasi è comodo rappresentare con un albero.
- La grammatica è **piccola** e cambia raramente.
- L'efficienza non è critica.

## Quando non serve

- La grammatica è grande: la gerarchia di classi diventerebbe enorme. Meglio i generatori di parser (ANTLR) o motori già pronti.
- Servono prestazioni elevate: interpretare un albero è più lento del codice compilato (in .NET l'albero si può compilare tramite `System.Linq.Expressions`).

## Pro e contro

**Pro**
- La grammatica è facile da estendere: una nuova regola è una nuova classe.
- Ogni regola è implementata nella propria piccola classe.
- Lo stesso albero si può interpretare più volte con contesti diversi.

**Contro**
- Difficile da mantenere quando la grammatica è grande.
- Serve un parser separato per costruire l'albero.

## Relazioni con altri pattern

- **Composite**: l'albero sintattico astratto è un composite (le espressioni terminali sono foglie, quelle non terminali contenitori).
- **Visitor**: permette di aggiungere nuove operazioni sull'albero (stampa, ottimizzazione, controllo dei tipi) senza modificare le classi delle espressioni.
- **Iterator**: per attraversare l'albero.
- **Flyweight**: le espressioni terminali (numeri e variabili uguali) possono essere condivise.
- In .NET gli esempi sono `System.Linq.Expressions` (alberi di espressioni che EF Core interpreta in SQL) e `Regex`.
