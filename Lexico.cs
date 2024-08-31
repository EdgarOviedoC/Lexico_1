using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

/*
    1) Sobrecargar el constructor lexico para que reciba como argumento el nombre del archivo a compilar
    2) Tener un contador de lineas 
*/
namespace Lexico_1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int linea;

        public Lexico()
        {
            linea = 1;
            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush = true;
            asm.AutoFlush = true;

            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo pueba.cpp no existe", log);
            }
        }

        public Lexico(string nombreArchivo)
        {
            log = new StreamWriter(nombreArchivo);
            asm = new StreamWriter(nombreArchivo);
            log.AutoFlush = true;
            asm.AutoFlush = true;

            /*
                Si nombre = suma.cpp
                log = suma.log
                asm = suma.asm
                y validar la extencion del nombre del archivo 
            */
        }

        //Destructor de la clase lexico
        public void Dispose()
        {
            archivo.Close();
            log.Close();
            asm.Close();
        }
        //
        public void nexToken()
        {
            char c;
            string Buffer = "";

            while (char.IsWhiteSpace(c = (char)archivo.Read()))
            {
            }
            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Identificador);
            }
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
            }
            else
            {
                setClasificacion(Tipos.Caracter);
            }

            setContenido(Buffer);
            log.Write(getContenido() + getClasificacion());

            //archivo.read();
            //archivo.peek();
        }
    }
}