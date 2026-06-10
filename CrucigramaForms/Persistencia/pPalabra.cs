using CrucigramaForms.Controladores;
using CrucigramaForms.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace CrucigramaForms.Persistencia
{
    internal class pPalabra
    {
        public void Agregar(Palabra palabra)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Palabras (CrucigramaId, Texto, Pista, Fila, Columna, Orientacion)
                VALUES (@crucigramaId, @texto, @pista, @fila, @columna, @orientacion);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("@crucigramaId", palabra.CrucigramaId);
            cmd.Parameters.AddWithValue("@texto", palabra.Texto);
            cmd.Parameters.AddWithValue("@pista", palabra.Pista);
            cmd.Parameters.AddWithValue("@fila", palabra.Fila);
            cmd.Parameters.AddWithValue("@columna", palabra.Columna);
            cmd.Parameters.AddWithValue("@orientacion", palabra.Orientacion);

            palabra.Id = (int)(long)cmd.ExecuteScalar();
        }

        public void Eliminar(int id)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "DELETE FROM Palabras WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public void Actualizar(Palabra palabra)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                UPDATE Palabras 
                SET Texto = @texto, Pista = @pista, Fila = @fila, Columna = @columna, Orientacion = @orientacion
                WHERE Id = @id";

            cmd.Parameters.AddWithValue("@texto", palabra.Texto);
            cmd.Parameters.AddWithValue("@pista", palabra.Pista);
            cmd.Parameters.AddWithValue("@fila", palabra.Fila);
            cmd.Parameters.AddWithValue("@columna", palabra.Columna);
            cmd.Parameters.AddWithValue("@orientacion", palabra.Orientacion);
            cmd.Parameters.AddWithValue("@id", palabra.Id);

            cmd.ExecuteNonQuery();
        }

        public List<Palabra> ObtenerPorCrucigrama(int crucigramaId)
        {
            var lista = new List<Palabra>();

            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Palabras WHERE CrucigramaId = @crucigramaId";
            cmd.Parameters.AddWithValue("@crucigramaId", crucigramaId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearPalabra(reader));

            return lista;
        }

        private Palabra MapearPalabra(SQLiteDataReader reader)
        {
            return new Palabra(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetInt32(4),
                reader.GetInt32(5),
                reader.GetString(6)
            );
        }
    }
}
