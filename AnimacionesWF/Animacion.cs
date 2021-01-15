using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Linq;

namespace AnimacionesWF
{
    public class Animacion
    {
        private const int TIMER_INTERVAL_DEFAULT = 55;
		
		//Eventos
        public event Action FinAnimacion;

        //Propiedades
        public String Nombre;

        private int pasosReproducidos;

        private Timer Timer;

        private List<GrupoPasos> Pasos;

        private Control control;
        private int repeticiones;

        //Control
        private bool isPlaying;

        //Eventos
        public event Action<String> OnEndSignal;
        public event Action<String> OnStartSignal;

        public Animacion()
        {
            
        }



        public Animacion(XElement elemento, Control control): this()
        {
            this.Timer = new Timer();
            this.Pasos = new List<GrupoPasos>();
            this.Timer.Tick += Tick;
            Timer.Interval = TIMER_INTERVAL_DEFAULT;
            this.control = control;

            //Parse XML
            Nombre = elemento.Attribute("name") == null ? "undefined" : elemento.Attribute("name").Value;

            foreach (XElement elementoHijo in elemento.Elements()) {
                switch (elementoHijo.Name.LocalName) {
                    case "key-step":
                        Pasos.Add(new GrupoPasos(elementoHijo));
                        break;
                    default:
                        throw new Exception("Etiqueta no soportada " + elementoHijo.Name);
                }
            }
        }

        public void Reproducir(Control control)
        {
            if (isPlaying)
            {
                throw new Exception("La animación ya se esta reprouciendo");
            }
            else {
                isPlaying = true;

                Pasos[pasosReproducidos].prepare(control, Timer.Interval);
                sendSignal(Pasos[pasosReproducidos].StartSignal, OnStartSignal);

                //Empezamos el timer
                Timer.Enabled = true;
            }
        }

        private void PasoAcabado()
        {
            Timer.Enabled = false;

            //Loops de los grupos
            if (repeticiones != Pasos[pasosReproducidos].Repeticiones) {
                repeticiones++;
                Pasos[pasosReproducidos].prepare(control, Timer.Interval);
                Timer.Enabled = true;
                return;
            }

            //Si es el final
            sendSignal(Pasos[pasosReproducidos].EndSignal, OnEndSignal);

            pasosReproducidos++;

            repeticiones = 0;

            //Comprobamos si ya hemos acabado todos los grupos
            if (pasosReproducidos != Pasos.Count)
            {
                Pasos[pasosReproducidos].prepare(control, Timer.Interval);
                sendSignal(Pasos[pasosReproducidos].StartSignal, OnStartSignal);
                Timer.Enabled = true;
            }
            else {
                //Se han reproducido todos los pasos
                FinAnimacion?.Invoke();
                Detener();
            }
        }

        private void Tick(Object sender, EventArgs e)
        {
            bool finPasos = true;

            foreach (Paso paso in Pasos[pasosReproducidos].Pasos) {
                finPasos = paso.setpForward(control);
            }

            if (finPasos) {
                PasoAcabado();
            }
        }

        public void Detener() {
            pasosReproducidos = 0;
            isPlaying = false;
            Timer.Enabled = false;
        }

        private void sendSignal(String signalName, Action<String> eventSignal) {
            if (signalName == null)
            {
                eventSignal?.Invoke("");
            }
            else
            {
                eventSignal?.Invoke(signalName);
            }
        }
    }
}

