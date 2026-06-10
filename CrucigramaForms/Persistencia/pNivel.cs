using CrucigramaForms.Controladores;
using CrucigramaForms.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace CrucigramaForms.Persistencia
{
    internal class pNivel
    {
        public List<Nivel> ObtenerTodos()
        {
            var lista = new List<Nivel>();

            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Niveles";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearNivel(reader));

            return lista;
        }

        public Nivel ObtenerPorId(int id)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Niveles WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapearNivel(reader);

            return null;
        }

        private Nivel MapearNivel(SQLiteDataReader reader)
        {
            return new Nivel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetInt32(3),
                reader.GetInt32(4)
            );
        }
    }
}
