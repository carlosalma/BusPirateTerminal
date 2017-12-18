using System;
using CommandLineParser.Arguments;
using System.IO.Ports;

namespace BusPirateTerminal
{
    // TODO: verificar los textos con las definiciones de los parámetros de comunicación.
    // TODO: traducir los textos.

    /// <summary>
    /// Define los parámetros
    /// </summary>
    class Parametros
    {
        //
        private string modo;
        [EnumeratedValueArgument(typeof(string), 'm', "modo", 
            AllowedValues = "pirate;manual",
            Description = 
                "Modos de configuración: <pirate> auto-configuración, <manual> configuración manual.",
            IgnoreCase = true,
            FullDescription =  
                "Existen dos modos de configuración, el modo 'pirate' que configura automáticamente \n " +
                "los parámetros de comunicación con 'Bus Pirate' y el modo 'manual', que requiere \n " +
                "de la introducción de todos los parámetros de comunicación.", 
            Example = "\n" +
                "- Modo automático: MicroTerminal.exe || MicroTerminal.exe -m pirate \n" +
                "- Modo manual: MicroTerminal.exe - manual -p COM3 -s 115200 -a none -b 8 -i 1")]
        public string Modo { get => modo; set => modo = value; }
        //
        private string port;
        [ValueArgument(typeof(string), 'p', "port", Description = "Número de puerto COM")]
        public string Port { get => port; set => port = value; }
        //
        private int speed;
        [ValueArgument(typeof(int), 's', "speed", Description = "Velocidad de comunicación en bps")]
        public int Speed { get => speed; set => speed = value; }
        //
        private Parity parity;
        [ValueArgument(typeof(Parity), 'a', "parity", Description = "Bit de paridad")]
        public Parity Parity { get => parity; set => parity = value; }
        //
        private int combits;
        [ValueArgument(typeof(int), 'b', "combits", Description = "Número de bits de comunicación")]
        public int Combits { get => combits; set => combits = value; }
        //
        private StopBits stopbits;
        [ValueArgument(typeof(StopBits), 'i', "stopbits", Description = "Bit de parada")]
        public StopBits Stopbits { get => stopbits; set => stopbits = value; }
        //
        private bool help;
        [SwitchArgument('h', "help", true, Description = "Esta ayuda")]
        public bool Help { get => help; set => help = value; }
        //
        private bool version;
        [SwitchArgument('v', "version", true, Description = "Versión")]
        public bool Version { get => version; set => version = value; }
        //
        public string Cabecera { get; }
        public string Pie { get; }
        
        //
        // Constructor
        //
        public Parametros()
        {
            Cabecera = "\n" +
                "Mini terminal de puerto serie que permite establecer comunicación con \n" +
                "el interface de 'Bus Pirate'. \n";

            Pie = "Autor: Carlos AlMa - 2017 - (Ver 0.11) \n";
        }

        //
        // Métodos
        //

        /// <summary>
        /// Muestra la ayuda sobre el uso de los parámetros
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="param">parámetros</param>
        public void MostrarAyudaParametros(CommandLineParser.CommandLineParser parser, Parametros param)
        {
            parser.ShowUsageHeader = param.Cabecera;
            parser.ShowUsageFooter = param.Pie;
            parser.ShowUsage();
        }
    }
}
