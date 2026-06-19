using CrucigramaForms.Controladores;
using CrucigramaForms.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace CrucigramaForms.Persistencia
{
    internal class pCrucigrama
    {
        private readonly pNivel _nivelRepo = new pNivel();

        public void Agregar(Crucigrama crucigrama)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Crucigramas (Titulo, NivelId)
                VALUES (@titulo, @nivelId);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("@titulo", crucigrama.Titulo);
            cmd.Parameters.AddWithValue("@nivelId", crucigrama.Nivel.Id);

            crucigrama.Id = (int)(long)cmd.ExecuteScalar();
        }

        public void Eliminar(int id)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "DELETE FROM Palabras WHERE CrucigramaId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DELETE FROM Crucigramas WHERE Id = @id";
            cmd.ExecuteNonQuery();
        }

        public void ActualizarTitulo(int id, string nuevoTitulo)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "UPDATE Crucigramas SET Titulo = @titulo WHERE Id = @id";
            cmd.Parameters.AddWithValue("@titulo", nuevoTitulo);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public Crucigrama ObtenerPorId(int id)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Crucigramas WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapearCrucigrama(reader);

            return null;
        }

        public List<Crucigrama> ObtenerTodos()
        {
            var lista = new List<Crucigrama>();

            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Crucigramas";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearCrucigrama(reader));

            return lista;
        }

        public List<Crucigrama> ObtenerPorNivel(int nivelId)
        {
            var lista = new List<Crucigrama>();

            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = "SELECT * FROM Crucigramas WHERE NivelId = @nivelId";
            cmd.Parameters.AddWithValue("@nivelId", nivelId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearCrucigrama(reader));

            return lista;
        }

        private Crucigrama MapearCrucigrama(SQLiteDataReader reader)
        {
            int id = reader.GetInt32(0);
            string titulo = reader.GetString(1);
            int nivelId = reader.GetInt32(2);

            Nivel nivel = _nivelRepo.ObtenerPorId(nivelId);

            var cruc = new Crucigrama(id, titulo, nivel);
            // load palabras for this crucigrama
            var palRepo = new pPalabra();
            cruc.Palabras = palRepo.ObtenerPorCrucigrama(id);

            return cruc;
        }
    }
}
