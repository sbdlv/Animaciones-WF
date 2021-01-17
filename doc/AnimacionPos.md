# pos
Tag: `<pos>`

Mueve la posicion de un control a los valores especificados.
## Atributos
 - hiddenstart: Define si el control empezara desde uno de los ejes del formulario, fuera del campo visual.
    - top: El control se reposicionara fuera de la parte superior del fomrulario.
    - bottom: El control se reposicionara fuera de la parte inferior del fomrulario.
    - left: El control se reposicionara fuera de la parte izquierda del fomrulario.
    - right: El control se reposicionara fuera de la parte derecha del fomrulario.
## Valores
- x: Modifica la posición x
- y: Modifica la posición y

## Ejemplos
```
<pos>
    <x>+100</x>
    <y>200</y>
</pos>
```
```
<pos hiddenstart="top">
    <y>+200</y>
</pos>
```