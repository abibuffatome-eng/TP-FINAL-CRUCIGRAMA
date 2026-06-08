using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace CrucigramaForms.Controladores
{
    internal class Conexion
    
    {
        // Gestiona la conexión y la inicialización de la base de datos SQLite.
        
            private static readonly string RutaArchivo = "crucigrama.db";
            private static readonly string CadenaConexion = $"Data Source={RutaArchivo};Version=3;";

            public static SQLiteConnection ObtenerConexion()
            {
                var conexion = new SQLiteConnection(CadenaConexion);
                conexion.Open();
                return conexion;
            }

            public static void InicializarBaseDeDatos()
            {
                using var conexion = ObtenerConexion();
                using var cmd = conexion.CreateCommand();

                cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre      TEXT    NOT NULL UNIQUE,
                    Contrasena  TEXT    NOT NULL,
                    TipoUsuario TEXT    NOT NULL CHECK(TipoUsuario IN ('admin', 'jugador'))
                );

                CREATE TABLE IF NOT EXISTS Niveles (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre      TEXT    NOT NULL UNIQUE,
                    Filas       INTEGER NOT NULL,
                    Columnas    INTEGER NOT NULL,
                    PuntajeBase INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Crucigramas (
                    Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo   TEXT    NOT NULL,
                    NivelId  INTEGER NOT NULL,
                    FOREIGN KEY (NivelId) REFERENCES Niveles(Id)
                );

                CREATE TABLE IF NOT EXISTS Palabras (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    CrucigramaId INTEGER NOT NULL,
                    Texto       TEXT    NOT NULL,
                    Pista       TEXT    NOT NULL,
                    Fila        INTEGER NOT NULL,
                    Columna     INTEGER NOT NULL,
                    Orientacion TEXT    NOT NULL CHECK(Orientacion IN ('horizontal', 'vertical')),
                    FOREIGN KEY (CrucigramaId) REFERENCES Crucigramas(Id)
                );

                CREATE TABLE IF NOT EXISTS Partidas (
                    Id           INTEGER PRIMARY KEY AUTOINCREMENT,
                    UsuarioId    INTEGER NOT NULL,
                    CrucigramaId INTEGER NOT NULL,
                    Puntaje      INTEGER NOT NULL,
                    Fecha        TEXT    NOT NULL,
                    FOREIGN KEY (UsuarioId)    REFERENCES Usuarios(Id),
                    FOREIGN KEY (CrucigramaId) REFERENCES Crucigramas(Id)
                );";

                cmd.ExecuteNonQuery();
                InsertarNivelesPorDefecto(conexion);
            }

            private static void InsertarNivelesPorDefecto(SQLiteConnection conexion)
            {
                using var cmd = conexion.CreateCommand();
                cmd.CommandText = @"
                INSERT OR IGNORE INTO Niveles (Nombre, Filas, Columnas, PuntajeBase)
                VALUES
                    ('Fácil',  5,  5,  100),
                    ('Medio',  10, 10, 250),
                    ('Difícil',15, 15, 500);";
                cmd.ExecuteNonQuery();
            }
        }
    }

