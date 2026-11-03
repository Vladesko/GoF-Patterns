🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Interpreter) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Interpreter) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Interpreter) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Interpreter) | **Español**

# Interpreter (Intérprete)

**Tipo:** patrón de comportamiento.

**Interpreter** define la **gramática de un lenguaje sencillo** como una jerarquía de clases e interpreta las frases de ese lenguaje. Cada regla de la gramática es una clase aparte, y una frase del lenguaje se representa como un **árbol** de objetos de esas clases (árbol de sintaxis abstracta, AST).

---

## Qué problema resuelve

A veces una misma tarea se repite en distintas variantes que resulta cómodo describir con un **pequeño lenguaje** en lugar de con código:

- fórmulas de cálculo de precios/salarios que configura el usuario: `price * count * (1 - tax)`;
- consultas de búsqueda y filtros: `(laptop OR tablet) AND used`;
- reglas de negocio, plantillas, expresiones regulares.

Si estas cadenas se analizan de forma improvisada (`Split`, `IndexOf`, `if`), el código se convierte rápidamente en un batiburrillo ilegible, y añadir una operación nueva (por ejemplo, `NOT` o paréntesis) se vuelve difícil.

## Solución

1. Describir la **gramática** del lenguaje.
2. Para cada regla de la gramática, crear una clase con el método `Interpret(context)`:
   - las expresiones **terminales** son las «hojas» (número, variable, palabra);
   - las **no terminales** contienen otras expresiones y combinan sus resultados (suma, `AND`, `OR`).
3. Una frase del lenguaje se convierte en un árbol de estos objetos, y la llamada a `Interpret` en la raíz calcula el resultado de forma recursiva.

El patrón en sí **no describe cómo construir el árbol**: puede hacerlo el programador a mano o un parser aparte (en los ejemplos aparecen ambas variantes).

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **AbstractExpression** | `IExpression`, `IQueryExpression` | Interfaz común de los nodos del árbol con el método de interpretación |
| **TerminalExpression** | `NumberExpression`, `VariableExpression`, `TagExpression` | Hojas: se interpretan sin recurrir a otras expresiones |
| **NonterminalExpression** | `BinaryExpression`, `AndExpression`, `OrExpression`, `NotExpression` | Contienen expresiones hijas, las interpretan y combinan el resultado |
| **Context** | `Context`, `Product` | Información externa para la interpretación: valores de las variables, el producto que se comprueba |
| **Client** | `ExpressionParser`, `QueryParser`, clases de demo | Construye el árbol (a mano o con un parser) y llama a la interpretación |

## Esquema

```text
   "price * count - discount"
               │ ExpressionParser
               ▼
          BinaryExpression '-'                 ◄── No terminal
           /                \
   BinaryExpression '*'   VariableExpression "discount"   ◄── Terminal
     /            \
 Variable "price"  Variable "count"

  tree.Interpret(context)  →  context: price=250, count=3, discount=50  →  700
```

---

## Ejemplo 1. Expresiones matemáticas (`MathExpressions/`)

**Gramática:**

```text
expression := term   (('+' | '-') term)*
term       := factor (('*' | '/') factor)*
factor     := number | variable | '(' expression ')'
```

**AbstractExpression**: `IExpression.Interpret(Context)` devuelve un `double`.

**TerminalExpression** (carpeta `Expressions/`):
- `NumberExpression`: devuelve su número;
- `VariableExpression`: toma el valor del contexto por su nombre.

**NonterminalExpression**: `BinaryExpression` guarda las subexpresiones izquierda y derecha y la operación; primero interpreta a los hijos y después aplica la operación:

```csharp
public double Interpret(Context context)
{
    var l = left.Interpret(context);
    var r = right.Interpret(context);
    return operation switch { '+' => l + r, '-' => l - r, '*' => l * r, '/' => l / r, ... };
}
```

(En la variante «de libro» se crea una clase aparte para cada operación, `AddExpression`, `MultiplyExpression`...; aquí están unificadas para no multiplicar código idéntico.)

**Context**: `Context` guarda los valores de las variables.

**Client**:
- la demo primero construye **a mano** el árbol `(price * count) - discount`;
- después `ExpressionParser` (descenso recursivo según la gramática anterior) construye árboles a partir de cadenas. Se ve que se respeta la precedencia de los operadores: `2 + 3 * 4` → `(2 + (3 * 4))`;
- el mismo árbol se interpreta con **distintos contextos**: la fórmula se analiza una vez y se puede evaluar muchas veces.

## Ejemplo 2. Consultas de búsqueda de productos (`ProductSearch/`)

**Gramática** (precedencia: `NOT` > `AND` > `OR`):

```text
or   := and ('OR' and)*
and  := not ('AND' not)*
not  := 'NOT' not | atom
atom := word | '(' or ')'
```

**AbstractExpression**: `IQueryExpression.Matches(Product)` devuelve un `bool`.

**TerminalExpression**: `TagExpression`: verdadero si el producto tiene esa etiqueta.

**NonterminalExpression**: `AndExpression`, `OrExpression` (dos hijos) y `NotExpression` (un hijo):

```csharp
public bool Matches(Product product) => left.Matches(product) && right.Matches(product);
```

**Context**: `Product` con un conjunto de etiquetas. Aquí el contexto es el objeto que se comprueba.

**Client**: `QueryParser` divide la cadena en tokens y construye el árbol; la demo filtra los productos: `products.Where(expression.Matches)`. Para cada consulta se muestra el árbol construido, para que se vea el orden de las operaciones. (Las etiquetas y las palabras clave `AND`/`OR`/`NOT` se mantienen en inglés porque forman parte de los datos y de la sintaxis del lenguaje de consultas.)

---

## Cuándo usarlo

- Hay un lenguaje sencillo (fórmulas, filtros, reglas) cuyas frases conviene representar como un árbol.
- La gramática es **pequeña** y cambia con poca frecuencia.
- La eficiencia no es crítica.

## Cuándo no hace falta

- La gramática es grande: la jerarquía de clases sería enorme. Es mejor usar generadores de parsers (ANTLR) o motores ya hechos.
- Se necesita un alto rendimiento: interpretar un árbol es más lento que el código compilado (en .NET el árbol se puede compilar con `System.Linq.Expressions`).

## Ventajas y desventajas

**Ventajas**
- La gramática es fácil de ampliar: una regla nueva es una clase nueva.
- Cada regla está implementada en su propia clase pequeña.
- Un mismo árbol se puede interpretar muchas veces con distintos contextos.

**Desventajas**
- Difícil de mantener cuando la gramática es grande.
- Se necesita un parser aparte para construir el árbol.

## Relación con otros patrones

- **Composite**: el árbol de sintaxis abstracta es un compuesto (las expresiones terminales son las hojas y las no terminales, los contenedores).
- **Visitor**: permite añadir operaciones nuevas sobre el árbol (impresión, optimización, comprobación de tipos) sin modificar las clases de las expresiones.
- **Iterator**: para recorrer el árbol.
- **Flyweight**: las expresiones terminales (números o variables iguales) se pueden compartir.
- Ejemplos en .NET: `System.Linq.Expressions` (árboles de expresiones que EF Core interpreta como SQL) y `Regex`.
