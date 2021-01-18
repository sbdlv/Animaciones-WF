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

        public AdjustInfo adjustInfo;

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

            //Adjust
            if (adjustInfo != null) {
                int adjustControlX, adjustControlY, adjustParentX, adjustParentY;


                //Control
                if (adjustInfo.TipoControlX == NumberType.PERCENTAGE)
                {
                    adjustControlX = control.Size.Width * adjustInfo.ControlX / 100;
                }
                else {
                    adjustControlX = adjustInfo.ControlX;
                }

                if (adjustInfo.TipoControlY == NumberType.PERCENTAGE)
                {
                    adjustControlY = control.Size.Height * adjustInfo.ControlY / 100;
                }
                else
                {
                    adjustControlY = adjustInfo.ControlY;
                }

                //Parent
                Size parentSize;

                if (adjustInfo.adjustToForm)
                {
                    parentSize = control.FindForm().ClientSize;
                }
                else {
                    parentSize = control.Parent.Size;
                }

                if (adjustInfo.TipoParentX == NumberType.PERCENTAGE)
                {
                    adjustParentX = parentSize.Width * adjustInfo.ParentX / 100;
                }
                else {
                    adjustParentX = adjustInfo.ParentX;
                }

                if (adjustInfo.TipoParentY == NumberType.PERCENTAGE)
                {
                    adjustParentY = parentSize.Height * adjustInfo.ParentY / 100;
                }
                else
                {
                    adjustParentY = adjustInfo.ParentY;
                }

                //Adjustment
                control.Location = new Point(adjustParentX - adjustControlX, adjustParentY - adjustControlY);
            }

            //Establecemos las pseudo a la pos actual
            pseudoX = control.Location.X;
            pseudoY = control.Location.Y;

            //Calculamos los valores finales objetivo
            objetivoX = aplicarValor(X, TipoX, control.Location.X);
            objetivoY = aplicarValor(Y, TipoY, control.Location.Y);

            //Calculamos la suma
            sumaX = calcularValorPorMovimientos(objetivoX, control.Location.X, TipoX, nMovimientos);
            sumaY = calcularValorPorMovimientos(objetivoY, control.Location.Y, TipoY, nMovimientos);

            //Console.WriteLine("Suma X" + sumaX);
            //Console.WriteLine("Suma Y" + sumaY);
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

            //Console.WriteLine("PseudoX: " + pseudoX);
            //Console.WriteLine("PseudoY: " + pseudoY);

            nuevaX = (int)pseudoX;
            nuevaY = (int)pseudoY;

            control.Location = new Point(nuevaX, nuevaY);
            //Console.WriteLine(nuevaX + "::" + nuevaY);

            return false;
        }


        /// <summary>
        /// Depe
        /// </summary>
        /// <param name="elemento"></param>
        public override void parseXML(XElement elemento)
        {
            //Attr
            if (elemento.Attribute("adjust") != null) {
                adjustInfo = new AdjustInfo(elemento.Attribute("adjust").Value);
            }

            //Childs
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

        public class AdjustInfo {
            public int ControlX, ControlY, ParentX, ParentY;
            public NumberType TipoControlX, TipoControlY, TipoParentX, TipoParentY;
            public bool adjustToForm;

            public AdjustInfo(String adjust) {
                String[] adjustParams = adjust.Split(' ');

                TipoControlX = comprobarTipoNumero(adjustParams[0]);
                if (TipoControlX == NumberType.PERCENTAGE) {
                    ControlX = Int32.Parse(adjustParams[0].Substring(0, adjustParams[0].Length - 1));
                }

                TipoControlY = comprobarTipoNumero(adjustParams[1]);
                if (TipoControlY == NumberType.PERCENTAGE)
                {
                    ControlY = Int32.Parse(adjustParams[1].Substring(0, adjustParams[1].Length - 1));
                }

                TipoParentX = comprobarTipoNumero(adjustParams[2]);
                if (TipoParentX == NumberType.PERCENTAGE)
                {
                    ParentX = Int32.Parse(adjustParams[2].Substring(0, adjustParams[2].Length - 1));
                }

                TipoParentY = comprobarTipoNumero(adjustParams[3]);
                if (TipoParentY == NumberType.PERCENTAGE)
                {
                    ParentY = Int32.Parse(adjustParams[3].Substring(0, adjustParams[3].Length - 1));
                }
                adjustToForm = adjustParams[4].ToLower().Equals("form");
            }
        }
    }
}
