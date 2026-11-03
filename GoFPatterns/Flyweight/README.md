🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Flyweight) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Flyweight) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Flyweight) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Flyweight) | **Español**

# Flyweight (Peso ligero)

**Tipo:** patrón estructural.

**Flyweight** permite meter en memoria una gran cantidad de objetos **compartiendo el estado común** entre ellos en lugar de guardar los mismos datos en cada objeto.

---

## Qué problema resuelve

En un juego hay que dibujar un bosque de 100 000 árboles. Cada árbol es un objeto con coordenadas, nombre de la especie, color y **textura** (pongamos, 1 MB). Si cada árbol guarda su propia copia de la textura, se necesitan ~100 GB de memoria, aunque solo hay cuatro especies distintas.

Lo mismo ocurre en un editor de texto: cada carácter tiene fuente, tamaño, color y negrita. Hay miles de caracteres, pero solo unos pocos estilos únicos.

El problema es que **la mayor parte de los datos se repite** en muchos objetos.

## Solución

Dividir el estado del objeto en dos partes:

- **Intrínseco (intrinsic)**: igual para muchos objetos e independiente del contexto: especie, color, textura; fuente, tamaño. Se traslada a un objeto **flyweight**, que se crea **una sola vez** y que todos comparten.
- **Extrínseco (extrinsic)**: único para cada objeto: las coordenadas del árbol, el carácter y su posición en el texto. Lo guarda el **contexto** (o el cliente) y se lo pasa al flyweight como parámetro de los métodos.

Los flyweights deben ser **inmutables**, ya que un mismo objeto lo usan miles de contextos. Para no crear duplicados, los flyweights los entrega una **fábrica** con un pool (caché).

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Flyweight** | `TreeType`, `CharacterStyle` | Guarda el estado intrínseco (compartido). Es inmutable. Recibe el estado extrínseco a través de parámetros |
| **FlyweightFactory** | `TreeTypeFactory`, `StyleFactory` | Pool de flyweights: devuelve uno existente o crea uno nuevo |
| **Context** | `Tree`, `FormattedCharacter` | Guarda el estado extrínseco + una referencia al flyweight. Hay muchos objetos de este tipo, pero son pequeños |
| **Client** | `ForestMap`, `TextDocument` | Crea los contextos obteniendo los flyweights solo a través de la fábrica |

## Esquema

```text
 ForestMap (Client)
   │ PlantTree(x, y, "Roble", ...)
   ▼
 TreeTypeFactory ── _types: "Roble|verde oscuro|oak.png" ─► TreeType ◄──┐
                                                           (1 MB)        │
                                                                         │ compartido
   Tree(x=10, y=20) ─────────────────────────────────────────────────────┤
   Tree(x=55, y=13) ─────────────────────────────────────────────────────┤
   Tree(x=91, y=77) ─────────────────────────────────────────────────────┘
   ... 100 000 contextos pequeños, 4 flyweights
```

---

## Ejemplo 1. Bosque (`Forest/`)

**Flyweight**: `TreeType`. Guarda el nombre, el color, la ruta de la textura y los propios datos «pesados» de la textura (un array de 1 MB). La clase es `sealed` y todas sus propiedades son de solo lectura. Las coordenadas **no se guardan** en ella: llegan como parámetros del método:

```csharp
public string Draw(int x, int y) => $"{Name} ({Color}) en [{x}, {y}] usando {TexturePath}";
```

**FlyweightFactory**: `TreeTypeFactory` guarda un diccionario `clave → TreeType`. Solo se crea un objeto nuevo si todavía no existe (en la demo se ve que el mensaje «creado un nuevo tipo de árbol» se imprime exactamente 4 veces para 100 000 árboles):

```csharp
if (!_types.TryGetValue(key, out var type))
{
    type = new TreeType(name, color, texturePath);
    _types[key] = type;
}
return type;
```

**Context**: `Tree`: dos `int` y una referencia a `TreeType`. El método `Draw()` pasa sus coordenadas al flyweight.

**Client**: `ForestMap` planta los árboles a través de la fábrica y nunca crea un `TreeType` por sí mismo.

La demo calcula el volumen aproximado de memoria: sin flyweight, ~100 GB; con flyweight, ~5,5 MB (4 texturas + 100 000 contextos pequeños).

## Ejemplo 2. Formato de texto (`TextFormatting/`)

**Flyweight**: `CharacterStyle`, un `sealed record` (fuente, tamaño, negrita, color). Un record es inmutable y se compara por valor, por lo que resulta cómodo usarlo como clave de diccionario.

**FlyweightFactory**: `StyleFactory` guarda un pool `Dictionary<CharacterStyle, CharacterStyle>`: si ya existe un estilo igual por valor, se devuelve **el mismo objeto**; si no, el candidato se añade al pool.

**Context**: `FormattedCharacter`, un `readonly record struct` con el carácter, la posición y una referencia al estilo.

**Client**: `TextDocument.Append(...)` obtiene el estilo de la fábrica y crea contextos para cada carácter. La demo muestra que para todos los caracteres del texto solo hay 3 objetos de estilo, y `CountDistinctStyleObjects()` lo comprueba comparando **por referencia** (`ReferenceEqualityComparer`).

Al final se muestra el flyweight integrado en .NET, el **internado de cadenas**: `string.Intern` devuelve el mismo objeto para cadenas iguales. Los literales de cadena de C# se internan automáticamente.

---

## Cuándo usarlo

- El programa tiene **muchísimos** objetos del mismo tipo y no caben en memoria.
- La mayor parte del estado de esos objetos se puede sacar fuera o compartir.
- Los objetos se pueden hacer inmutables.
- Ejemplos: partículas y tiles en juegos, caracteres en editores, marcadores en un mapa, caché de iconos.

## Cuándo no hace falta

- Hay pocos objetos: el ahorro no compensa la complicación.
- El estado de los objetos es casi totalmente único: no hay nada que compartir.

## Ventajas y desventajas

**Ventajas**
- Ahorro considerable de memoria.

**Desventajas**
- Se gasta tiempo de CPU en buscar en el pool y en pasar el estado extrínseco.
- El código se complica: el estado queda «repartido» entre el contexto y el flyweight.
- Los flyweights tienen que ser inmutables; de lo contrario, cambiar uno afectaría a todos los contextos.

## Relación con otros patrones

- **Composite**: las hojas del árbol a menudo se implementan como flyweights (por ejemplo, los caracteres de un documento).
- **Singleton** se le parece en que el objeto se comparte, pero un singleton es una instancia por clase, mientras que flyweights hay muchos (uno por cada estado intrínseco único) y son inmutables.
- **Factory Method** / pool de objetos: la fábrica de flyweights es, en esencia, una caché.
- **State** y **Strategy**: los objetos de estado y de estrategia sin campos propios suelen implementarse como flyweights.
