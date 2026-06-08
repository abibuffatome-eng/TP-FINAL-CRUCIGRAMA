using System;
using System.Collections.Generic;
using System.Text;

namespace CrucigramaForms.Modelos
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public string TipoUsuario { get; set; }

        public Usuario() { }

        public Usuario(int id, string nombre, string contrasena, string tipoUsuario)
        {
            Id = id;
            Nombre = nombre;
            Contrasena = contrasena;
            TipoUsuario = tipoUsuario;
        }

        public bool EsAdmin() => TipoUsuario == "admin";

        public override string ToString() => $"{Nombre} ({TipoUsuario})";
    }
}
