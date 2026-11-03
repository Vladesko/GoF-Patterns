🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/TemplateMethod) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/TemplateMethod) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/TemplateMethod) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/TemplateMethod) | **Español**

# Template Method (Método plantilla)

**Tipo:** patrón de comportamiento.

**Template Method** define el **esqueleto de un algoritmo** en un método de la clase base y delega en las subclases la implementación de algunos pasos. Las subclases pueden sobrescribir los pasos, pero **no pueden cambiar la estructura** del algoritmo.

---

## Qué problema resuelve

Hay que importar clientes desde CSV y desde JSON. En ambos casos el proceso es el mismo: preparar los datos → analizarlos → validarlos → guardarlos → (hacer algo después de la importación). Solo cambia el paso del **análisis**.

Si se escriben dos clases independientes, `CsvImporter` y `JsonImporter`:
- la validación y el guardado se **duplican**;
- se corrige un error de validación en una clase y se olvida en la otra;
- si aparece XML, habrá que copiarlo todo por tercera vez;
- nada garantiza que todos los importadores ejecuten los pasos en el mismo orden.

## Solución

Sacar el algoritmo común a la **clase base** en forma de un método (el método plantilla) que llama a los pasos en orden. Hay tres tipos de pasos:

- **abstractos**: cada subclase está obligada a implementarlos (`Parse`);
- **comunes**: implementados en la clase base e iguales para todas (`Save`);
- **hooks**: métodos `virtual` con una implementación por defecto (a menudo vacía) que la subclase **puede** sobrescribir para «engancharse» al algoritmo (`Preprocess`, `AfterImport`).

El método plantilla en sí **no es virtual**: las subclases no pueden cambiar el orden de los pasos.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **AbstractClass** | `CustomerImporter`, `HotBeverage` | Contiene el método plantilla (`Import`, `Prepare`) y declara los pasos: abstractos, comunes y hooks |
| **ConcreteClass** | `CsvCustomerImporter`, `JsonCustomerImporter`, `PartnerCsvCustomerImporter`, `Tea`, `GreenTea`, `Coffee` | Implementa los pasos abstractos y, si hace falta, sobrescribe los hooks. No toca el algoritmo |

## Esquema

```text
 ┌──────────────────────────────────────────┐
 │ «abstract» CustomerImporter              │
 │ + Import(raw)          ← método plantilla│
 │     Preprocess()       ← hook (virtual)  │
 │     Parse()            ← abstract        │
 │     Validate()         ← virtual         │
 │     Save()             ← común (private) │
 │     AfterImport()      ← hook (vacío)    │
 └──────────────────────────────────────────┘
          ▲                         ▲
 ┌──────────────────────┐  ┌──────────────────────┐
 │ CsvCustomerImporter  │  │ JsonCustomerImporter │
 │ Preprocess(): quita  │  │ Parse(): JSON        │
 │   la cabecera        │  │ AfterImport(): correo│
 │ Parse(): CSV         │  └──────────────────────┘
 └──────────────────────┘
          ▲
 ┌────────────────────────────┐
 │ PartnerCsvCustomerImporter │
 │ Validate(): reglas estrictas│
 └────────────────────────────┘
```

---

## Ejemplo 1. Importación de clientes (`DataImport/`)

**AbstractClass**: `CustomerImporter`. El método plantilla `Import`:

```csharp
public IReadOnlyList<Customer> Import(string rawData)
{
    var content = Preprocess(rawData);          // hook
    var records = Parse(content);               // abstract
    var valid = records.Where(Validate).ToList();   // virtual
    Save(valid);                                // paso común
    AfterImport(valid);                         // hook
    return valid;
}
```

- `Parse`: abstracto; cada formato tiene el suyo.
- `Validate`: validación común (el email contiene `@`, la edad está dentro de límites razonables), pero se puede endurecer.
- `Save`: `private`; la subclase no puede ni sobrescribir este paso ni llamarlo por separado.
- `Preprocess` y `AfterImport`: hooks.

