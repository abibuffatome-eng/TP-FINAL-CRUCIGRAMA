using CrucigramaForms.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrucigramaForms.Logica
{
    internal class cJuego
    {
        public static void IniciarPartida(Crucigrama crucigrama)
        {
            crucigrama.ConstruirGrilla();
        }

        public static bool IngresarLetra(Crucigrama crucigrama, int fila, int columna, char letra)
        {
            Celda celda = crucigrama.Grilla[fila, columna];

            if (celda.EstaBloqueada) return false;

            celda.LetraIngresada = char.ToUpper(letra);
            return true;
        }

        public static bool BorrarLetra(Crucigrama crucigrama, int fila, int columna)
        {
            Celda celda = crucigrama.Grilla[fila, columna];

            if (celda.EstaBloqueada) return false;

            celda.LetraIngresada = '\0';
            return true;
        }

        public static bool VerificarPalabra(Crucigrama crucigrama, Palabra palabra)
        {
            return crucigrama.PalabraEsCorrecta(palabra);
        }

        public static bool VerificarCrucigrama(Crucigrama crucigrama)
        {
            return crucigrama.EstaCompleto();
        }

        public static Partida FinalizarPartida(Crucigrama crucigrama, int usuarioId)
        {
            int puntaje = cPuntaje.CalcularPuntaje(crucigrama);
            return new Partida(usuarioId, crucigrama.Id, puntaje);
        }
    }
}
