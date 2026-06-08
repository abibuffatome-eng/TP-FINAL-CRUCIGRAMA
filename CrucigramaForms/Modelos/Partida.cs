using System;
using System.Collections.Generic;
using System.Text;

namespace CrucigramaForms.Modelos
{
    internal class Partida
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CrucigramaId { get; set; }
        public int Puntaje { get; set; }
        public DateTime Fecha { get; set; }

        public Partida() { }

        public Partida(int usuarioId, int crucigramaId, int puntaje)
        {
            UsuarioId = usuarioId;
            CrucigramaId = crucigramaId;
            Puntaje = puntaje;
            Fecha = DateTime.Now;
        }

        public override string ToString() => $"{Fecha:dd/MM/yyyy HH:mm} — Puntaje: {Puntaje}";
    }
}
