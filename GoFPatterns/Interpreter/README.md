🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Interpreter) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Interpreter) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Interpreter) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Interpreter)

# Interpreter (Interpreter)

**Typ:** Verhaltensmuster.

**Interpreter** definiert die **Grammatik einer einfachen Sprache** als Klassenhierarchie und interpretiert Sätze dieser Sprache. Jede Grammatikregel ist eine eigene Klasse, und ein Satz der Sprache wird als **Baum** aus Objekten dieser Klassen dargestellt (abstrakter Syntaxbaum, AST).

---

## Welches Problem wird gelöst

Manchmal wiederholt sich dieselbe Aufgabe in verschiedenen Varianten, die sich bequemer in einer **kleinen Sprache** als in Code beschreiben lassen:

- vom Benutzer konfigurierbare Formeln zur Preis-/Gehaltsberechnung: `price * count * (1 - tax)`;
- Suchanfragen und Filter: `(laptop OR tablet) AND gebraucht`;
- Geschäftsregeln, Vorlagen, reguläre Ausdrücke.

Zerlegt man solche Zeichenketten ad hoc (`Split`, `IndexOf`, `if`), wird der Code schnell zu unlesbarem Durcheinander, und eine neue Operation (z. B. `NOT` oder Klammern) hinzuzufügen wird schwierig.

## Lösung

1. Die **Grammatik** der Sprache beschreiben.
2. Für jede Grammatikregel eine Klasse mit der Methode `Interpret(context)` anlegen:
   - **terminale** Ausdrücke – die „Blätter“ (Zahl, Variable, Wort);
   - **nichtterminale** – enthalten andere Ausdrücke und verknüpfen deren Ergebnisse (Addition, `AND`, `OR`).
3. Ein Satz der Sprache wird zu einem Baum aus diesen Objekten, und der Aufruf von `Interpret` an der Wurzel berechnet das Ergebnis rekursiv.

Das Muster selbst **beschreibt nicht, wie der Baum aufgebaut wird**: Das kann der Programmierer von Hand tun oder ein eigener Parser (in den Beispielen gibt es beide Varianten).

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **AbstractExpression** | `IExpression`, `IQueryExpression` | Gemeinsame Schnittstelle der Baumknoten mit der Interpretationsmethode |
| **TerminalExpression** | `NumberExpression`, `VariableExpression`, `TagExpression` | Blätter: werden ohne Rückgriff auf andere Ausdrücke interpretiert |
| **NonterminalExpression** | `BinaryExpression`, `AndExpression`, `OrExpression`, `NotExpression` | Enthalten Kindausdrücke, interpretieren sie und verknüpfen das Ergebnis |
| **Context** | `Context`, `Product` | Externe Informationen für die Interpretation: Variablenwerte, das geprüfte Produkt |
| **Client** | `ExpressionParser`, `QueryParser`, Demo-Klassen | Baut den Baum (von Hand oder per Parser) und ruft die Interpretation auf |

## Schema

```text
   "price * count - discount"
               │ ExpressionParser
               ▼
          BinaryExpression '-'                 ◄── Nichtterminal
           /                \
   BinaryExpression '*'   VariableExpression "discount"   ◄── Terminal
     /            \
 Variable "price"  Variable "count"

  tree.Interpret(context)  →  Kontext: price=250, count=3, discount=50  →  700
```

---

## Beispiel 1. Mathematische Ausdrücke (`MathExpressions/`)

**Grammatik:**

```text
expression := term   (('+' | '-') term)*
term       := factor (('*' | '/') factor)*
factor     := number | variable | '(' expression ')'
```

**AbstractExpression** – `IExpression.Interpret(Context)` gibt `double` zurück.

**TerminalExpression** (Ordner `Expressions/`):
- `NumberExpression` – gibt seine Zahl zurück;
- `VariableExpression` – holt den Wert per Namen aus dem Kontext.

**NonterminalExpression** – `BinaryExpression` hält den linken und rechten Teilausdruck sowie die Operation; interpretiert zuerst die Kinder und wendet dann die Operation an:

