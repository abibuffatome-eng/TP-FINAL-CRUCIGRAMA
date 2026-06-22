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
            this.Size = new Size(600, 450); 
            this.FormBorderStyle = FormBorderStyle.FixedDialog; 
            this.StartPosition = FormStartPosition.CenterParent; 
            this.BackColor = Color.FromArgb(248, 246, 242); 
            this.Font = new Font("Segoe UI", 10);
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = true;

            int anchoLabel = 160;
            int xLabel = (this.ClientSize.Width / 2) - (anchoLabel / 2);
            int xControl = 230;
            int anchoTextBox = 240;
            int alto = 24;
            int[] filaY = { 40, 80, 130, 180, 230 };

            label1 = new Label { Left = xLabel - 20, Top = filaY[0], Width = anchoLabel + 40, Text = "SELECCIONE UN NIVEL", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(80, 85, 80), TextAlign = ContentAlignment.TopCenter, AutoSize = true };

            int anchoBoton = 243;
            int MitadPantalla = this.ClientSize.Width / 2;

            btFacil = new Button
            {
                Left = MitadPantalla - (anchoBoton / 2),
                Top = 100,
                Width = anchoBoton,
                Height = 46,
                Text = "FÁCIL",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137), // Verde Sage
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btFacil.FlatAppearance.BorderSize = 0;

            btMedio = new Button
            {
                Left = MitadPantalla - (anchoBoton / 2),
                Top = 170,
                Width = anchoBoton,
                Height = 46,
                Text = "MEDIO",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137), // Verde Sage
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btMedio.FlatAppearance.BorderSize = 0;

            btDificil = new Button
            {
                Left = MitadPantalla - (anchoBoton / 2),
                Top = 240,
                Width = anchoBoton,
                Height = 46,
                Text = "DIFÍCIL",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137), // Verde Sage
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btDificil.FlatAppearance.BorderSize = 0;

            btRanking = new Button
            {
                Left = MitadPantalla - (anchoBoton / 2),
                Top = 310,
                Width = anchoBoton,
                Height = 40,
                Text = "RANKING",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(248, 246, 242),
                ForeColor = Color.FromArgb(100, 90, 85),
                FlatStyle = FlatStyle.Flat
            };
            btRanking.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 188);
            btRanking.FlatAppearance.BorderSize = 1;

            btFacil.Cursor = Cursors.Hand;
            btMedio.Cursor = Cursors.Hand;
            btDificil.Cursor = Cursors.Hand;
            btRanking.Cursor = Cursors.Hand;

            btFacil.Click += btFacil_Click;
            btMedio.Click += btMedio_Click;
            btDificil.Click += btDificil_Click;
            btRanking.Click += btRanking_Click;

            this.Controls.AddRange(new Control[] { label1, btFacil, btMedio, btDificil, btRanking });

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
            f.Text = " Seleccionar Crucigrama";
            f.Size = new Size(500, 430);
            f.StartPosition = FormStartPosition.CenterParent;
            f.BackColor = Color.FromArgb(248, 246, 242); // Estilo Clean / Crema
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox = false;
            f.MinimizeBox = false;

            var lb = new ListBox
            {
                Left = 20,
                Top = 20,
                Width = 444,
                Height = 280,
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            foreach (var c in crucs) lb.Items.Add(c);

            var btnPlay = new Button
            {
                Text = "JUGAR",
                Left = 20,
                Top = 325,
                Width = 140,
                Height = 40,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137), // Verde Sage
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnPlay.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                Text = "CANCELAR",
                Left = 175,
                Top = 325,
                Width = 140,
                Height = 40,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(248, 246, 242),
                ForeColor = Color.FromArgb(100, 90, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 188);
            btnCancel.FlatAppearance.BorderSize = 1;

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