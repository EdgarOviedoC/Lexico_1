using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

/*
    1) Sobrecargar el constructor lexico para que reciba como argumento el nombre del archivo a compilar
    2) Tener un contador de lineas 
    3) Agregar operador relacional  
        ==, >,>=, <, <=, <>, !
    4) Agregar Operador logico
        ||, &&, !
*/
namespace Lexico_1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        //int line;
        StreamWriter error;

        public Lexico()
        {
            error = new StreamWriter("Errores.error");
            error.AutoFlush = true;

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
                throw new Error("El archivo pueba.cpp no existe", error);
            }
        }

        public Lexico(string nombreArchivo)
        {
            error = new StreamWriter("Errores.error");
            error.AutoFlush = true;

            if (Path.GetExtension(nombreArchivo) == ".cpp" && File.Exists(Path.ChangeExtension(nombreArchivo, ".cpp")))
            {
                archivo = new StreamReader(nombreArchivo);
                log = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".log"));
                asm = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".asm"));
                log.AutoFlush = true;
                asm.AutoFlush = true;
            }
            else
            {
                throw new Error("El archivo " + nombreArchivo + " No existe o extension invalida", error);
            }
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

            Buffer += c;

            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Identificador);
                while (char.IsLetterOrDigit(c = (char)archivo.Peek()))
                {
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == ';')
            {
                setClasificacion(Tipos.FinSentencia);
            }
            else if (c == '{')
            {
                setClasificacion(Tipos.InicioBloque);
            }
            else if (c == '}')
            {
                setClasificacion(Tipos.FinBloque);
            }
            else if (c == '?')
            {
                setClasificacion(Tipos.OperadorTernario);
            }
            else if (c == '=')
            {
                setClasificacion(Tipos.Asignacion);
            }
            else if (c == '+')
            {
                setClasificacion(Tipos.OperadorTermino);

                if ((c = (char)archivo.Peek()) == '+' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '-')
            {
                setClasificacion(Tipos.OperadorTermino);

                if ((c = (char)archivo.Peek()) == '-' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    Buffer += c;
                    archivo.Read();
                }
                else if (c == '>')
                {
                    setClasificacion(Tipos.Puntero);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '*' || c == '/' || c == '%')
            {
                setClasificacion(Tipos.OperadorFactor);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else
            {
                setClasificacion(Tipos.Caracter);
            }
            if (!finArchivo())
            {
                setContenido(Buffer);
                log.WriteLine(getContenido() + "  =  " + getClasificacion());
            }
            //archivo.read();
            //archivo.peek();
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}