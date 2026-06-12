using System;
using System.Collections.Generic;
using System.Text;

namespace CrucigramaForms.Modelos
{
    internal class Celda
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public char LetraCorrecta { get; set; }
        public char LetraIngresada { get; set; }
        public bool EstaBloqueada { get; set; }

        public Celda() 
        { 

        }

        public Celda(int fila, int columna, char letraCorrecta, bool estaBloqueada)
        {
            Fila = fila;
            Columna = columna;
            LetraCorrecta = letraCorrecta;
            EstaBloqueada = estaBloqueada;
            LetraIngresada = '\0';
        }

        public bool EstaVacia() => LetraIngresada == '\0';

        public bool EsCorrecta() => !EstaBloqueada && char.ToUpper(LetraIngresada) == char.ToUpper(LetraCorrecta);

        public override string ToString() => EstaBloqueada ? "■" : (EstaVacia() ? "_" : LetraIngresada.ToString());
    }
}
