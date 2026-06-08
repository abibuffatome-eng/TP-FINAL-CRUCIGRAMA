using CrucigramaForms.Controladores;
using CrucigramaForms.Modelos;
using System.Diagnostics;

namespace CrucigramaForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());


            // Program.cs — bloque de prueba temporal
            Conexion.InicializarBaseDeDatos();

            // Probar que la grilla se arma bien
            var nivel = new Nivel(1, "Fácil", 5, 5, 100);
            var crucigrama = new Crucigrama(1, "Prueba", nivel);

            crucigrama.Palabras.Add(new Palabra(1, 1, "GATO", "Animal doméstico", 0, 0, "horizontal"));
            crucigrama.Palabras.Add(new Palabra(2, 1, "GATO", "Empieza con G", 0, 0, "vertical"));

            crucigrama.ConstruirGrilla();

            // Imprimir grilla en consola de depuración
            for (int f = 0; f < nivel.Filas; f++)
            {
                for (int c = 0; c < nivel.Columnas; c++)
                    Debug.Write(crucigrama.Grilla[f, c] + " ");
                Debug.WriteLine("");
            }
        }
    }
}