```csharp
public double Interpret(Context context)
{
    var l = left.Interpret(context);
    var r = right.Interpret(context);
    return operation switch { '+' => l + r, '-' => l - r, '*' => l * r, '/' => l / r, ... };
}
```

(In der „Lehrbuch“-Variante legt man für jede Operation eine eigene Klasse an – `AddExpression`, `MultiplyExpression`... –, hier sind sie zusammengefasst, um keinen gleichartigen Code zu vervielfachen.)

**Context** – `Context` speichert die Werte der Variablen.

**Client**:
- die Demo baut zuerst den Baum `(price * count) - discount` **von Hand** auf;
- danach baut `ExpressionParser` (rekursiver Abstieg nach der obigen Grammatik) Bäume aus Zeichenketten. Man sieht, dass die Operatorpriorität berücksichtigt wird: `2 + 3 * 4` → `(2 + (3 * 4))`;
- derselbe Baum wird mit **verschiedenen Kontexten** interpretiert – eine Formel kann einmal geparst und viele Male berechnet werden.

## Beispiel 2. Suchanfragen für Produkte (`ProductSearch/`)

**Grammatik** (Priorität: `NOT` > `AND` > `OR`):

```text
or   := and ('OR' and)*
and  := not ('AND' not)*
not  := 'NOT' not | atom
atom := word | '(' or ')'
```

**AbstractExpression** – `IQueryExpression.Matches(Product)` gibt `bool` zurück.

**TerminalExpression** – `TagExpression`: wahr, wenn das Produkt dieses Tag hat.

**NonterminalExpression** – `AndExpression`, `OrExpression` (zwei Kinder), `NotExpression` (ein Kind):

```csharp
public bool Matches(Product product) => left.Matches(product) && right.Matches(product);
```

**Context** – `Product` mit einer Menge von Tags. Hier ist der Kontext das Objekt, das geprüft wird.

**Client** – `QueryParser` zerlegt die Zeichenkette in Tokens und baut den Baum, die Demo filtert die Produkte: `products.Where(expression.Matches)`. Für jede Anfrage wird der erzeugte Baum ausgegeben, damit die Reihenfolge der Operationen sichtbar ist.

---

## Wann einsetzen

- Es gibt eine einfache Sprache (Formeln, Filter, Regeln), deren Sätze sich gut als Baum darstellen lassen.
- Die Grammatik ist **klein** und ändert sich selten.
- Effizienz ist nicht kritisch.

## Wann nicht nötig

- Die Grammatik ist groß – die Klassenhierarchie würde riesig. Besser sind Parsergeneratoren (ANTLR) oder fertige Engines.
- Hohe Performance ist gefragt – die Interpretation eines Baums ist langsamer als kompilierter Code (in .NET lässt sich ein Baum über `System.Linq.Expressions` kompilieren).

## Vor- und Nachteile

**Vorteile**
- Die Grammatik lässt sich leicht erweitern: neue Regel – neue Klasse.
- Jede Regel ist in einer eigenen kleinen Klasse umgesetzt.
- Ein Baum kann mehrfach mit verschiedenen Kontexten interpretiert werden.

**Nachteile**
- Bei großer Grammatik schwer zu pflegen.
- Für den Aufbau des Baums wird ein eigener Parser benötigt.

## Beziehung zu anderen Mustern

- **Composite** – ein abstrakter Syntaxbaum ist ein Kompositum (terminale Ausdrücke sind Blätter, nichtterminale sind Container).
- **Visitor** – ermöglicht es, neue Operationen auf dem Baum hinzuzufügen (Ausgabe, Optimierung, Typprüfung), ohne die Ausdrucksklassen zu ändern.
- **Iterator** – zum Durchlaufen des Baums.
- **Flyweight** – terminale Ausdrücke (gleiche Zahlen, Variablen) können geteilt werden.
- Beispiele in .NET sind `System.Linq.Expressions` (Ausdrucksbäume, die EF Core in SQL übersetzt) und `Regex`.
