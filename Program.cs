using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lexico_1;

namespace Prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Lexico T = new Lexico())
            {
                T.SetContenido("Hola");
                T.SetClasificacion(Token.Tipos.Identificador);

                Console.WriteLine(T.GetContenido() + " = " + T.GetClasificacion());

                T.SetContenido("1345");
                T.SetClasificacion(Token.Tipos.Numero);

                Console.WriteLine(T.GetContenido() + " = " + T.GetClasificacion());
            }
        }
    }
}