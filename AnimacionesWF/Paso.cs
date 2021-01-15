using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AnimacionesWF
{
    public abstract class Paso
    {
        public const int RELATIVO = 2, ABSOLUTO = 1, CONGELADO = 0;
        public String Signal;
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
        protected static float calcularValorPorMovimientos(int objetivo, int actual, int tipo, int nMovimientos) {
            switch (tipo)
            {
                case CONGELADO:
                    return 0;
                case RELATIVO:
                    return (float) (objetivo - actual)  / nMovimientos;
                case ABSOLUTO:
                    return (float) (objetivo - actual) / nMovimientos;
                default:
                    throw new Exception("Tipo de valor no soportado: " + tipo);
            }
        }

        public abstract void parseXML(XElement elemento);

        protected static int aplicarValor(int valor, int tipoValor, int actual) {
            switch (tipoValor) {
                case CONGELADO:
                    return actual;
                case ABSOLUTO:
                    return valor;
                case RELATIVO:
                    return actual + valor;
                default:
                    throw new Exception("Tipo de valor desconocido " + tipoValor);
            }
        }
        protected static int comprobarTipoValor(String valor)
        {
            //Comprobamos los signos del valor
            switch (valor[0])
            {
                case '+':
                case '-':
                    return RELATIVO;
            }

            return ABSOLUTO;
        }
    }
}
