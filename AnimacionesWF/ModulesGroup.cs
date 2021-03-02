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
    public class ModulesGroup : IEnumerator
    {
        public int Ms;
        public List<AnimationModule> Animations;
        private int puntero;
        public String StartSignal;
        public String EndSignal;
        public int Repeticiones;



        public ModulesGroup(XElement elemento) : this(0)
        {
            Ms = Int32.Parse(elemento.Attribute("ms").Value);
            Repeticiones = elemento.Attribute("loops") == null ? 0 : Int32.Parse(elemento.Attribute("loops").Value);
            StartSignal = elemento.Attribute("startSignal") == null ? "" : elemento.Attribute("startSignal").Value;
            EndSignal = elemento.Attribute("endSignal") == null ? "": elemento.Attribute("endSignal").Value;

            //Parseamos los pasos que tiene el grupo
            foreach (XElement elementoHijo in elemento.Elements())
            {
                //Cargamos de la clase Animacion el modulo segun su tagname
                Animations.Add(Animation.InstanceNewModule(elementoHijo.Name.LocalName, elementoHijo));
            }
        }

        public ModulesGroup(int ms, List<AnimationModule> pasos)
        {
            Ms = ms;
            this.Animations = pasos;
            puntero = 0;
        }

        public ModulesGroup(int ms) : this(ms, new List<AnimationModule>()){}

        public bool MoveNext()
        {
            puntero++;
            return puntero < Animations.Count;
        }

        public void Reset()
        {
            puntero = 0;
        }

        public object Current
        {
            get
            {
                return Animations[puntero];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="interval"></param>
        /// <returns>El N.Total de movimientos que se van a tener que realizar para finalizar el grupo</returns>
        public int prepare(Control control, int interval) {
            int nMovimientos = Ms / interval;

            foreach (AnimationModule paso in Animations)
            {
                paso.prepare(control, nMovimientos);
            }

            return nMovimientos;
        }
    }
}