**ConcreteClass** (carpeta `Importers/`):
- `CsvCustomerImporter`: analiza las líneas separadas por `;` y sobrescribe el hook `Preprocess` para quitar la línea de cabecera;
- `JsonCustomerImporter`: analiza el JSON con `System.Text.Json` y sobrescribe el hook `AfterImport` (envío de correos de bienvenida);
- `PartnerCsvCustomerImporter`: hereda del **importador CSV** y solo endurece `Validate` (mayores de edad con correo corporativo), llamando a `base.Validate`. El análisis del CSV y todo el algoritmo los obtiene gratis.

**Client**: la demo trabaja con todos los importadores a través de la clase base y llama al mismo `Import`. En la salida se ve que los pasos son los mismos y solo cambian los detalles.

## Ejemplo 2. Bebidas calientes (`Beverages/`): el ejemplo clásico con hooks

**AbstractClass**: `HotBeverage` con el método plantilla `Prepare()`:

```csharp
public void Prepare()
{
    BoilWater();
    Brew();
    PourInCup();
    if (CustomerWantsCondiments())
        AddCondiments();
}
```

- `BoilWater`, `PourInCup`: pasos comunes;
- `Brew`, `AddCondiments`: abstractos;
- `WaterTemperature` y `CustomerWantsCondiments()`: **hooks**; por defecto, 100 °C y «sí, se quieren complementos».

**ConcreteClass** (carpeta `Drinks/`):
- `Tea`: solo los pasos obligatorios;
- `GreenTea`: sobrescribe los hooks: agua a 80 °C y sin complementos;
- `Coffee`: el hook `CustomerWantsCondiments()` depende de la elección del cliente (parámetro del constructor).

Los hooks muestran cómo una subclase puede **controlar el desarrollo** del algoritmo (saltarse un paso) sin cambiar el algoritmo en sí.

---

## El «principio de Hollywood»

*«No nos llame, ya le llamaremos nosotros».* La clase base llama a los métodos de las subclases, y no al revés. La subclase no controla el algoritmo: solo aporta los detalles cuando se le piden.

## Cuándo usarlo

- Varias clases hacen lo mismo y solo se diferencian en algunos pasos.
- Hay que permitir ampliar solo ciertas partes del algoritmo sin dejar cambiar su estructura.
- Hay que eliminar la duplicación de código común en clases parecidas.
- Ejemplos: importación/exportación de datos, generación de informes, procesamiento de peticiones (en ASP.NET Core, `ControllerBase`, `BackgroundService.ExecuteAsync`), frameworks de pruebas (`SetUp` → prueba → `TearDown`).

## Cuándo no hace falta

- Las variantes del algoritmo deben cambiarse **en tiempo de ejecución**: es mejor Strategy.
- Hay muchos pasos y las subclases los sobrescriben casi todos: en la práctica no hay un algoritmo común.

## Ventajas y desventajas

**Ventajas**
- El código común está en un solo lugar, sin duplicación.
- La estructura del algoritmo está protegida frente a cambios.
- Las subclases sobrescriben solo lo que realmente es distinto.

**Desventajas**
- Acoplamiento rígido mediante **herencia**: los cambios en la clase base afectan a todas las derivadas.
- Limitado por la estructura del esqueleto: un paso nuevo en mitad del algoritmo exige modificar la clase base.
- Se puede violar el principio de sustitución de Liskov si una subclase sobrescribe un paso de forma «inesperada» (por ejemplo, suprime un paso obligatorio).
- Cuantos más pasos, más difícil es de mantener.

## Relación con otros patrones

- **Strategy** resuelve un problema parecido mediante **composición** y permite cambiar el algoritmo completo en tiempo de ejecución. **Template Method** lo hace mediante **herencia**, y la variante se fija a nivel de clase.
- **Factory Method** es un caso particular de Template Method: el paso del algoritmo es la creación de un objeto. En este proyecto, `NotificationFactory.Notify()` de `FactoryMethod/NotifySystem` es un método plantilla que llama al método fábrica `CreateNotification()`.
