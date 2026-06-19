using CrucigramaForms.Modelos;
using System;

namespace CrucigramaForms.Logica
{
    internal class cPuntaje
    {
        private const int PENALIZACION_POR_INTENTO = 10; // ajustá este número a gusto

        public static int CalcularPuntaje(Crucigrama crucigrama)
        {
            // si no está completo, no hay puntaje (no debería llegar acá, pero por seguridad)
            if (!crucigrama.EstaCompleto()) return 0;

            int puntaje = crucigrama.Nivel.PuntajeBase - (crucigrama.IntentosFallidos * PENALIZACION_POR_INTENTO);

            return Math.Max(puntaje, 0); // nunca negativo
        }
    }
}