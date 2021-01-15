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
    class PasoCircular : Paso
    {
        public bool UnderOrigin;    //True si el circulo toma como centro
        public int Radio;

        //Calculados
        private int offSet;
        private int diferenciaPosiciones;
        private Point[] posiciones;
        private int punteroPosiciones;
        private bool isFinalizada;

        public PasoCircular(XElement elemento)
        {
            parseXML(elemento);
        }

        public override void parseXML(XElement elemento)
        {
            foreach (XElement elementoHijo in elemento.Elements())
            {
                switch (elementoHijo.Name.LocalName.ToLower()) {
                    case "r":
                        Radio = Int32.Parse(elementoHijo.Value);
                        break;
                }
            }
        }

        public override void prepare(Control control, int nMovimientos)
        {
            int x, y, grado = 1;

            //Reset
            offSet = 0;
            punteroPosiciones = 0;

            diferenciaPosiciones = 360 / nMovimientos;
            grado = diferenciaPosiciones;
            posiciones = new Point[nMovimientos];

            //Calculo de psoiciones
            for (int i = 0; i < nMovimientos; i++)
            {
                //Evitar pasarnos, ya que empezamos en el grado 1º
                if (grado > 360) {
                    grado = 360;
                }
                Console.WriteLine(String.Format("x({0})= {1} * Cos({0}) + {2}",grado,Radio,control.Location.X));
                x = (int)(Radio * Math.Cos(grado) + control.Location.X);
                y = (int)(Radio * Math.Sin(grado) + control.Location.Y);

                posiciones[i] = new Point(x, y);
                Console.WriteLine(posiciones[i]);

                grado += diferenciaPosiciones;
            }
        }

        public override bool setpForward(Control control)
        {
            //Fin animacion
            if (punteroPosiciones == posiciones.Length) {
                isFinalizada = true;
                //Hacer fix final
                //
                return true;
            }
            control.Location = posiciones[punteroPosiciones];
            punteroPosiciones++;

            return false;
        }
    }
}
