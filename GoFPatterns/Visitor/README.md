🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Visitor) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Visitor) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Visitor) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Visitor)

# Visitor (Visitatore)

**Tipo:** pattern comportamentale.

**Visitor** permette di **aggiungere nuove operazioni** a una gerarchia di classi **senza modificare le classi stesse**. L'operazione viene spostata in un oggetto visitor separato, che «visita» gli elementi ed esegue su ciascuno l'azione richiesta.

---

## Quale problema risolve

C'è una gerarchia stabile di forme: `Circle`, `Rectangle`. Su di esse bisogna eseguire operazioni sempre nuove: calcolare l'area, esportare in XML, poi in JSON, disegnare, calcolare il perimetro...

Se ogni operazione si aggiunge come metodo nelle classi delle forme:
- le classi delle forme **si gonfiano** di codice che non le riguarda (l'esportazione in XML non è una responsabilità del cerchio);
- ogni nuova operazione richiede di modificare **tutte** le classi della gerarchia;
- a volte le classi non si possono proprio modificare (codice di terze parti).

Se l'operazione si scrive dall'esterno con dei controlli sul tipo:

```csharp
foreach (var shape in shapes)
{
    if (shape is Circle c) ...
    else if (shape is Rectangle r) ...
}
```

…quando si aggiunge una nuova forma è facile **dimenticare** di aggiornare una di queste catene, e il compilatore non lo segnala.

## Soluzione

1. Spostare ogni operazione in una classe separata, il **visitor**, che ha un metodo per ogni tipo di elemento: `VisitCircle`, `VisitRectangle`.
2. Aggiungere agli elementi **un solo** metodo `Accept(visitor)`, che chiama sul visitor il «proprio» metodo: il cerchio chiama `VisitCircle(this)`, il rettangolo `VisitRectangle(this)`.

Questa tecnica si chiama **double dispatch**: il codice giusto viene scelto in base a **due** tipi, il tipo dell'elemento (tramite l'`Accept` virtuale) e il tipo del visitor (tramite il `Visit…` virtuale). Senza controlli sul tipo né cast.

Una nuova operazione è una nuova classe visitor. Le classi delle forme non cambiano più.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Visitor** | `IShapeVisitor` | Dichiara un metodo di visita per ogni tipo di elemento |
| **ConcreteVisitor** | `AreaCalculatorVisitor`, `XmlExportVisitor` | Implementa un'unica operazione per tutti i tipi di elementi. Può accumulare un risultato |
| **Element** | `IShape` | Dichiara il metodo `Accept(IShapeVisitor)` |
| **ConcreteElement** | `Circle`, `Rectangle` | Implementa `Accept` chiamando il metodo corrispondente del visitor |
| **Client / ObjectStructure** | `ShapeDemostrateVisitor` (la lista `List<IShape>`) | Conserva la collezione di elementi e vi «accompagna» il visitor |

## Schema

```text
 «interface» IShape                    «interface» IShapeVisitor
   Accept(IShapeVisitor v)               VisitCircle(Circle)
      ▲              ▲                   VisitRectangle(Rectangle)
      │              │                        ▲                 ▲
   Circle        Rectangle                    │                 │
   Accept(v) =>  Accept(v) =>      AreaCalculatorVisitor   XmlExportVisitor
   v.VisitCircle(this)             TotalArea               GetXml()
                 v.VisitRectangle(this)

 shape.Accept(visitor)  ──►  visitor.VisitCircle(circle)     ← double dispatch
```

---

## Esempio. Forme: area ed esportazione XML (`Shapes/`)

**Element**: `IShape`:

```csharp
public interface IShape
{
    void Accept(IShapeVisitor visitor);
}
```

**ConcreteElement** (cartella `VisitShapes/`): `Circle` (raggio) e `Rectangle` (larghezza, altezza) conservano solo i propri dati (proprietà `required init`) e implementano `Accept`:

```csharp
public class Circle : IShape
{
    public required double Radius { get; init; }

    public void Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
}
```

Notate: nelle forme **non c'è** né il calcolo dell'area né l'esportazione.

