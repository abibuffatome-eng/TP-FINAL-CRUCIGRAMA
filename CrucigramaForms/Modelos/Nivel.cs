using System;
using System.Collections.Generic;
using System.Text;

namespace CrucigramaForms.Modelos
{
    internal class Nivel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Filas { get; set; }
        public int Columnas { get; set; }
        public int PuntajeBase { get; set; }

        public Nivel() { }

        public Nivel(int id, string nombre, int filas, int columnas, int puntajeBase)
        {
            Id = id;
            Nombre = nombre;
            Filas = filas;
            Columnas = columnas;
            PuntajeBase = puntajeBase;

        }

        public override string ToString() => $"{Nombre} ({Filas}x{Columnas})";
    }
}
