using System;
using System.Collections.Generic;
using System.Text;

namespace CrucigramaForms.Modelos
{
    internal class Crucigrama
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public Nivel Nivel { get; set; }
        public List<Palabra> Palabras { get; set; }
        public Celda[,] Grilla { get; set; }

        public Crucigrama()
        {
            Palabras = new List<Palabra>();
        }

        public Crucigrama(int id, string titulo, Nivel nivel)
        {
            Id = id;
            Titulo = titulo;
            Nivel = nivel;
            Palabras = new List<Palabra>();
        }

        public void ConstruirGrilla()
        {
            Grilla = new Celda[Nivel.Filas, Nivel.Columnas];

            for (int f = 0; f < Nivel.Filas; f++)
                for (int c = 0; c < Nivel.Columnas; c++)
                    Grilla[f, c] = new Celda(f, c, '\0', true);

            foreach (var palabra in Palabras)
                UbicarPalabra(palabra);
        }

        private void UbicarPalabra(Palabra palabra)
        {
            for (int i = 0; i < palabra.Texto.Length; i++)
            {
                int fila = palabra.EsHorizontal() ? palabra.Fila : palabra.Fila + i;
                int columna = palabra.EsHorizontal() ? palabra.Columna + i : palabra.Columna;

                if (fila < Nivel.Filas && columna < Nivel.Columnas)
                    Grilla[fila, columna] = new Celda(fila, columna, palabra.Texto[i], false);
            }
        }

        public bool EstaCompleto()
        {
            foreach (var palabra in Palabras)
                if (!PalabraEsCorrecta(palabra)) return false;
            return true;
        }

        public bool PalabraEsCorrecta(Palabra palabra)
        {
            for (int i = 0; i < palabra.Texto.Length; i++)
            {
                int fila = palabra.EsHorizontal() ? palabra.Fila : palabra.Fila + i;
                int columna = palabra.EsHorizontal() ? palabra.Columna + i : palabra.Columna;

                if (!Grilla[fila, columna].EsCorrecta()) return false;
            }
            return true;
        }

        public override string ToString() => $"{Titulo} — {Nivel}";
    }
}
