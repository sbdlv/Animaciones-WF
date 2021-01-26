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
    public class Animation
    {
        private const int TIMER_INTERVAL_DEFAULT = 55;
		
		//Events
        public event Action FinAnimacion;

        //Propiedades
        public String Nombre;

        private int pasosReproducidos;

        private Timer Timer;

        private List<ModulesGroup> ModulesGroup;

        private Control control;
        private int repeticiones;

        //Control
        private bool isPlaying;

        //Eventos
        public event Action<String> OnEndSignal;
        public event Action<String> OnStartSignal;

        //Parser
        private static Dictionary<String, Type> _modules;
        private static Dictionary<String, Type> modules
        {
            get
            {
                if (_modules == null)
                {
                    _modules = new Dictionary<string, Type>();

                    //Built-in modules
                    RegisterAnimationModule(typeof(AnimationModulePosition));
                    RegisterAnimationModule(typeof(AnimationModuleSize));

                }
                return _modules;
            }
        }

        public static AnimationModule InstanceNewModule(String XMLTagName, XElement root)
        {
            if (!modules.Keys.Contains(XMLTagName)) {
                throw new Exception("Tag de module desconocido. Asegurate de registrar antes el modulo en la clase Animation");
            }
            return (AnimationModule)Activator.CreateInstance(modules[XMLTagName], root);
        }

        //Modules
        public static void RegisterAnimationModule(AnimationModule module)
        {
            RegisterAnimationModule(module.GetType());
        }

        public static void RegisterAnimationModule(Type module)
        {
            String tagName = null;

            if (!typeof(AnimationModule).IsAssignableFrom(module)) {
                throw new Exception("El tipo pasado no hereda de la clase AnimationModule");
            }

            foreach (object o in module.GetCustomAttributes(true))
            {
                if (o.GetType().Equals(typeof(XMLTagNameAttribute)))
                {
                    tagName = ((XMLTagNameAttribute)o).XMLTagName;
                    break;
                }
            }
            if (tagName == null)
            {
                throw new Exception("El modulo no contiene el atributo [XMLTagName()] definido. Por favor, definelo");
            }

            try
            {
                modules.Add(tagName, module);
            }
            catch (ArgumentException e)
            {
                throw new Exception("Ya existe un modulo registrado con el tag: " + tagName);
            }
        }


        public Animation(XElement elemento, Control control)
        {
            this.Timer = new Timer();
            this.ModulesGroup = new List<ModulesGroup>();
            this.Timer.Tick += Tick;
            Timer.Interval = TIMER_INTERVAL_DEFAULT;
            this.control = control;

            //Parse XML
            Nombre = elemento.Attribute("name") == null ? "undefined" : elemento.Attribute("name").Value;

            foreach (XElement elementoHijo in elemento.Elements()) {
                switch (elementoHijo.Name.LocalName) {
                    case "key-step":
                        ModulesGroup.Add(new ModulesGroup(elementoHijo));
                        break;
                    default:
                        throw new Exception("Etiqueta no soportada " + elementoHijo.Name);
                }
            }
        }

        public Animation(XElement elemento)
        {
            this.Timer = new Timer();
            this.ModulesGroup = new List<ModulesGroup>();
            this.Timer.Tick += Tick;
            Timer.Interval = TIMER_INTERVAL_DEFAULT;

            //Parse XML
            Nombre = elemento.Attribute("name") == null ? "undefined" : elemento.Attribute("name").Value;

            foreach (XElement elementoHijo in elemento.Elements())
            {
                switch (elementoHijo.Name.LocalName)
                {
                    case "key-step":
                        ModulesGroup.Add(new ModulesGroup(elementoHijo));
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
                this.control = control;

                ModulesGroup[pasosReproducidos].prepare(control, Timer.Interval);
                sendSignal(ModulesGroup[pasosReproducidos].StartSignal, OnStartSignal);

                //Empezamos el timer
                Timer.Enabled = true;
            }
        }

        private void PasoAcabado()
        {
            Timer.Enabled = false;

            //Loops de los grupos
            if (repeticiones != ModulesGroup[pasosReproducidos].Repeticiones) {
                repeticiones++;
                ModulesGroup[pasosReproducidos].prepare(control, Timer.Interval);
                Timer.Enabled = true;
                return;
            }

            //Si es el final
            sendSignal(ModulesGroup[pasosReproducidos].EndSignal, OnEndSignal);

            pasosReproducidos++;

            repeticiones = 0;

            //Comprobamos si ya hemos acabado todos los grupos
            if (pasosReproducidos != ModulesGroup.Count)
            {
                ModulesGroup[pasosReproducidos].prepare(control, Timer.Interval);
                sendSignal(ModulesGroup[pasosReproducidos].StartSignal, OnStartSignal);
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

            foreach (AnimationModule module in ModulesGroup[pasosReproducidos].Animations) {
                finPasos = module.setpForward(control);
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

