🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Prototype) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Prototype) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Prototype) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Prototype) | **Español**

# Prototype (Prototipo)

**Tipo:** patrón creacional.

**Prototype** permite crear objetos nuevos copiando otros ya existentes (prototipos), sin depender de sus clases concretas.

---

## Qué problema resuelve

Necesitamos obtener una copia de un objeto. La forma ingenua es crear un objeto de la misma clase y copiar en él todos los campos. Problemas:

- algunos campos pueden ser **privados** e inaccesibles desde fuera;
- el cliente tiene que **conocer la clase concreta** del objeto, y a menudo solo dispone de una interfaz (`IDocumentPrototype`, `Unit`);
- el objeto puede ser **costoso de configurar** (inicialización larga, carga de configuración), y repetirlo para cada copia sale caro.

## Solución

Encargar la copia al propio objeto. Todos los objetos copiables implementan una interfaz común con el método `Clone()`. El objeto conoce sus propios campos (incluidos los privados) y decide él mismo cómo copiarlos.

A menudo se añade además un **registro de prototipos**: un almacén de objetos preconfigurados del que el cliente obtiene copias por clave.

### Copia superficial y copia profunda

- **Shallow copy** (`MemberwiseClone()`) copia los campos «tal cual»: los tipos por valor se copian, pero los campos de referencia de la copia y del original apuntan a **los mismos** objetos.
- **Deep copy** copia también los objetos anidados; la copia es totalmente independiente.

Para los objetos inmutables (`string`, `record` con propiedades init) la diferencia no importa: se pueden compartir de forma segura.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Prototype** | `IDocumentPrototype`, `Unit` | Declara el método de clonación `Clone()` |
| **ConcretePrototype** | `Contract`, `Invoice`, `Archer`, `Mage` | Implementa `Clone()` copiando su propio estado |
| **Prototype Registry** | `DocumentTemplateRegistry` | Guarda prototipos configurados y entrega sus copias por clave (participante opcional) |
| **Client** | `Spawner`, `DemonstratePrototype` | Crea objetos nuevos llamando a `Clone()` del prototipo |

## Esquema

```text
 Client ───────────► «interface» IDocumentPrototype
                        Clone(): IDocumentPrototype
                              ▲              ▲
                              │              │
                          Contract        Invoice
                     Clone() => new Contract(this)

 DocumentTemplateRegistry
   _templates: clave ─► prototipo
   Create(key) => _templates[key].Clone()
```

---

## Ejemplo 1. Plantillas de documentos (`Documents/`)

**Tarea:** un sistema de gestión documental tiene plantillas (NDA, factura para la UE). Un documento nuevo se crea copiando la plantilla y después se rellenan los datos concretos.

**Prototype**: `IDocumentPrototype` con el método `Clone()`.

**ConcretePrototype**: `Contract` contiene los campos de referencia `DocumentStyle Style` y `List<string> Sections`. La copia profunda se implementa mediante un **constructor de copia privado**; así la clase tiene acceso a todos sus campos:

```csharp
private Contract(Contract source)
{
    Title = source.Title;
    Company = source.Company;
    Style = source.Style.Copy();       // copia del objeto anidado
    Sections = [.. source.Sections];   // copia de la lista
}

public IDocumentPrototype Clone() => new Contract(this);
```

Para comparar, existe `ShallowClone()` mediante `MemberwiseClone()`. En la demo se ve que, al modificar `shallow.Style.FontSize` y `shallow.Sections`, **también cambia el original**, porque comparten el mismo objeto de estilo y la misma lista. Los cambios de la copia profunda no afectan al original.

`Invoice` es el segundo prototipo, con su propio conjunto de campos y su propia implementación de `Clone()`.

**Registry**: `DocumentTemplateRegistry` guarda las plantillas por clave y en `Create(key)` siempre devuelve una **copia**, por lo que la plantilla en sí nunca se estropea:

```csharp
var ndaForAcme = (Contract)registry.Create("nda");
ndaForAcme.Company = "Acme";
```

## Ejemplo 2. Generador de unidades de juego (`GameUnits/`)

**Tarea:** en un juego hay que generar muchas unidades del mismo tipo («arquero de élite», «mago de fuego»), cuya configuración se carga desde un archivo de configuración.

**Prototype**: la clase abstracta `Unit` con `abstract Unit Clone()`.

**ConcretePrototype**: `Archer` y `Mage` (carpeta `Units/`) copian sus campos específicos; `Mage` copia la lista de hechizos para que los clones no la compartan.

**Client**: `Spawner` recibe **cualquier** `Unit` y genera instancias nuevas sin saber si es un arquero o un mago:

```csharp
public Unit Spawn(Position position)
{
    var unit = prototype.Clone();
    unit.Name = $"{prototype.Name} #{++_counter}";
    unit.Position = position;
    return unit;
}
```

`Position` es un `record`. Es inmutable, así que no hace falta copiarlo: se puede compartir entre clones. La demo también muestra el mecanismo integrado en C# para clonar records, la expresión `with`: `start with { X = 7 }`.

---

## Cuándo usarlo

- El código no debe depender de las clases concretas de los objetos que se copian.
- Crear el objeto es costoso y se necesitan muchas instancias parecidas.
- Hay un conjunto de configuraciones típicas y, en lugar de muchas subclases, resulta más cómodo guardar prototipos configurados.

## Cuándo no hace falta

- Los objetos son sencillos y baratos: es más fácil llamar al constructor.
- Los objetos son inmutables: se pueden compartir sin copiarlos.

## Ventajas y desventajas

**Ventajas**
- Clonación sin depender de clases concretas.
- Menos código de inicialización repetido.
- Una alternativa a la herencia para obtener «variaciones» de un objeto.

**Desventajas**
- Es difícil clonar objetos con grafos complejos (referencias circulares, recursos como conexiones y archivos).
- Hay que vigilar con cuidado la copia superficial/profunda.

## Relación con otros patrones

- **Abstract Factory** puede guardar un conjunto de prototipos y clonarlos en lugar de crearlos con `new`.
- **Memento**: en objetos sencillos, la instantánea del estado se puede hacer mediante clonación.
- **Composite** y **Decorator**: la clonación ayuda a copiar las estructuras complejas de estos patrones.
- El registro de prototipos suele implementarse como **Singleton**.

> En .NET existe la interfaz `ICloneable`, pero no se recomienda usarla: devuelve `object` y no indica si la copia es profunda o superficial. Es mejor una interfaz tipada propia, como `IDocumentPrototype`.
