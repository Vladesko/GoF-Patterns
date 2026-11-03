🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/AbtractFactory) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/AbtractFactory) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/AbtractFactory) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/AbtractFactory) | **Español**

# Abstract Factory (Fábrica abstracta)

**Tipo:** patrón creacional.

**Abstract Factory** permite crear **familias de objetos relacionados** sin atarse a las clases concretas de esos objetos. El cliente trabaja con la fábrica y con los productos solo a través de interfaces.

---

## Qué problema resuelve

Un servicio de pagos trabaja con proveedores de pago (en el ejemplo, los ficticios **PoolPay** y **Strope**). Para cada proveedor se necesita un **conjunto** de objetos:
- procesador de pagos;
- procesador de reembolsos;
- validador de pagos.

Estos objetos forman una **familia**: el procesador de PoolPay debe funcionar junto con el validador y los reembolsos de PoolPay. Mezclar el procesador de PoolPay con los reembolsos de Strope es un error (la transacción de un proveedor no existe en el otro).

Si el servicio de pagos crea los objetos por sí mismo:

```csharp
if (provider == "PoolPay") { processor = new PoolPayPaymentProcess(); refund = new PoolPayRefundProcessor(); ... }
else if (provider == "Strope") { processor = new StropePaymentProcessor(); refund = new StropeRefundProcessor(); ... }
```

- el servicio depende de **todas** las clases concretas de todos los proveedores;
- añadir un proveedor implica modificar el servicio;
- es fácil equivocarse y crear un «conjunto incompatible».

## Solución

1. Definir una **interfaz** para cada tipo de producto (`IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator`).
2. Declarar una **fábrica abstracta**: una interfaz con métodos para crear cada producto de la familia.
3. Para cada familia (proveedor), crear una **fábrica concreta** que solo crea productos de su propia familia.
4. El cliente recibe la fábrica desde fuera y crea los productos solo a través de ella; así no conoce las clases concretas y **no puede mezclar familias**.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **AbstractFactory** | `IPaymentProviderFactory` | Declara los métodos de creación de todos los productos de la familia |
| **ConcreteFactory** | `PoolPayPaymentFactory`, `StropePaymentFactory` | Crea los productos de una familia (de un proveedor) |
| **AbstractProduct** | `IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator` | Interfaces de los productos |
| **ConcreteProduct** | `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`, `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator` | Implementaciones concretas de los productos para cada proveedor |
| **Client** | `PaymentService` (+ `IPaymentService`) | Usa solo las interfaces de la fábrica y de los productos |

## Esquema

```text
                       «interface» IPaymentProviderFactory
                         CreatePaymentProcess(): IPaymentProcessor
                         CreateRefundProcess():  IRefundProcessor
                         CreatePaymentValidator(): IPaymentValidator
                              ▲                         ▲
                              │                         │
               PoolPayPaymentFactory            StropePaymentFactory
                 │      │       │                 │      │       │
                 ▼      ▼       ▼                 ▼      ▼       ▼
         PoolPayPayment PoolPay PoolPay      StropePayment Strope Strope
           Process      Refund  Validator      Processor   Refund Validator

 PaymentService(IPaymentProviderFactory factory)   ← Cliente
   Pay(amount):  validator.ValidatePayment → processor.ProcessPayment
   Refund(id):   refundProcessor.ProcessRefund
```

---

## Ejemplo. Proveedores de pago (`Payments/`)

**AbstractProduct** (carpeta `Interfaces/Abstractions/`):

```csharp
public interface IPaymentProcessor { Guid ProcessPayment(decimal amount); }
public interface IRefundProcessor  { void ProcessRefund(Guid transactionId); }
public interface IPaymentValidator { bool ValidatePayment(decimal amount); }
```

**AbstractFactory** (carpeta `Interfaces/AbstractFactories/`):

```csharp
public interface IPaymentProviderFactory
{
    IPaymentProcessor CreatePaymentProcess();
    IRefundProcessor CreateRefundProcess();
    IPaymentValidator CreatePaymentValidator();
}
```

