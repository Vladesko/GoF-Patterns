🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Interpreter) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Interpreter) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Interpreter) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Interpreter)

# Interpreter

**Type:** behavioral pattern.

**Interpreter** defines the **grammar of a simple language** as a class hierarchy and interprets sentences of that language. Each grammar rule is a separate class, and a sentence of the language is represented as a **tree** of objects of these classes (an abstract syntax tree, AST).

---

## The problem

Sometimes the same task recurs in different variants that are convenient to describe with a **small language** rather than with code:

- price/salary formulas configured by the user: `price * count * (1 - tax)`;
- search queries and filters: `(laptop OR tablet) AND used`;
- business rules, templates, regular expressions.

If such strings are parsed ad hoc (`Split`, `IndexOf`, `if`), the code quickly turns into an unreadable mess, and adding a new operation (e.g. `NOT` or parentheses) becomes hard.

## Solution

1. Describe the **grammar** of the language.
2. For each grammar rule, create a class with an `Interpret(context)` method:
   - **terminal** expressions are the "leaves" (a number, a variable, a word);
   - **nonterminal** expressions contain other expressions and combine their results (addition, `AND`, `OR`).
3. A sentence of the language is turned into a tree of these objects, and calling `Interpret` on the root computes the result recursively.

The pattern itself **does not describe how to build the tree**: it can be done manually by the programmer or by a separate parser (the examples include both options).

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **AbstractExpression** | `IExpression`, `IQueryExpression` | The common interface of tree nodes with an interpretation method |
| **TerminalExpression** | `NumberExpression`, `VariableExpression`, `TagExpression` | Leaves: interpreted without referring to other expressions |
| **NonterminalExpression** | `BinaryExpression`, `AndExpression`, `OrExpression`, `NotExpression` | Contain child expressions, interpret them and combine the result |
| **Context** | `Context`, `Product` | External information for interpretation: variable values, the product being checked |
| **Client** | `ExpressionParser`, `QueryParser`, demo classes | Builds the tree (manually or with a parser) and invokes interpretation |

## Diagram

```text
   "price * count - discount"
               │ ExpressionParser
               ▼
          BinaryExpression '-'                 ◄── Nonterminal
           /                \
   BinaryExpression '*'   VariableExpression "discount"   ◄── Terminal
     /            \
 Variable "price"  Variable "count"

  tree.Interpret(context)  →  context: price=250, count=3, discount=50  →  700
```

---

## Example 1. Mathematical expressions (`MathExpressions/`)

**Grammar:**

```text
expression := term   (('+' | '-') term)*
term       := factor (('*' | '/') factor)*
factor     := number | variable | '(' expression ')'
```

**AbstractExpression** — `IExpression.Interpret(Context)` returns a `double`.

**TerminalExpression** (folder `Expressions/`):
- `NumberExpression` — returns its number;
- `VariableExpression` — takes the value from the context by name.

**NonterminalExpression** — `BinaryExpression` stores the left and right subexpressions and the operation; it first interprets the children, then applies the operation:

```csharp
public double Interpret(Context context)
{
    var l = left.Interpret(context);
    var r = right.Interpret(context);
    return operation switch { '+' => l + r, '-' => l - r, '*' => l * r, '/' => l / r, ... };
}
```

(In the "textbook" variant, each operation gets its own class — `AddExpression`, `MultiplyExpression`... — here they are combined to avoid duplicating identical code.)

**Context** — `Context` stores the variable values.

**Client**:
- the demo first builds the tree `(price * count) - discount` **manually**;
- then `ExpressionParser` (recursive descent over the grammar above) builds trees from strings. You can see that operator precedence is respected: `2 + 3 * 4` → `(2 + (3 * 4))`;
- the same tree is interpreted with **different contexts** — a formula can be parsed once and evaluated many times.

## Example 2. Product search queries (`ProductSearch/`)

**Grammar** (precedence: `NOT` > `AND` > `OR`):

```text
or   := and ('OR' and)*
and  := not ('AND' not)*
not  := 'NOT' not | atom
atom := word | '(' or ')'
```

**AbstractExpression** — `IQueryExpression.Matches(Product)` returns a `bool`.

**TerminalExpression** — `TagExpression`: true if the product has that tag.

**NonterminalExpression** — `AndExpression`, `OrExpression` (two children), `NotExpression` (one child):

```csharp
public bool Matches(Product product) => left.Matches(product) && right.Matches(product);
```

**Context** — `Product` with a set of tags. Here the context is the object being checked.

**Client** — `QueryParser` splits the string into tokens and builds the tree; the demo filters products: `products.Where(expression.Matches)`. For each query the built tree is printed, so the order of operations is visible.

---

## When to use

- There is a simple language (formulas, filters, rules) whose sentences are convenient to represent as a tree.
- The grammar is **small** and changes rarely.
- Efficiency is not critical.

## When not to use

- The grammar is large — the class hierarchy becomes huge. Parser generators (ANTLR) or ready-made engines are better.
- High performance is required — interpreting a tree is slower than compiled code (in .NET a tree can be compiled via `System.Linq.Expressions`).

## Pros and cons

**Pros**
- The grammar is easy to extend: a new rule is a new class.
- Each rule is implemented in its own small class.
- One tree can be interpreted many times with different contexts.

**Cons**
- Hard to maintain with a large grammar.
- A separate parser is needed to build the tree.

## Relations with other patterns

- **Composite** — an abstract syntax tree is a composite (terminal expressions are leaves, nonterminal ones are containers).
- **Visitor** — lets you add new operations on the tree (printing, optimization, type checking) without changing the expression classes.
- **Iterator** — for traversing the tree.
- **Flyweight** — terminal expressions (identical numbers, variables) can be shared.
- Examples in .NET are `System.Linq.Expressions` (expression trees that EF Core translates into SQL) and `Regex`.
