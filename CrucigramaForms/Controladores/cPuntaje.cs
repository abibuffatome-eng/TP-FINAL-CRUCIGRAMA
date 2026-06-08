using CrucigramaForms.Modelos;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CrucigramaForms.Logica
{
    internal class cPuntaje
    {
        public static int CalcularPuntaje(Crucigrama crucigrama)
        {
            int palabrasCorrectas = 0;

            foreach (var palabra in crucigrama.Palabras)
                if (crucigrama.PalabraEsCorrecta(palabra))
                    palabrasCorrectas++;

            if (palabrasCorrectas == 0) return 0;

            int totalPalabras = crucigrama.Palabras.Count;
            int puntajeBase = crucigrama.Nivel.PuntajeBase;
            double proporcion = (double)palabrasCorrectas / totalPalabras;
            int puntaje = (int)(puntajeBase * proporcion);

            if (palabrasCorrectas == totalPalabras)
                puntaje += puntajeBase / 2;

            return puntaje;
        }
    }
}
