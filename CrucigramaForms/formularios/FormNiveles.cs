using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrucigramaForms.Persistencia;
using CrucigramaForms.Modelos;
using System.Globalization;
using System.Text;

namespace CrucigramaForms.formularios
{
    public partial class FormNiveles : Form
    {
        private Label label1;
        private Button btFacil, btMedio, btDificil, btRanking;
        Usuario usuario_;
        public FormNiveles()
        {
            
            InitializeComponent();
            CrearFormularioNiveles();
        }
        internal FormNiveles(Usuario usuario_)
        {
            this.usuario_ = usuario_;
            InitializeComponent();
            CrearFormularioNiveles();
        }
        private static string NormalizeString(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
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

            label1 = new Label { Left = xLabel, Top = 30, Width = anchoLabel, Text = "Seleccione un Nivel: ", AutoSize = true };

            int anchoBoton = 243;
            int MitadPantalla = this.ClientSize.Width / 2;

            btFacil = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 78, Width = anchoBoton, Height = 48, Text = "Facil - (100)" };
            btMedio = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 148, Width = anchoBoton, Height = 48, Text = "Medio - (250)" };
            btDificil = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 218, Width = anchoBoton, Height = 48, Text = "Dificil - (500)" };
            btRanking = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 288, Width = anchoBoton, Height = 48, Text = "Ranking" };

            btRanking.BackColor = Color.Cyan;

            btFacil.Cursor = Cursors.Hand;
            btMedio.Cursor = Cursors.Hand;
            btDificil.Cursor = Cursors.Hand;
            btRanking.Cursor = Cursors.Hand;

            btFacil.Click += btFacil_Click;
            btMedio.Click += btMedio_Click;
            btDificil.Click += btDificil_Click;
            btRanking.Click += btRanking_Click;

            this.Controls.AddRange(new Control[] {label1, btFacil, btMedio, btDificil, btRanking });

        }
        private void btFacil_Click(object sender, EventArgs e)
        {
            ShowSeleccionCrucigramaForLevel("Facil");
        }
        private void btMedio_Click(object sender, EventArgs e)
        {
            ShowSeleccionCrucigramaForLevel("Medio");
        }
        private void btDificil_Click(object sender, EventArgs e)
        {
            ShowSeleccionCrucigramaForLevel("Dificil");
        }
        private void btRanking_Click(object sender, EventArgs e)
        {
            var ranking = new FormRanking();
            this.Hide();
            ranking.ShowDialog();
            this.Show();
        }

        private void ShowSeleccionCrucigramaForLevel(string nivelNombre)
        {
            var nivelRepo = new pNivel();
            var crucRepo = new pCrucigrama();
            // try to match nivel by name case-insensitive and diacritics-insensitive
            Nivel nivel = null;
            foreach (var n in nivelRepo.ObtenerTodos())
            {
                if (NormalizeString(n.Nombre).Equals(NormalizeString(nivelNombre), StringComparison.OrdinalIgnoreCase)) { nivel = n; break; }
            }
            if (nivel == null)
            {
                MessageBox.Show($"No se encontró el nivel '{nivelNombre}'.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var crucs = crucRepo.ObtenerPorNivel(nivel.Id);
            if (crucs == null || crucs.Count == 0)
            {
                MessageBox.Show($"No hay crucigramas disponibles para el nivel {nivelNombre}.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var f = new Form();
            f.Text = "Seleccionar Crucigrama";
            f.Size = new Size(500, 400);
            f.StartPosition = FormStartPosition.CenterParent;

            var lb = new ListBox { Left = 10, Top = 10, Width = 460, Height = 300 };
            foreach (var c in crucs) lb.Items.Add(c);

            var btnPlay = new Button { Text = "Jugar", Left = 10, Top = 320, Width = 120, Height = 36, Cursor = Cursors.Hand };
            var btnCancel = new Button { Text = "Cancelar", Left = 150, Top = 320, Width = 120, Height = 36, Cursor = Cursors.Hand };

            btnPlay.Click += (s, e) => { if (lb.SelectedItem != null) f.DialogResult = DialogResult.OK; else MessageBox.Show("Seleccioná un crucigrama.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); };
            btnCancel.Click += (s, e) => f.DialogResult = DialogResult.Cancel;

            f.Controls.AddRange(new Control[] { lb, btnPlay, btnCancel });

            if (f.ShowDialog(this) != DialogResult.OK) return;

            var selected = lb.SelectedItem as Crucigrama;
            if (selected == null) return;

            // ensure grilla built
            selected.ConstruirGrilla();

            using var juego = new FormCrucigrama(selected, usuario_);
            this.Hide();
            juego.ShowDialog();
            
            this.Show();
        }
    }
}
