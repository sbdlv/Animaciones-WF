# size
Tag: `<size>`

Modifica el tamaño de un control.
## Valores
- w: Modifica el ancho del control.
- h: Modifica el alto del control.
## Atributos
- modo: Define como se deberia de hacer el resize.
    - normal: Simplemente se establecen los valores definidos. No es necesario definir este modo.
    - center: Mantiene el control centrado a la vez que se hace resize.

## Ejemplos
```
<size mode="normal">
    <w>+100</w>
    <h>55</h>
</size>
```
---
***Modo center*** 

Seguramente produzca errores con animaciones pos.

---