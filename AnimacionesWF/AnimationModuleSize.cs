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
    [XMLTagName("size")]
    class AnimationModuleSize : AnimationModule
    {
        public static int NORMAL = 0, CENTER = 1, CENTER_X = 2, CENTER_Y = 3;
        public int Width;
        public int Height;
        public ValueType TipoHeight;
        public ValueType TipoWidth;
        public bool CentrarX;
        public bool CentrarY;
        private bool finalizada;
        private Point posOriginal;
        private Size sizeOriginal;
        private bool acumuladoX, acumuladoY;

        //Calculados
        private float pseudoWidth;
        private float pseudoHeight;

        private float sumaWidth;
        private float sumaHeight;

        private int objetivoWidht;
        private int objetivoHeight;

        public AnimationModuleSize(XElement root) : base(root)
        {
            //Default 
            CentrarX = false;
            CentrarY = false;

            switch (root.Attribute("mode")?.Value.ToLower()) {
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
            foreach (XElement elementoHijo in root.Elements()) {
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

            posOriginal = control.Location;
            sizeOriginal = control.Size;

            

        }

        public override bool setpForward(Control control)
        {
            int nuevoWidth, nuevoHeight, ajusteX = 0, ajusteY = 0, fixedSumaW = (int) sumaWidth, fixedSumaH = (int) sumaHeight;
            if (finalizada) {
                return finalizada;
            }

            if (Math.Round(pseudoWidth) == objetivoWidht && Math.Round(pseudoHeight) == objetivoHeight)
            {
                Console.WriteLine("From " + control.Size);
                control.Size = new System.Drawing.Size(objetivoWidht, objetivoHeight);  //Fix
                Console.WriteLine("To" + control.Size);
                finalizada = true;
                return finalizada;
            }




            //Centrar

            //En la anterior hemos restado 1 a suma?

            /*
            nuevoWidth = control.Width;
            if (acumuladoX) {
                acumuladoX = false;
                fixedSumaW = (int)sumaWidth + 1;
            } else if (sumaWidth % 2 != 0)
            {
                acumuladoX = true;
                fixedSumaW = (int)sumaWidth - 1;
            }


            nuevoWidth += fixedSumaW;
            ajusteX = fixedSumaW / 2;*/

            //Pseudos
            pseudoHeight += sumaHeight;
            pseudoWidth += sumaWidth;

            

            nuevoWidth = (int) pseudoWidth;
            nuevoHeight = (int) pseudoHeight;

            if (CentrarX)
            {
                ajusteX = nuevoWidth / 2;
            }

            //Establecemos el tamaño
            control.Size = new Size(nuevoWidth, nuevoHeight);

            //Centrar
            if (CentrarX || CentrarY) {
                //control.Location = new Point(control.Location.X - ajusteX, control.Location.Y - ajusteY);
                control.Location = new Point(posOriginal.X - ajusteX, posOriginal.Y - ajusteY);
            }

            Console.WriteLine("[Step]: " + control.Width + "    Division: " + ((int) sumaWidth / 2) + "\tPosX: " + control.Location.X);

            return false;
        }
    }
}
