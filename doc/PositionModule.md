# Position Module
Tag: `<pos>`

Mueve la posicion de un control a los valores especificados.
## Atributos
- adjust: Posiciona el control mediante su punto de referencia a su padre o al formulario. Sintaxis: `adjust="A% B% D% C% parent"`
    - A: El procentaje o valor entero (px) para el punto de reajuste en el eje de las X del control
    - B: El procentaje o valor entero (px) para el punto de reajuste en el eje de las Y del control
    - D: El procentaje o valor entero (px) para el punto de reajuste sobre el parent/form en el que se colocara el punto de referencia del control para ele eje de las X
    - C: El procentaje o valor entero (px) para el punto de reajuste sobre el parent/form en el que se colocara el punto de referencia del control para ele eje de las Y
    - parent: El padre en el que se colocara el punto de referencia del control
        - form: El formulario
        - parent: El padre del control 
## Valores
- x: Modifica la posición x
- y: Modifica la posición y
## Excplicación adjust
Algunos ejemplos gráficos de como el atributo `adjust` funciona:
```
adjust="0% 0% ? ? ?"
○──────┐
│      │
│      │
└──────┘
```
```
adjust="100% 0% ? ? ?"
┌──────○
│      │
│      │
└──────┘
```
```
adjust="100% 100% ? ? ?"
┌──────┐
│      │
│      │
└──────○
```
```
adjust="0% 100% ? ? ?"
┌──────┐
│      │
│      │
○──────┘
```

```
En relación al padre/formulario:

adjust="100% 100% 0% 0% form"

┌──────┐
│//////│
│//////│
└──────○════════════════════════╗
       ║                      x ║
       ║                        ║
       ║          Form1         ║
       ║                        ║
       ║                        ║
       ╚════════════════════════╝
```
```
En relación al padre/formulario:

adjust="0% 100% 0% 100% form"

       ╔════════════════════════╗
       ║                      x ║
       ║                        ║
       ┌──────┐   Form1         ║
       │      │                 ║
       │      │                 ║
       ○──────┘═════════════════╝
       
```
## Ejemplos
```xml
<pos>
    <x>+100</x>
    <y>200</y>
</pos>
```
```xml
<pos adjust="120 50% 0 0 form">
    <x>+200</x>
</pos>
```
