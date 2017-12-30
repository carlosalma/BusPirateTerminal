using System;

// TODO: buscar info sobre: _serialPort.WriteLine(String.Format("<{0}>: {1}", name, message));

namespace BusPirateTerminal
{
    class Consola
    {
        public string Prompt { get; }
        public string PromptPB { get; }
        public string Version { get; }
        public ConsoleColor ColorTexto { get; }

        public Consola()
        {
            Prompt = "::> ";
            Version = "0.3";
            ColorTexto = ConsoleColor.Red;
        }

        /// <summary>
        ///   Cabecera
        /// </summary>
        public void MsgPresentacion()
        {
            Console.ForegroundColor = ColorTexto;
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Consola de acceso a Bus Pirate");
            Console.WriteLine("----------------------------------------------");
        }

        /// <summary>
        ///   Cuerpo
        /// </summary>
        /// <param name="comPort">
        /// </param>
        public void MsgConexionEstablecida(string comPort)
        {
            Console.WriteLine(value: $"{Prompt}Conexión: ESTABLECIDA en puerto {comPort}");
            Console.WriteLine(value: $"{Prompt}Para salir de la consola, teclear: quit");
            Console.WriteLine("----------------------------------------------");
        }

        /// <summary>
        ///   Muestra el número de puertos serie disponibles
        ///   en el dispositivo y los lista.
        /// </summary>
        public void MsgListadoPuertos()
        {
            SerialCom conexionSerie = new SerialCom();

            string[] puertos = conexionSerie.ListarPuertosDisponibles();

            string listado = "Listado de puertos: \n";

            foreach (string puerto in puertos)
            {
                listado += puerto + "\n";
            }

            Console.WriteLine(value: $"{Prompt}Número de puertos serie disponibles: {puertos.Length}");
            Console.WriteLine(value: $"{Prompt}{listado}");
        }
    }
}