**Visitor**: `IShapeVisitor` con i metodi `VisitCircle(Circle)` e `VisitRectangle(Rectangle)`.

**ConcreteVisitor** (cartella `Visiotrs/`):

`AreaCalculatorVisitor`: accumula l'area totale di tutte le forme visitate:

```csharp
public void VisitCircle(Circle circle) => TotalArea += Math.PI * circle.Radius * circle.Radius;
public void VisitRectangle(Rectangle rectangle) => TotalArea += rectangle.Width * rectangle.Height;
```

`XmlExportVisitor`: assembla la rappresentazione XML in uno `StringBuilder` e la restituisce tramite `GetXml()`.

Entrambi i visitor **conservano uno stato** tra una visita e l'altra: è comodo quando il risultato viene raccolto sull'intera collezione.

**Client**: `ShapeDemostrateVisitor` crea una lista di forme e due visitor, poi scorre le forme e fa «accettare» a ciascuna entrambi i visitor:

```csharp
foreach (var shape in shapes)
{
    shape.Accept(areaVisitor);
    shape.Accept(xmlVisitor);
}
Console.WriteLine($"Area totale: {areaVisitor.TotalArea}");
Console.WriteLine(xmlVisitor.GetXml());
```

Per aggiungere, ad esempio, un `JsonExportVisitor` o un `PerimeterVisitor` serve una sola nuova classe: `Circle` e `Rectangle` non cambiano.

---

## Il prezzo della flessibilità

Visitor rende facile aggiungere **operazioni**, ma difficile aggiungere **elementi**. Se si aggiunge `Triangle`, bisogna aggiungere `VisitTriangle` a `IShapeVisitor` e implementarlo in **tutti** i visitor. In compenso è il compilatore stesso a indicare tutti i punti da aggiornare, a differenza delle catene `if (shape is ...)`.

Per questo il pattern è adatto quando la **gerarchia degli elementi è stabile** e le **operazioni vengono aggiunte spesso**.

> Nel C# moderno un risultato simile si ottiene a volte con un'espressione `switch` sui tipi (`shape switch { Circle c => ..., Rectangle r => ... }`). È più conciso, ma il compilatore non garantisce che tutti i tipi siano gestiti (se la gerarchia non è chiusa). Visitor dà questa garanzia.

## Quando usarlo

- Su oggetti con una struttura complessa (albero, AST, documento) bisogna eseguire molte operazioni diverse e non correlate.
- Non si vogliono «sporcare» le classi degli elementi con logica estranea.
- La gerarchia degli elementi è stabile, mentre le operazioni vengono aggiunte spesso.
- Esempi: compilatori e analizzatori di codice (Roslyn, `CSharpSyntaxVisitor`), `ExpressionVisitor` in `System.Linq.Expressions`, esportazione di documenti in formati diversi, calcoli sulla struttura di un ordine o di un magazzino.

## Quando non serve

- La gerarchia degli elementi cambia spesso: ogni nuova classe richiederebbe di modificare tutti i visitor.
- Le operazioni sono poche e appartengono naturalmente agli elementi stessi.

## Pro e contro

**Pro**
- Nuove operazioni senza modificare le classi degli elementi (Open/Closed).
- Le operazioni correlate sono raccolte in un'unica classe (SRP).
- Il visitor può accumulare stato durante l'attraversamento della struttura.
- Nessun controllo sul tipo né cast.

**Contro**
- È difficile aggiungere nuovi tipi di elementi.
- Il visitor potrebbe aver bisogno di accedere ai dettagli interni degli elementi, il che costringe a rendere pubblici i loro campi.

## Relazioni con altri pattern

- **Composite**: il visitor attraversa spesso l'albero di un composite, eseguendo un'operazione su tutti i nodi.
- **Iterator**: l'iterator attraversa la struttura, il visitor esegue un'azione su ogni elemento.
- **Interpreter**: i visitor aggiungono nuove operazioni sull'albero sintattico (stampa, ottimizzazione, verifica) senza modificare le classi delle espressioni.
- **Command**: un visitor si può considerare un comando esteso, capace di lavorare con tipi di oggetti diversi.
