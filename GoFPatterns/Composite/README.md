🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Composite) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Composite) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Composite) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Composite) | **Español**

# Composite (Compuesto)

**Tipo:** patrón estructural.

**Composite** permite agrupar objetos en una **estructura de árbol** y trabajar con ella como si fuera un solo objeto. El cliente trata igual a un elemento individual y a un grupo completo.

---

## Qué problema resuelve

Tenemos una jerarquía «parte — todo»: archivos y carpetas, empleados y departamentos, productos y cajas de productos, elementos de menú y submenús.

Hay que calcular, por ejemplo, el tamaño de una carpeta. Sin el patrón, el cliente se ve obligado a:
- comprobar el tipo de cada elemento (`if (item is File) ... else if (item is Folder) ...`);
- escribir él mismo el recorrido del árbol teniendo en cuenta el anidamiento;
- reescribir este código cada vez que aparece un tipo de elemento nuevo (por ejemplo, un acceso directo o un archivo comprimido).

## Solución

Introducir una **interfaz común** para los elementos simples y los compuestos. Esta interfaz tiene un método que tiene sentido para ambos (`GetSize()`):

- la **hoja** (archivo) simplemente devuelve su tamaño;
- el **contenedor** (carpeta) llama a `GetSize()` de todos sus hijos y suma el resultado.

Como los hijos también implementan la interfaz común, la recursión surge de forma natural, y el cliente llama a un solo método de la raíz sin preocuparse de lo que hay dentro.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Component** | `FileSystemItem`, `IOrgUnit` | Interfaz común para los elementos simples y compuestos |
| **Leaf** | `FileItem`, `Employee` | Elemento simple sin hijos. Realiza el trabajo real |
| **Composite** | `FolderItem`, `Department` | Contenedor. Guarda hijos (de tipo Component), les delega el trabajo y combina el resultado |
| **Client** | clases de demo | Trabaja con todos los elementos a través de Component |

## Esquema

```text
           ┌──────────────────────────┐
 Client ──►│ «abstract» FileSystemItem │◄──────────────┐
           │ + GetSize()              │                │ hijos
           │ + Print()                │                │ 0..*
           └──────────────────────────┘                │
                 ▲                ▲                    │
                 │                │                    │
          ┌──────────┐     ┌──────────────────────┐    │
          │ FileItem │     │ FolderItem           │◆───┘
          │ (Leaf)   │     │ + Add(), Remove()    │
          └──────────┘     │ GetSize() = Σ hijos  │
                           └──────────────────────┘
```

---

## Ejemplo 1. Sistema de archivos (`FileSystem/`)

**Component**: la clase abstracta `FileSystemItem` con las operaciones `GetSize()`, `Print()` y `Find()`.

**Leaf**: `FileItem` (carpeta `Items/`) conoce su tamaño y, al buscar, solo se comprueba a sí mismo:

```csharp
public override long GetSize() => size;

public override IEnumerable<FileSystemItem> Find(Func<FileSystemItem, bool> predicate) =>
    predicate(this) ? [this] : [];
```

**Composite**: `FolderItem` guarda la lista de hijos y les **delega** todas las operaciones:

```csharp
public override long GetSize() => _children.Sum(c => c.GetSize());

public override void Print(string indent = "")
{
    Console.WriteLine($"{indent}+ {Name}/ ({GetSize()} B)");
    foreach (var child in _children)
        child.Print(indent + "    ");
}
```

El método `Add` devuelve `this`, así que el árbol se puede montar cómodamente «en escalera» directamente en la demo.

**Client**: la demo llama a `GetSize()` tanto en un archivo suelto como en todo el proyecto, y el código de la llamada es el mismo. `Find(i => i.Name.EndsWith(".cs"))` encuentra recursivamente todos los archivos C# a cualquier profundidad.

### Dónde poner `Add`/`Remove`

Aquí los métodos de gestión de hijos están **solo en `FolderItem`**: es la variante «segura», porque no se puede llamar a `Add` en un archivo. El libro de GoF describe también la variante «transparente», en la que `Add`/`Remove` se declaran en Component: así el cliente no distingue en absoluto los elementos, pero la hoja tiene que lanzar una excepción en `Add`.

## Ejemplo 2. Estructura de la empresa (`OrgStructure/`)

**Tarea:** calcular la plantilla y la masa salarial de cualquier unidad, desde un solo empleado hasta toda la empresa.

**Component**: la interfaz `IOrgUnit` (`GetSalaryBudget()`, `GetHeadcount()`, `Display()`).

**Leaf**: `Employee`: plantilla 1, presupuesto igual a su salario.

**Composite**: `Department` suma los valores de todos los empleados y departamentos anidados. El departamento «Desarrollo» contiene tanto a un empleado (CTO) como a dos subdepartamentos: se pueden mezclar hojas y contenedores en un mismo nivel.

```csharp
public decimal GetSalaryBudget() => _units.Sum(u => u.GetSalaryBudget());
public int GetHeadcount() => _units.Sum(u => u.GetHeadcount());
```

**Client**: en la demo, el array `IOrgUnit[] units = [company, development, backend, new Employee(...)]` se procesa con un solo bucle, aunque contiene tanto la empresa entera como un solo becario.

---

## Cuándo usarlo

- Hay que representar una jerarquía «parte — todo» en forma de árbol.
- El cliente debe trabajar de manera uniforme con objetos simples y compuestos.
- Ejemplos reales: sistema de archivos, componentes de interfaz (contenedores y elementos), menús, DOM de XML/HTML, expresiones en una calculadora, un pedido compuesto de productos y lotes.

## Cuándo no hace falta

- La estructura es plana, sin anidamiento.
- Las hojas y los contenedores se comportan de forma demasiado distinta y la interfaz común resulta artificial.

## Ventajas y desventajas

**Ventajas**
- Simplifica el cliente: no hace falta comprobar los tipos de los elementos.
- Es fácil añadir tipos de elemento nuevos (Open/Closed).
- Las operaciones complejas sobre el árbol se escriben de forma recursiva y breve.

**Desventajas**
- La interfaz común puede volverse demasiado general: acaba incluyendo métodos que no tienen sentido para algunos elementos.
- Es difícil restringir qué elementos se pueden meter en un contenedor (por ejemplo, «en la carpeta solo archivos de cierto tipo»): harán falta comprobaciones en tiempo de ejecución.

## Relación con otros patrones

- **Builder** es cómodo para construir paso a paso árboles Composite complejos.
- **Iterator**: para recorrer un árbol Composite (véase el ejemplo `Iterator/Tree`).
- **Visitor**: para ejecutar operaciones sobre todos los nodos del árbol sin modificar sus clases.
- **Chain of Responsibility** suele funcionar sobre un Composite: la petición sube desde la hoja hacia los padres.
- **Flyweight**: las hojas del árbol pueden ser flyweights para ahorrar memoria.
- **Decorator** se le parece estructuralmente (composición recursiva), pero el decorador tiene un solo objeto anidado y su objetivo es añadir comportamiento, mientras que el compuesto tiene muchos hijos y su objetivo es combinar el resultado.
