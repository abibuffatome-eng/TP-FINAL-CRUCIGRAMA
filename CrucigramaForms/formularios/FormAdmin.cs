using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CrucigramaForms.Modelos;
using CrucigramaForms.Persistencia;

namespace CrucigramaForms.formularios
{
    public partial class FormAdmin : Form
    {
        private readonly pCrucigrama _crucRepo = new pCrucigrama();
        private readonly pPalabra _palRepo = new pPalabra();
        private readonly pNivel _nivelRepo = new pNivel();

        private ListBox lbCrucigramas;
        private Button btnAddCruc, btnEditCruc, btnDeleteCruc;
        private TextBox txtTitulo;
        private ComboBox cbNiveles;

        private ListBox lbPalabras;
        private Button btnAddPal, btnEditPal, btnDeletePal;

        private List<Crucigrama> crucigramas = new List<Crucigrama>();

        public FormAdmin()
        {
            InitializeComponent();
            BuildUi();
            LoadNiveles();
            LoadCrucigramas();
        }

        private void BuildUi()
        {
            this.Text = "Administración - Crucigramas";
            this.Size = new Size(900, 560);

            // Left: crucigramas list and controls
            lbCrucigramas = new ListBox { Left = 10, Top = 10, Width = 400, Height = 420 };
            lbCrucigramas.SelectedIndexChanged += LbCrucigramas_SelectedIndexChanged;

            txtTitulo = new TextBox { Left = 10, Top = 440, Width = 260 };
            cbNiveles = new ComboBox { Left = 280, Top = 440, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };

            btnAddCruc = new Button { Left = 420, Top = 10, Width = 140, Height = 36, Text = "Agregar Crucigrama" };
            btnEditCruc = new Button { Left = 420, Top = 56, Width = 140, Height = 36, Text = "Editar Título" };
            btnDeleteCruc = new Button { Left = 420, Top = 102, Width = 140, Height = 36, Text = "Eliminar" };

            btnAddCruc.Click += BtnAddCruc_Click;
            btnEditCruc.Click += BtnEditCruc_Click;
            btnDeleteCruc.Click += BtnDeleteCruc_Click;

            // Right: palabras list and controls
            lbPalabras = new ListBox { Left = 580, Top = 10, Width = 300, Height = 420 };

            btnAddPal = new Button { Left = 420, Top = 200, Width = 140, Height = 36, Text = "Agregar Palabra" };
            btnEditPal = new Button { Left = 420, Top = 246, Width = 140, Height = 36, Text = "Editar Palabra" };
            btnDeletePal = new Button { Left = 420, Top = 292, Width = 140, Height = 36, Text = "Eliminar Palabra" };

            btnAddPal.Click += BtnAddPal_Click;
            btnEditPal.Click += BtnEditPal_Click;
            btnDeletePal.Click += BtnDeletePal_Click;

            // Labels
            var lblTitulo = new Label { Left = 10, Top = 420, Width = 200, Text = "Título:" };
            var lblNivel = new Label { Left = 280, Top = 420, Width = 200, Text = "Nivel:" };
            var lblCrucList = new Label { Left = 10, Top = 0, Width = 200, Text = "Crucigramas:" };
            var lblPalList = new Label { Left = 580, Top = 0, Width = 200, Text = "Palabras (seleccionar crucigrama):" };

            this.Controls.AddRange(new Control[] {
                lbCrucigramas, lblCrucList, txtTitulo, lblTitulo, cbNiveles, lblNivel,
                btnAddCruc, btnEditCruc, btnDeleteCruc,
                lbPalabras, lblPalList, btnAddPal, btnEditPal, btnDeletePal
            });
        }

        private void LoadNiveles()
        {
            var niveles = _nivelRepo.ObtenerTodos();
            cbNiveles.Items.Clear();
            foreach (var n in niveles)
                cbNiveles.Items.Add(n);
            if (cbNiveles.Items.Count > 0)
                cbNiveles.SelectedIndex = 0;
        }

        private void LoadCrucigramas()
        {
            crucigramas = _crucRepo.ObtenerTodos();
            lbCrucigramas.Items.Clear();
            foreach (var c in crucigramas)
                lbCrucigramas.Items.Add(c);

            lbPalabras.Items.Clear();
        }

        private void LbCrucigramas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = lbCrucigramas.SelectedItem as Crucigrama;
            if (selected == null) return;

            txtTitulo.Text = selected.Titulo;
            // select nivel in combobox
            for (int i = 0; i < cbNiveles.Items.Count; i++)
            {
                if (((Nivel)cbNiveles.Items[i]).Id == selected.Nivel.Id)
                {
                    cbNiveles.SelectedIndex = i;
                    break;
                }
            }

            LoadPalabras(selected.Id);
        }

        private void LoadPalabras(int crucId)
        {
            lbPalabras.Items.Clear();
            var palabras = _palRepo.ObtenerPorCrucigrama(crucId);
            foreach (var p in palabras)
                lbPalabras.Items.Add(p);
        }

