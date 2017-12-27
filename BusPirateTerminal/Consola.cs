using System;

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
    }
}
