using CrucigramaForms.Controladores;
using CrucigramaForms.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace CrucigramaForms.Persistencia
{
    internal class pPartida
    {
        public void Agregar(Partida partida)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Partidas (UsuarioId, CrucigramaId, Puntaje, Fecha)
                VALUES (@usuarioId, @crucigramaId, @puntaje, @fecha)";

            cmd.Parameters.AddWithValue("@usuarioId", partida.UsuarioId);
            cmd.Parameters.AddWithValue("@crucigramaId", partida.CrucigramaId);
            cmd.Parameters.AddWithValue("@puntaje", partida.Puntaje);
            cmd.Parameters.AddWithValue("@fecha", partida.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));

            cmd.ExecuteNonQuery();
        }

        public List<Partida> ObtenerPorUsuario(int usuarioId)
        {
            var lista = new List<Partida>();

            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                SELECT * FROM Partidas 
                WHERE UsuarioId = @usuarioId 
                ORDER BY Fecha DESC";

            cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(MapearPartida(reader));

            return lista;
        }

        public List<RankingItem> ObtenerRanking()
        {
            var lista = new List<RankingItem>();
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            // Usamos INNER JOIN para unir Partidas con Usuarios mediante el UsuarioId
            cmd.CommandText = @"
        SELECT U.Nombre, SUM(P.Puntaje) as PuntajeTotal
        FROM Partidas P
        INNER JOIN Usuarios U ON P.UsuarioId = U.Id
        GROUP BY U.Id, U.Nombre
        ORDER BY PuntajeTotal DESC 
        LIMIT 10";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new RankingItem
                {
                    NombreUsuario = reader.GetString(0), // U.Nombre
                    PuntajeTotal = Convert.ToInt32(reader.GetValue(1)) // SUM(P.Puntaje)
                });
            }
            return lista;
        }

        public int ObtenerMejorPuntaje(int usuarioId)
        {
            using var conexion = Conexion.ObtenerConexion();
            using var cmd = conexion.CreateCommand();

            cmd.CommandText = @"
                SELECT COALESCE(MAX(Puntaje), 0) 
                FROM Partidas 
                WHERE UsuarioId = @usuarioId";

            cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

            return (int)(long)cmd.ExecuteScalar();
        }

        private Partida MapearPartida(SQLiteDataReader reader)
        {
            return new Partida
            {
                Id = reader.GetInt32(0),
                UsuarioId = reader.GetInt32(1),
                CrucigramaId = reader.GetInt32(2),
                Puntaje = reader.GetInt32(3),
                Fecha = System.DateTime.Parse(reader.GetString(4))
            };
        }
    }
}
