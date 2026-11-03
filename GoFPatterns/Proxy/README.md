🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Proxy) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Proxy) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Proxy) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Proxy) | **Español**

# Proxy (Intermediario)

**Tipo:** patrón estructural.

**Proxy** es un objeto sustituto que implementa **la misma interfaz** que el objeto real y **controla el acceso** a él: intercepta las llamadas y puede hacer algo antes o después de pasar la llamada al original (o no pasarla en absoluto).

---

## Qué problema resuelve

Hay un objeto al que no se puede o no conviene acceder directamente:

- es **pesado**: una galería de 100 fotos de 20 MB no debería cargarlas todas al abrirse si el usuario solo va a mirar dos;
- necesita **control de acceso**: solo el titular puede retirar dinero de la cuenta;
- sus llamadas son **costosas** y conviene **cachear** el resultado: la consulta del saldo al sistema central del banco;
- hay que **registrar/auditar** los accesos;
- el objeto está **en otro lugar** (otro servicio, otra máquina).

Añadir todo esto a la propia clase es mala idea: empieza a encargarse de la seguridad, la caché y el logging en lugar de su lógica de negocio. Y a menudo la clase ni siquiera se puede modificar (código de terceros).

## Solución

Crear una clase sustituta con la misma interfaz. El proxy guarda una referencia al objeto real (o lo crea él mismo cuando hace falta) y le delega el trabajo, añadiendo su propia lógica. El cliente trabaja con la interfaz y **no nota la sustitución**.

### Tipos de proxy

| Tipo | Qué hace | Ejemplo en el proyecto |
|------|----------|------------------------|
| **Virtual proxy** | Pospone la creación del objeto pesado hasta el primer acceso | `LazyImageProxy` |
| **Protection proxy** | Comprueba los derechos de acceso | `ProtectionAccountProxy` |
| **Caching proxy** | Cachea los resultados | `CachingAccountProxy` |
| **Logging proxy** | Registra los accesos | `LoggingAccountProxy` |
| **Remote proxy** | Representa un objeto de otro espacio de direcciones (clientes gRPC/WCF, envoltorios de `HttpClient`) | — |
| **Smart reference** | Conteo de referencias, liberación de recursos cuando el objeto ya no se necesita | — |

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Subject** | `IImage`, `IBankAccount` | Interfaz común del objeto real y del proxy. Gracias a ella el proxy puede ocupar el lugar del original |
| **RealSubject** | `HighResolutionImage`, `BankAccount` | El objeto real con la lógica de negocio útil |
| **Proxy** | `LazyImageProxy`, `ProtectionAccountProxy`, `CachingAccountProxy`, `LoggingAccountProxy` | Guarda una referencia al RealSubject, controla el acceso a él y puede gestionar su ciclo de vida |
| **Client** | clases de demo | Trabaja a través de Subject y no sabe si tiene delante el original o el proxy |

## Esquema

```text
               ┌──────────────────────┐
  Client ─────►│ «interface» IImage   │
               │ Display()            │
               └──────────────────────┘
                    ▲             ▲
                    │             │
     ┌──────────────────┐   ┌───────────────────────┐
     │ LazyImageProxy   │──►│ HighResolutionImage   │
     │ - _realImage?    │   │ (pesado, se carga     │
     │ Display():       │   │  en el constructor)   │
     │  crear si null,  │   └───────────────────────┘
     │  luego delegar   │
     └──────────────────┘
```

---

## Ejemplo 1. Carga diferida de imágenes (`ImageLoading/`): virtual proxy

**Subject**: `IImage` (`FileName`, `Display()`).

**RealSubject**: `HighResolutionImage` carga la imagen desde el disco directamente en el constructor (simulado con `Thread.Sleep`). Crear un objeto así ya es costoso.

**Proxy**: `LazyImageProxy` guarda solo el nombre del archivo y crea el objeto real en la **primera** llamada a `Display()`:

