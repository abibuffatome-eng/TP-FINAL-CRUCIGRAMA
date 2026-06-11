using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CrucigramaForms.formularios
{
    public partial class FormNiveles : Form
    {
        private Label label1;
        private Button btFacil, btMedio, btDificil;
        public FormNiveles()
        {
            InitializeComponent();
            CrearFormularioNiveles();
        }
        private void CrearFormularioNiveles()
        {
            this.Size = new Size(600, 400); //tamaño del formulario
            this.FormBorderStyle = FormBorderStyle.FixedDialog; //evita q usuario redimensione el formulario
            this.StartPosition = FormStartPosition.CenterParent; // centra el formulario
            this.BackColor = Color.FromArgb(235, 240, 245); //color de fondo
            this.Font = new Font("Segoe UI", 10); // fuente del formulario
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = true;

            int anchoLabel = 160;
            int xLabel = (this.ClientSize.Width/2)-(anchoLabel/2);
            int xControl = 230;
            int anchoTextBox = 240;
            int alto = 24;
            int[] filaY = { 30, 80, 130, 180, 230 };

            label1 = new Label { Left = xLabel, Top = filaY[0], Width = anchoLabel, Text = "Seleccione un Nivel: ", AutoSize = true };

            int anchoBoton = 243;
            int MitadPantalla = this.ClientSize.Width / 2;

            btFacil = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 78, Width = anchoBoton, Height = 48, Text = "Facil" };
            btMedio = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 148, Width = anchoBoton, Height = 48, Text = "Medio" };
            btDificil = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 218, Width = anchoBoton, Height = 48, Text = "Dificil" };

            btFacil.Cursor = Cursors.Hand;
            btMedio.Cursor = Cursors.Hand;
            btDificil.Cursor = Cursors.Hand;

            btFacil.Click += btFacil_Click;
            btMedio.Click += btMedio_Click;
            btDificil.Click += btDificil_Click;

            this.Controls.AddRange(new Control[] {label1, btFacil, btMedio, btDificil });

        }
        private void btFacil_Click(object sender, EventArgs e)
        {
            FormCrucigrama formCrucigramaFacil = new FormCrucigrama();
            this.Hide();
            formCrucigramaFacil.ShowDialog();
            this.Show();
        }
        private void btMedio_Click(object sender, EventArgs e)
        {
            FormCrucigrama formCrucigramaMedio = new FormCrucigrama();
            this.Hide();
            formCrucigramaMedio.ShowDialog();
            this.Show();
        }
        private void btDificil_Click(object sender, EventArgs e)
        {
            FormCrucigrama formCrucigramaDificil = new FormCrucigrama();
            this.Hide();
            formCrucigramaDificil.ShowDialog();
            this.Show();
        }
    }
}
