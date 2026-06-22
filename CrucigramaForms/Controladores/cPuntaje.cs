using CrucigramaForms.Modelos;
using System;

namespace CrucigramaForms.Logica
{
    internal class cPuntaje
    {
        private const int penalizacion_de_intentos = 10; 

        // puntaje actual, se puede consultar en cualquier momento (completo o no)
        public static int CalcularPuntajeActual(Crucigrama crucigrama)
        {
            int puntaje = crucigrama.Nivel.PuntajeBase - (crucigrama.IntentosFallidos * penalizacion_de_intentos);
            return Math.Max(puntaje, 0);
        }
        public static int CalcularPuntaje(Crucigrama crucigrama)
        {
            if (!crucigrama.EstaCompleto()) return 0;
            return CalcularPuntajeActual(crucigrama); // nunca negativo
        }
    }
}