```csharp
public void Display()
{
    _realImage ??= new HighResolutionImage(FileName);
    _realImage.Display();
}
```

**Client**: la demo abre una galería de 5 imágenes de dos maneras:
- sin proxy: los 5 archivos se cargan de golpe (~1 s);
- con proxy: la galería se abre al instante y solo se cargan las 2 imágenes que el usuario ha abierto. Mostrarla de nuevo ya no vuelve a cargar el archivo.

En .NET un papel parecido lo cumple `Lazy<T>`, y en Entity Framework, los **lazy-loading proxies** de las propiedades de navegación.

## Ejemplo 2. Cuenta bancaria (`BankAccount/`): protection, caching, logging

**Subject**: `IBankAccount` (`GetBalance()`, `Withdraw()`).

**RealSubject**: `BankAccount` contiene **solo** la lógica de negocio. La consulta del saldo es lenta (simula el acceso al sistema central del banco), y el contador `BalanceRequests` muestra cuántas veces se ha accedido realmente al banco.

**Proxies** (carpeta `Proxies/`):

`ProtectionAccountProxy`: solo `UserRole.Owner` puede retirar dinero:

```csharp
public void Withdraw(decimal amount)
{
    if (role != UserRole.Owner)
        throw new UnauthorizedAccessException($"{role} no puede retirar dinero.");
    account.Withdraw(amount);
}
```

`CachingAccountProxy`: recuerda el saldo y **vacía la caché** después de una retirada:

```csharp
public decimal GetBalance() => _cachedBalance ??= account.GetBalance();
```

`LoggingAccountProxy`: escribe la auditoría de todas las operaciones, incluidas las fallidas.

**Client**: como todos los proxies implementan `IBankAccount`, se pueden anidar unos dentro de otros:

```csharp
IBankAccount ownerAccess = new LoggingAccountProxy(
    new ProtectionAccountProxy(
        new CachingAccountProxy(realAccount), UserRole.Owner));
```

La demo muestra que tres consultas del saldo producen **un solo** acceso real al banco; tras la retirada, la caché se vacía y el saldo está actualizado; el invitado recibe una denegación, y esta queda registrada en la auditoría.

---

## Cuándo usarlo

- Inicialización diferida de un objeto pesado (virtual proxy).
- Control de acceso (protection proxy).
- Caché de resultados (caching proxy).
- Logging, métricas, auditoría de accesos.
- Trabajar con un objeto remoto como si fuera local (remote proxy).

## Cuándo no hace falta

- El objeto es ligero y no requiere control: sería un nivel de indirección superfluo.
- No se trata de controlar, sino de **añadir** un comportamiento nuevo que el propio cliente elige y combina: eso se parece más a Decorator.

## Ventajas y desventajas

**Ventajas**
- Control sobre el objeto sin que el cliente lo note.
- Permite gestionar el ciclo de vida del objeto real (crearlo más tarde, liberarlo antes).
- Funciona aunque el objeto real todavía no se haya creado o no esté disponible.
- Open/Closed: los proxies nuevos se añaden sin modificar el servicio ni el cliente.

**Desventajas**
- Un nivel adicional de indirección; puede haber retrasos en la respuesta.
- Aumenta el número de clases.

## Relación con otros patrones

- **Adapter** ofrece una interfaz **distinta**; **Proxy**, **la misma**; **Decorator**, la misma pero ampliada.
- **Decorator** y **Proxy** tienen una estructura casi idéntica (envoltorio + delegación); la diferencia está en la intención: el decorador **añade** comportamiento y su composición la decide el cliente; el proxy **controla el acceso** y normalmente gestiona él mismo el ciclo de vida del objeto (como `LazyImageProxy`, que crea él mismo el `HighResolutionImage`).
- **Facade** también oculta un objeto tras de sí, pero la fachada tiene su propia interfaz simplificada, mientras que el proxy tiene la interfaz del original.
