using CrucigramaForms.Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CrucigramaForms.formularios
{
    public partial class FormRanking : Form
    {
        private DataGridView dgvRanking;
        private pPartida _repo = new pPartida();

        public FormRanking()
        {
            CrearFormulario();
            LlenarRanking();
        }

        private void CrearFormulario()
        {
            this.Text = "Ranking General";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(235, 240, 245);

            dgvRanking = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            this.Controls.Add(dgvRanking);
        }

        private void LlenarRanking()
        {
            // Asignamos la fuente de datos
            var datos = _repo.ObtenerRanking();

            dgvRanking.DataSource = datos;

            // Configuramos las columnas una vez cargados los datos
            if (dgvRanking.Columns["NombreUsuario"] != null)
                dgvRanking.Columns["NombreUsuario"].HeaderText = "Jugador";

            if (dgvRanking.Columns["PuntajeTotal"] != null)
                dgvRanking.Columns["PuntajeTotal"].HeaderText = "Puntaje Total";
        }
    }
}