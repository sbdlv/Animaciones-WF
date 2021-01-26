using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AnimacionesWF
{
    [XMLTagName(null)]
    public abstract class AnimationModule
    {

        public AnimationModule(XElement root) { 
        }

        /// <summary>
        /// Cambia los datos del control
        /// </summary>
        /// <returns>Trues si ya se ha acabado el paso</returns>
        public abstract bool setpForward(Control control);


        /// <summary>
        /// Prepara el paso sabieno el Nº de movimientos que se van a hacer. Se resetean los valores internos.
        /// </summary>
        /// <param name="nMovimientos"></param>
        public abstract void prepare(Control control, int nMovimientos);

        /// <summary>
        /// Calcula la division de un valor entre el numero de llamadas que va a tener la animacion, segun su tipo de valor
        /// </summary>
        /// <param name="objetivo">El valor objetivo al final de la animación</param>
        /// <param name="actual">El valor actual</param>
        /// <param name="tipo">El tipo de valor</param>
        /// <param name="nMovimientos">El nº de llamadas que tiene la animacion</param>
        /// <returns></returns>
        protected static float calcularValorPorMovimientos(int objetivo, int actual, ValueType tipo, int nMovimientos) {
            float final;

            switch (tipo)
            {
                case ValueType.FROZEN:
                    return 0;
                case ValueType.RELATIVE:
                    final = objetivo - actual;
                    if (nMovimientos != 0) {
                        final /= nMovimientos;
                    }
                    return final;
                case ValueType.ABSOLUTE:
                    final = objetivo - actual;
                    if (nMovimientos != 0)
                    {
                        final /= nMovimientos;
                    }
                    return final;
                default:
                    throw new Exception("Tipo de valor no soportado: " + tipo);
            }
        }

        protected static int aplicarValor(int valor, ValueType tipoValor, int actual) {
            switch (tipoValor) {
                case ValueType.FROZEN:
                    return actual;
                case ValueType.ABSOLUTE:
                    return valor;
                case ValueType.RELATIVE:
                    return actual + valor;
                default:
                    throw new Exception("Tipo de valor no soportado: " + tipoValor);
            }
        }
        protected static ValueType comprobarTipoValor(String valor)
        {
            //Comprobamos los signos del valor
            switch (valor[0])
            {
                case '+':
                case '-':
                    return ValueType.RELATIVE;
            }

            if (valor[valor.Length - 1] == '%')
            {
                return ValueType.PERCENTAGE;
            }

            return ValueType.ABSOLUTE;
        }

        public enum ValueType {
            FROZEN,
            PERCENTAGE,
            RELATIVE,
            ABSOLUTE,
        }

        public enum Directions { 
            TOP,BOTTOM,LEFT,RIGHT, NONE
        }
    }
}
