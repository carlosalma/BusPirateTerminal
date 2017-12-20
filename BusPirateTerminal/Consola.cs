using System;

namespace BusPirateTerminal
{
    class Consola
    {
        public string Prompt { get; }
        public string Version { get; }

        public Consola()
        {
            Prompt = "::> ";
            Version = "0.0.1";
        }

        public void MsgPresentacion()
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Consola de acceso a Bus Pirate");
            Console.WriteLine("----------------------------------------------");
        }

        public void MsgConexionEstablecida(string comPort)
        {
            Console.WriteLine(value: $"{Prompt}Conexión: ESTABLECIDA");
            Console.WriteLine(value: $"{Prompt}Puerto: {comPort}");
            Console.WriteLine(value: $"{Prompt}Para salir de la consola, teclear: quit");
            Console.WriteLine("----------------------------------------------");
        }
    }
}
