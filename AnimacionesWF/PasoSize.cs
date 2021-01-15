using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AnimacionesWF
{
    class PasoSize : Paso
    {
        public static int NORMAL = 0, CENTER = 1, CENTER_X = 2, CENTER_Y = 3;
        public int Width;
        public int Height;
        public int TipoHeight;
        public int TipoWidth;
        public bool CentrarX;
        public bool CentrarY;
        private bool finalizada;

        //Calculaos
        private float pseudoWidth;
        private float pseudoHeight;

        private float sumaWidth;
        private float sumaHeight;

        private int objetivoWidht;
        private int objetivoHeight;

        public PasoSize(XElement elemento)
        {
            parseXML(elemento);
        }

        public override void parseXML(XElement elemento)
        {
            //Default 
            CentrarX = false;
            CentrarY = false;

            switch (elemento.Attribute("modo")?.Value.ToLower()) {
                case "center":
                    CentrarX = true;
                    CentrarY = true;
                    break;
                case "center-x":
                    CentrarX = true;
                    break;
                case "center-y":
                    CentrarY = true;
                    break;
            }
            foreach (XElement elementoHijo in elemento.Elements()) {
                switch (elementoHijo.Name.LocalName.ToLower()) {
                    case "w":
                        Width = Int32.Parse(elementoHijo.Value);
                        TipoWidth = comprobarTipoValor(elementoHijo.Value);
                        break;
                    case "h":
                        Height = Int32.Parse(elementoHijo.Value);
                        TipoHeight= comprobarTipoValor(elementoHijo.Value);
                        break;
                    default:
                        throw new Exception("ChildNode no soportado " + elementoHijo.Name.LocalName);
                }
            }
        }

        public override void prepare(Control control, int nMovimientos)
        {
            finalizada = false;

            pseudoHeight = control.Size.Height;
            pseudoWidth= control.Size.Width;

            objetivoHeight = aplicarValor(Height, TipoHeight, control.Size.Height);
            objetivoWidht = aplicarValor(Width, TipoWidth, control.Size.Width);

            sumaHeight = calcularValorPorMovimientos(objetivoHeight, control.Size.Height, TipoHeight, nMovimientos);
            sumaWidth = calcularValorPorMovimientos(objetivoWidht, control.Size.Width, TipoWidth, nMovimientos);

            

        }

        public override bool setpForward(Control control)
        {
            int nuevoWidth, nuevoHeight, ajusteX = control.Location.X, ajusteY = control.Location.Y;
            if (finalizada) {
                return finalizada;
            }

            if (Math.Round(pseudoWidth) == objetivoWidht && Math.Round(pseudoHeight) == objetivoHeight)
            {
                control.Size = new System.Drawing.Size(objetivoWidht, objetivoHeight);  //Fix
                finalizada = true;
                return finalizada;
            }

            pseudoWidth += sumaWidth;
            pseudoHeight+= sumaHeight;

            nuevoWidth = (int)pseudoWidth;
            if (CentrarX) {
                ajusteX += (int) -sumaWidth /2;
            }

            nuevoHeight = (int)pseudoHeight;
            if (CentrarY)
            {
                ajusteY += (int)-sumaHeight /2;
            }

            control.Size = new System.Drawing.Size(nuevoWidth, nuevoHeight);

            if (CentrarX || CentrarY) {
                control.Location = new System.Drawing.Point(ajusteX, ajusteY);
            }

            return false;
        }
    }
}
