using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AnimacionesWF
{
    public class PasoPosicion : Paso
    {
        public int X;
        public int Y;

        public int TipoY;
        public int TipoX;

        private bool finalizada;

        //Valores calculaos
        private float pseudoX;
        private float pseudoY;

        private float sumaX;
        private float sumaY;

        private int objetivoX;
        private int objetivoY;

        public PasoPosicion(XElement elemento)
        {
            parseXML(elemento);
        }

        public override void prepare(Control control, int nMovimientos)
        {
            //Reestablecemos valores
            finalizada = false;

            //Establecemos las pseudo a la pos actual
            pseudoX = control.Location.X;
            pseudoY = control.Location.Y;

            //Calculamos los valores finales objetivo
            objetivoX = aplicarValor(X, TipoX, control.Location.X);
            objetivoY = aplicarValor(Y, TipoY, control.Location.Y);

            //Calculamos la suma
            sumaX = calcularValorPorMovimientos(objetivoX, control.Location.X, TipoX, nMovimientos);
            sumaY = calcularValorPorMovimientos(objetivoY, control.Location.Y, TipoY, nMovimientos);

            Console.WriteLine("Suma X" + sumaX);
            Console.WriteLine("Suma Y" + sumaY);


        }

        public override bool setpForward(Control control)
        {
            int nuevaX, nuevaY;

            if (finalizada) {
                return finalizada;
            }

            if (Math.Round(pseudoX) == objetivoX && Math.Round(pseudoY) == objetivoY)
            {
                Console.WriteLine("Fin paso posicion");
                control.Location = new Point(objetivoX, objetivoY); //Fix para divisiones y movimientos incompletos
                finalizada = true;
                return finalizada;
            }

            pseudoX += sumaX;
            pseudoY += sumaY;

            Console.WriteLine("PseudoX: " + pseudoX);
            Console.WriteLine("PseudoY: " + pseudoY);

            nuevaX = (int)pseudoX;
            nuevaY = (int)pseudoY;

            control.Location = new Point(nuevaX, nuevaY);
            Console.WriteLine(nuevaX + "::" + nuevaY);

            return false;
        }


        /// <summary>
        /// Depe
        /// </summary>
        /// <param name="elemento"></param>
        public override void parseXML(XElement elemento)
        {
            foreach (XElement elementoHijo in elemento.Elements()) {
                switch (elementoHijo.Name.LocalName.ToLower()) {
                    case "x":
                        TipoX = comprobarTipoValor(elementoHijo.Value);
                        X = Int32.Parse(elementoHijo.Value);
                        break;
                    case "y":
                        TipoY = comprobarTipoValor(elementoHijo.Value);
                        Y = Int32.Parse(elementoHijo.Value);
                        break;
                    default:
                        throw new Exception("ChildNode no soportado " + elementoHijo.Name.LocalName);
                }
            }
        }
    }
}
