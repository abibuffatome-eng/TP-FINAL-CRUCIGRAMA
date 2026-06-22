using CrucigramaForms.Modelos;
using System;

namespace CrucigramaForms.Logica
{
    internal class cPuntaje
    {
        private const int PENALIZACION_POR_INTENTO = 10; // ajustá este número a gusto

        // puntaje actual, se puede consultar en cualquier momento (completo o no)
        public static int CalcularPuntajeActual(Crucigrama crucigrama)
        {
            int puntaje = crucigrama.Nivel.PuntajeBase - (crucigrama.IntentosFallidos * PENALIZACION_POR_INTENTO);
            return Math.Max(puntaje, 0);
        }
        public static int CalcularPuntaje(Crucigrama crucigrama)
        {
            // si no está completo, no hay puntaje (no debería llegar acá, pero por seguridad)
            if (!crucigrama.EstaCompleto()) return 0;
            return CalcularPuntajeActual(crucigrama); // nunca negativo
        }
    }
}