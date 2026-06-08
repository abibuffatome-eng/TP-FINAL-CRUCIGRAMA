using System;
using System.Collections.Generic;
using System.Text;

namespace CrucigramaForms.Modelos
{
    internal class Palabra
    {
        public int Id { get; set; }
        public int CrucigramaId { get; set; }
        public string Texto { get; set; }
        public string Pista { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public string Orientacion { get; set; }

        public Palabra() { }

        public Palabra(int id, int crucigramaId, string texto, string pista, int fila, int columna, string orientacion)
        {
            Id = id;
            CrucigramaId = crucigramaId;
            Texto = texto.ToUpper();
            Pista = pista;
            Fila = fila;
            Columna = columna;
            Orientacion = orientacion;
        }

        public bool EsHorizontal() => Orientacion == "horizontal";

        public override string ToString() => $"[{Orientacion[0].ToString().ToUpper()}] ({Fila},{Columna}) {Texto} — {Pista}";
    }
}