**ConcreteFactory + ConcreteProduct**: cada proveedor tiene su propia carpeta:
- `PoolPays/`: `PoolPayPaymentFactory` crea `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`;
- `Stopes/`: `StropePaymentFactory` crea `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator`.

Todas las clases concretas están marcadas como `internal`: fuera del ensamblado nadie las conoce, solo están disponibles las interfaces.

```csharp
internal class PoolPayPaymentFactory : IPaymentProviderFactory
{
    public IPaymentProcessor CreatePaymentProcess() => new PoolPayPaymentProcess();
    public IPaymentValidator CreatePaymentValidator() => new PoolPayPaymentValidator();
    public IRefundProcessor CreateRefundProcess() => new PoolPayRefundProcessor();
}
```

**Client**: `PaymentService` (carpeta `Services/`) recibe la fábrica en el constructor y crea con ella **una sola vez** todo el conjunto de productos. A partir de ahí trabaja solo con interfaces:

```csharp
internal class PaymentService(IPaymentProviderFactory paymentProviderFactory) : IPaymentService
{
    private readonly IPaymentProcessor _paymentProcessor = paymentProviderFactory.CreatePaymentProcess();
    private readonly IRefundProcessor _refundProcessor = paymentProviderFactory.CreateRefundProcess();
    private readonly IPaymentValidator _paymentValidator = paymentProviderFactory.CreatePaymentValidator();

    public Guid Pay(decimal amount)
    {
        if (!_paymentValidator.ValidatePayment(amount))
            throw new InvalidOperationException("Importe de pago no válido.");
        return _paymentProcessor.ProcessPayment(amount);
    }

    public void Refund(Guid transactionId) => _refundProcessor.ProcessRefund(transactionId);
}
```

Como los tres productos se obtienen de la misma fábrica, **pertenecen con garantía a la misma familia**.

**Demostración**: `DemontrateAbstractFactory` crea `PaymentService` primero con la fábrica de PoolPay y después con la de Strope, y ejecuta las mismas operaciones `Pay` + `Refund`. El código del servicio es el mismo; solo cambia la fábrica que se le pasa.

Para incorporar un proveedor nuevo basta con añadir una carpeta con la fábrica y los tres productos. `PaymentService` y los proveedores existentes no cambian.

---

## Cuándo usarlo

- El sistema debe trabajar con distintas **familias** de objetos relacionados que deben usarse **juntos**.
- Hay que ocultar al cliente las clases concretas de los productos.
- La familia se elige una sola vez (por configuración, por ajustes) y después se usa en todas partes.
- Ejemplos: proveedores de pago, temas de interfaz (botón + casilla + ventana para Windows/macOS), proveedores de BD (`DbProviderFactory` en ADO.NET: conexión + comando + parámetro), proveedores en la nube (almacenamiento + cola + caché).

## Cuándo no hace falta

- Hay un único producto y no una familia: basta con Factory Method.
- Solo hay una familia y no se va a ampliar.

## Ventajas y desventajas

**Ventajas**
- Garantiza la compatibilidad de los productos creados.
- El cliente no depende de las clases concretas.
- El código de creación de productos está reunido en un solo lugar (SRP).
- Una familia nueva se añade sin modificar el cliente (Open/Closed).

**Desventajas**
- Muchas interfaces y clases.
- Añadir un **nuevo tipo de producto** (por ejemplo, `ISubscriptionProcessor`) es difícil: hay que modificar la interfaz de la fábrica y **todas** las fábricas concretas.

## Relación con otros patrones

- **Factory Method**: una fábrica abstracta suele estar formada por varios métodos fábrica. Factory Method crea **un** producto y se basa en la herencia; Abstract Factory crea una **familia** y se pasa como objeto (composición).
- **Builder** ensambla **un** objeto complejo paso a paso; Abstract Factory devuelve de inmediato un **conjunto** de objetos simples.
- **Prototype**: la fábrica puede crear productos clonando prototipos.
- **Singleton**: normalmente basta con una sola instancia de cada fábrica concreta.
- **Facade**: se puede usar una fábrica abstracta en lugar de una fachada para ocultar cómo se crean los objetos del subsistema.
- **Strategy**: desde el punto de vista de `PaymentService`, la fábrica parece una estrategia de «qué proveedor usar».
