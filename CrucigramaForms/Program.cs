using CrucigramaForms.Controladores;
using CrucigramaForms.Modelos;
using System.Diagnostics;

namespace CrucigramaForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Conexion.InicializarBaseDeDatos(); 
            Application.Run(new Form1());

          
        }
    }
}