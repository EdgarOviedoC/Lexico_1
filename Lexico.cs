using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

/* Proyecto
-------------------------- Terminado ---------------------------
 1) Agregar token cadena. 
    Ejemplos
     Cadena vacia ""
     Cadena con texto "Hola mundo"
     Cadena si no se cierra las comillas error lexico
----------------------------------------------------------------
----------------------------------------------------------------
 2) Numero. Error lexico si despues del punto no hay numero
----------------------------------------------------------------
----------------------------------------------------------------
 3) Notacion exponencial E-8 ó E+8
    Tambien se puede con el exponenete en numeros decimales
    si despues de la E no viene un + ó - Error lexico
----------------------------------------------------------------
---------------------- Terminado -------------------------------
 4)#Digitos es un carecter pero concatenar numeros despues del #
    '@' es un caracter si esta entre comillas
-----------------------------------------------------------------
*/
namespace Lexico_1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int line = 1;

        //Constructor de la clase lexico
        public Lexico()
        {
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

        //Constructor sobrecargado
        public Lexico(string nombreArchivo)
        {
            log = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".log"));
            log.AutoFlush = true;

            if (File.Exists(Path.ChangeExtension(nombreArchivo, ".cpp")))
            {
                archivo = new StreamReader(nombreArchivo);
            }
            else
            {
                throw new Error("El archivo " + nombreArchivo + " no existe", log);
            }

            if (Path.GetExtension(nombreArchivo) == ".cpp")
            {
                asm = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".asm"));
                asm.AutoFlush = true;
            }
            else
            {
                throw new Error("El archivo tiene extension invalida", log);
            }

        }

        //Destructor de la clase lexico
        public void Dispose()
        {
            log.WriteLine("Total de lineas {0}", line);

            log.Close();
            archivo.Close();
            asm.Close();
        }

        public void nexToken()
        {
            char c;
            string Buffer = "";

            while (char.IsWhiteSpace(c = (char)archivo.Peek()))
            {
                ContadorLineas(c);
                archivo.Read();
            }

            Buffer += c;
            archivo.Read();

            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Caracter);

                while (char.IsLetterOrDigit(c = (char)archivo.Peek()))
                {
                    setClasificacion(Tipos.Identificador);
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

                if (c == '.')
                {
                    Buffer += c;
                    archivo.Read();
                    if (!(char.IsDigit(c = (char)archivo.Peek())))
                    {
                        throw new Error("Lexic error: Expected \"digit\" ", log, line);
                    }
                    else
                    {
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            Buffer += c;
                            archivo.Read();
                        }
                    }
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
            else if (c == '$')
            {
                setClasificacion(Tipos.Caracter);

                if (Char.IsDigit(c = (char)archivo.Peek()))
                {
                    setClasificacion(Tipos.Moneda);

                    while (char.IsDigit(c = (char)archivo.Peek()))
                    {
                        Buffer += c;
                        archivo.Read();
                    }
                }
            }
            else if (c == '=')
            {
                setClasificacion(Tipos.Asignacion);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '|')
            {
                setClasificacion(Tipos.Caracter);

                if ((c = (char)archivo.Peek()) == '|')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '&')
            {
                setClasificacion(Tipos.Caracter);

                if ((c = (char)archivo.Peek()) == '&')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '!')
            {
                setClasificacion(Tipos.OperadorLogico);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '<')
            {
                setClasificacion(Tipos.OperadorRelacional);

                if ((c = (char)archivo.Peek()) == '=' || c == '>')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '>')
            {
                setClasificacion(Tipos.OperadorRelacional);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }
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
            else if (c == '"')
            {
                setClasificacion(Tipos.Cadena);

                while (!((c = (char)archivo.Peek()) == '"'))
                {
                    Buffer += c;
                    archivo.Read();

                    if (finArchivo())
                    {
                        archivo.Read();
                        throw new Error("Lexico error: Expected \'\"\' ", log, line);
                    }
                }
                Buffer += c;
                archivo.Read();
            }
            else if (c == '\'')
            {
                setClasificacion(Tipos.Caracter);

                c = (char)archivo.Read();

                Buffer += c;

                c = (char)archivo.Peek();

                if (c != '\'')
                {
                    throw new Error("Lexico error: Expected ''' ", log, line);
                }
                else
                {
                    Buffer += c;
                    archivo.Read();
                }
            }
            else
            {
                setClasificacion(Tipos.Caracter);
            }

            setContenido(Buffer);
            log.WriteLine("{0}  °°°°  {1}", getContenido(), getClasificacion());
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }

        public void ContadorLineas(int c)
        {
            if (c == 10)
            {
                line++;
            }
        }
    }
}