        private void BtnAddCruc_Click(object sender, EventArgs e)
        {
            string titulo = txtTitulo.Text.Trim();
            if (string.IsNullOrEmpty(titulo))
            {
                MessageBox.Show("Ingresa un título para el crucigrama.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbNiveles.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un nivel.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nivel = (Nivel)cbNiveles.SelectedItem;
            var cruc = new Crucigrama { Titulo = titulo, Nivel = nivel };
            _crucRepo.Agregar(cruc);

            LoadCrucigramas();
            // select new one
            var added = crucigramas.FirstOrDefault(c => c.Id == cruc.Id);
            if (added != null) lbCrucigramas.SelectedItem = added;
        }

        private void BtnEditCruc_Click(object sender, EventArgs e)
        {
            var selected = lbCrucigramas.SelectedItem as Crucigrama;
            if (selected == null)
            {
                MessageBox.Show("Selecciona un crucigrama para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoTitulo = txtTitulo.Text.Trim();
            if (string.IsNullOrEmpty(nuevoTitulo))
            {
                MessageBox.Show("Ingresa un título.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _crucRepo.ActualizarTitulo(selected.Id, nuevoTitulo);
            LoadCrucigramas();
        }

        private void BtnDeleteCruc_Click(object sender, EventArgs e)
        {
            var selected = lbCrucigramas.SelectedItem as Crucigrama;
            if (selected == null)
            {
                MessageBox.Show("Selecciona un crucigrama para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var r = MessageBox.Show($"Eliminar '{selected.Titulo}'? Esto quitará también sus palabras.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            _crucRepo.Eliminar(selected.Id);
            LoadCrucigramas();
        }

        private void BtnAddPal_Click(object sender, EventArgs e)
        {
            var selected = lbCrucigramas.SelectedItem as Crucigrama;
            if (selected == null)
            {
                MessageBox.Show("Selecciona un crucigrama primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pal = ShowPalabraDialog(null);
            if (pal == null) return;

            pal.CrucigramaId = selected.Id;
            _palRepo.Agregar(pal);
            LoadPalabras(selected.Id);
        }

        private void BtnEditPal_Click(object sender, EventArgs e)
        {
            var selectedCruc = lbCrucigramas.SelectedItem as Crucigrama;
            var selectedPal = lbPalabras.SelectedItem as Palabra;
            if (selectedCruc == null || selectedPal == null)
            {
                MessageBox.Show("Selecciona una palabra para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var edited = ShowPalabraDialog(selectedPal);
            if (edited == null) return;

            edited.Id = selectedPal.Id;
            edited.CrucigramaId = selectedCruc.Id;
            _palRepo.Actualizar(edited);
            LoadPalabras(selectedCruc.Id);
        }

        private void BtnDeletePal_Click(object sender, EventArgs e)
        {
            var selectedCruc = lbCrucigramas.SelectedItem as Crucigrama;
            var selectedPal = lbPalabras.SelectedItem as Palabra;
            if (selectedCruc == null || selectedPal == null)
            {
                MessageBox.Show("Selecciona una palabra para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var r = MessageBox.Show($"Eliminar palabra '{selectedPal.Texto}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            _palRepo.Eliminar(selectedPal.Id);
            LoadPalabras(selectedCruc.Id);
        }

        // Small dialog to create/edit a Palabra
        private Palabra ShowPalabraDialog(Palabra fuente)
        {
            using var f = new Form();
            f.Text = fuente == null ? "Agregar Palabra" : "Editar Palabra";
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.ClientSize = new Size(420, 260);
            f.StartPosition = FormStartPosition.CenterParent;

            var lblTexto = new Label { Left = 10, Top = 10, Width = 100, Text = "Texto:" };
            var txtTexto = new TextBox { Left = 120, Top = 10, Width = 280 };

            var lblPista = new Label { Left = 10, Top = 50, Width = 100, Text = "Pista:" };
            var txtPista = new TextBox { Left = 120, Top = 50, Width = 280 };

            var lblFila = new Label { Left = 10, Top = 90, Width = 100, Text = "Fila (0-based):" };
            var txtFila = new TextBox { Left = 120, Top = 90, Width = 80 };

            var lblCol = new Label { Left = 220, Top = 90, Width = 80, Text = "Columna:" };
            var txtCol = new TextBox { Left = 300, Top = 90, Width = 100 };

            var lblOri = new Label { Left = 10, Top = 130, Width = 100, Text = "Orientación:" };
            var cbOri = new ComboBox { Left = 120, Top = 130, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cbOri.Items.AddRange(new string[] { "horizontal", "vertical" });

            var btnOk = new Button { Text = "Aceptar", Left = 120, Width = 120, Top = 180, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancelar", Left = 260, Width = 120, Top = 180, DialogResult = DialogResult.Cancel };

            f.Controls.AddRange(new Control[] { lblTexto, txtTexto, lblPista, txtPista, lblFila, txtFila, lblCol, txtCol, lblOri, cbOri, btnOk, btnCancel });

            if (fuente != null)
            {
                txtTexto.Text = fuente.Texto;
                txtPista.Text = fuente.Pista;
                txtFila.Text = fuente.Fila.ToString();
                txtCol.Text = fuente.Columna.ToString();
                cbOri.SelectedItem = fuente.Orientacion;
            }
            else
            {
                cbOri.SelectedIndex = 0;
            }

            if (f.ShowDialog(this) != DialogResult.OK) return null;

            // validations
            var texto = txtTexto.Text.Trim();
            if (string.IsNullOrEmpty(texto))
            {
                MessageBox.Show("Ingresa el texto de la palabra.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (!int.TryParse(txtFila.Text.Trim(), out int fila) || fila < 0)
            {
                MessageBox.Show("Fila inválida.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            if (!int.TryParse(txtCol.Text.Trim(), out int col) || col < 0)
            {
                MessageBox.Show("Columna inválida.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var orientacion = cbOri.SelectedItem as string ?? "horizontal";

            var palabra = new Palabra
            {
                Texto = texto.ToUpper(),
                Pista = txtPista.Text.Trim(),
                Fila = fila,
                Columna = col,
                Orientacion = orientacion
            };

            return palabra;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
