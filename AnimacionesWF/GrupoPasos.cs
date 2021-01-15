using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AnimacionesWF
{
    public class GrupoPasos : IEnumerator
    {
        public int Ms;
        public List<Paso> Pasos;
        private int puntero;
        public String StartSignal;
        public String EndSignal;
        public int Repeticiones;



        public GrupoPasos(XElement elemento) : this(0)
        {
            Ms = Int32.Parse(elemento.Attribute("ms").Value);
            Repeticiones = elemento.Attribute("loops") == null ? 0 : Int32.Parse(elemento.Attribute("loops").Value);
            StartSignal = elemento.Attribute("startSignal") == null ? "" : elemento.Attribute("startSignal").Value;
            EndSignal = elemento.Attribute("endSignal") == null ? "": elemento.Attribute("endSignal").Value;

            //Parseamos los pasos que tiene el grupo
            foreach (XElement elementoHijo in elemento.Elements())
            {
                switch (elementoHijo.Name.LocalName)
                {
                    case "pos":
                        Pasos.Add(new PasoPosicion(elementoHijo));
                        break;
                    case "size":
                        Pasos.Add(new PasoSize(elementoHijo));
                        break;
                    case "circle":
                        Pasos.Add(new PasoCircular(elementoHijo));
                        break;
                    default:
                        throw new Exception("Paso no soportado " + elementoHijo.Name);
                }
            }
        }

        public GrupoPasos(int ms, List<Paso> pasos)
        {
            Ms = ms;
            this.Pasos = pasos;
            puntero = 0;
        }

        public GrupoPasos(int ms) : this(ms, new List<Paso>()){}

        public bool MoveNext()
        {
            puntero++;
            return puntero < Pasos.Count;
        }

        public void Reset()
        {
            puntero = 0;
        }

        public object Current
        {
            get
            {
                return Pasos[puntero];
            }
        }

        public void prepare(Control control, int interval) {
            int nMovimientos = Ms / interval;

            foreach (Paso paso in Pasos)
            {
                paso.prepare(control, nMovimientos);
            }
        }
    }
}
