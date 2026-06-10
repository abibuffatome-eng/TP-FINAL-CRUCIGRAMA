using CrucigramaForms.Controladores;
using CrucigramaForms.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace CrucigramaForms.Persistencia
{
    internal class pUsuario
    {
        //buenas hechas las persistencias por abi 
        public void Agregar(Usuario usuario)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Usuarios (Nombre, Contrasena, TipoUsuario)
                VALUES (@nombre, @contrasena, @tipo)";

            cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
            cmd.Parameters.AddWithValue("@tipo", usuario.TipoUsuario);

            cmd.ExecuteNonQuery();
        }

        public Usuario ObtenerPorNombre(string nombre)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Usuarios WHERE Nombre = @nombre";
            cmd.Parameters.AddWithValue("@nombre", nombre);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapearUsuario(reader);

            return null;
        }

        public Usuario Login(string nombre, string contrasena)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                SELECT * FROM Usuarios 
                WHERE Nombre = @nombre AND Contrasena = @contrasena";

            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@contrasena", contrasena);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapearUsuario(reader);

            return null;
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();

            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Usuarios WHERE TipoUsuario = 'jugador'";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearUsuario(reader));

            return lista;
        }

        public bool ExisteNombre(string nombre)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM Usuarios WHERE Nombre = @nombre";
            cmd.Parameters.AddWithValue("@nombre", nombre);

            return (long)cmd.ExecuteScalar() > 0;
        }

        private Usuario MapearUsuario(SQLiteDataReader reader)
        {
            return new Usuario(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            );
        }
    }
}
