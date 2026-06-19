using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
        private Button btnConfirmCruc;
        private Button btnPreviewCruc;
        private Button btnRefresh;
        private TextBox txtTitulo;
        private ComboBox cbNiveles;

        private ListBox lbPalabras;
        private Button btnAddPal, btnEditPal, btnDeletePal;
        private Button btnCerrarSesion;

        private List<Crucigrama> crucigramas = new List<Crucigrama>();

        public FormAdmin()
        {
            InitializeComponent();
            BuildUi();
            LoadNiveles();
            LoadCrucigramas();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // reload levels and crucigramas; if one is selected reload its palabras
            LoadNiveles();
            LoadCrucigramas();

            var selected = lbCrucigramas.SelectedItem as Crucigrama;
            if (selected != null)
                LoadPalabras(selected.Id);
        }

        private void BtnPreviewCruc_Click(object sender, EventArgs e)
        {
            var selected = lbCrucigramas.SelectedItem as Crucigrama;
            if (selected == null)
            {
                MessageBox.Show("Selecciona un crucigrama para la vista previa.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var f = new FormPreviewCrucigrama(selected);
            f.ShowDialog(this);
        }

        private void BuildUi()
        {
            this.Text = "Administración - Crucigramas";
            this.Size = new Size(900, 520);

            // paneles para organizar la UI: izquierdo para crucigramas, derecho para palabras
            var leftX = 10;
            var leftW = 420;
            var rightX = leftX + leftW + 10; // small margin between panels
            var rightW = 420;

            // Panel izquierdo (crucigramas)
            var pnlLeft = new Panel { Left = leftX, Top = 10, Width = leftW, Height = 500, BorderStyle = BorderStyle.None };
            var lblCrucList = new Label { Left = 0, Top = 0, Width = leftW, Text = "Crucigramas:" };
            lbCrucigramas = new ListBox { Left = 0, Top = 24, Width = leftW, Height = 320 };
            lbCrucigramas.SelectedIndexChanged += LbCrucigramas_SelectedIndexChanged;

            var lblTitulo = new Label { Left = 0, Top = 355, Width = 100, Text = "Título:" };
            txtTitulo = new TextBox { Left = 0, Top = 378, Width = 260 };
            var lblNivel = new Label { Left = 270, Top = 355, Width = 100, Text = "Nivel:" };
            cbNiveles = new ComboBox { Left = 270, Top = 378, Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };

            pnlLeft.Controls.AddRange(new Control[] { lblCrucList, lbCrucigramas, lblTitulo, txtTitulo, lblNivel, cbNiveles });

            // place crucigrama action buttons below title inside left panel
            btnAddCruc = new Button { Left = 0, Top = txtTitulo.Top + txtTitulo.Height + 8, Width = 140, Height = 36, Text = "Agregar Crucigrama" };
            btnEditCruc = new Button { Left = 150, Top = txtTitulo.Top + txtTitulo.Height + 8, Width = 140, Height = 36, Text = "Editar Título" };
            btnDeleteCruc = new Button { Left = 300, Top = txtTitulo.Top + txtTitulo.Height + 8, Width = 100, Height = 36, Text = "Eliminar" };
            btnAddCruc.Click += BtnAddCruc_Click;
            btnEditCruc.Click += BtnEditCruc_Click;
            btnDeleteCruc.Click += BtnDeleteCruc_Click;
            pnlLeft.Controls.AddRange(new Control[] { btnAddCruc, btnEditCruc, btnDeleteCruc });

            // Panel derecho (palabras)
            var pnlRight = new Panel { Left = rightX, Top = 10, Width = rightW, Height = 500, BorderStyle = BorderStyle.None };
            var lblPalList = new Label { Left = 0, Top = 0, Width = rightW, Text = "Palabras (seleccionar crucigrama):" };
            lbPalabras = new ListBox { Left = 0, Top = 24, Width = rightW - 110, Height = 360 };

            // preview and refresh buttons on the right side of palabras panel
            btnPreviewCruc = new Button { Left = lbPalabras.Right + 10, Top = 24, Width = 90, Height = 36, Text = "Vista Previa" };
            btnRefresh = new Button { Left = lbPalabras.Right + 10, Top = 70, Width = 90, Height = 36, Text = "Refrescar" };
            btnPreviewCruc.Click += BtnPreviewCruc_Click;
            btnRefresh.Click += BtnRefresh_Click;

            pnlRight.Controls.AddRange(new Control[] { lblPalList, lbPalabras, btnPreviewCruc, btnRefresh });

            // place Confirmar under preview/refresh in right panel
            btnConfirmCruc = new Button { Left = btnPreviewCruc.Left, Top = btnRefresh.Top + btnRefresh.Height + 8, Width = 90, Height = 36, Text = "Confirmar" };
            btnConfirmCruc.Click += BtnConfirmCruc_Click;
            pnlRight.Controls.Add(btnConfirmCruc);

            // place palabra action buttons under the palabras list inside right panel
            btnAddPal = new Button { Left = 0, Top = lbPalabras.Bottom + 10, Width = 120, Height = 36, Text = "Agregar Palabra" };
            btnEditPal = new Button { Left = 130, Top = lbPalabras.Bottom + 10, Width = 120, Height = 36, Text = "Editar Palabra" };
            btnDeletePal = new Button { Left = 260, Top = lbPalabras.Bottom + 10, Width = 120, Height = 36, Text = "Eliminar Palabra" };
            btnAddPal.Click += BtnAddPal_Click;
            btnEditPal.Click += BtnEditPal_Click;
            btnDeletePal.Click += BtnDeletePal_Click;
            pnlRight.Controls.AddRange(new Control[] { btnAddPal, btnEditPal, btnDeletePal });

            // Center controls (actions)
            var centerX = leftX + leftW + 10;
            var btnLeft = centerX;
            btnAddCruc = new Button { Left = btnLeft, Top = 30, Width = 140, Height = 36, Text = "Agregar Crucigrama" };
            btnEditCruc = new Button { Left = btnLeft, Top = 76, Width = 140, Height = 36, Text = "Editar Título" };
            btnDeleteCruc = new Button { Left = btnLeft, Top = 122, Width = 140, Height = 36, Text = "Eliminar" };
            btnConfirmCruc = new Button { Left = btnLeft, Top = 168, Width = 140, Height = 36, Text = "Confirmar" };
            btnPreviewCruc = new Button { Left = btnLeft, Top = 214, Width = 140, Height = 36, Text = "Vista Previa" };
            btnRefresh = new Button { Left = btnLeft, Top = 260, Width = 140, Height = 36, Text = "Refrescar" };

            btnAddPal = new Button { Left = btnLeft, Top = 320, Width = 140, Height = 36, Text = "Agregar Palabra" };
            btnEditPal = new Button { Left = btnLeft, Top = 366, Width = 140, Height = 36, Text = "Editar Palabra" };
            btnDeletePal = new Button { Left = btnLeft, Top = 412, Width = 140, Height = 36, Text = "Eliminar Palabra" };
            btnCerrarSesion = new Button { Left = btnLeft, Top = 460, Width = 140, Height = 36, Text = "Cerrar Sesión" };

            btnAddCruc.Click += BtnAddCruc_Click;
            btnEditCruc.Click += BtnEditCruc_Click;
            btnDeleteCruc.Click += BtnDeleteCruc_Click;
            btnConfirmCruc.Click += BtnConfirmCruc_Click;
            btnPreviewCruc.Click += BtnPreviewCruc_Click;
            btnRefresh.Click += BtnRefresh_Click;

            btnAddPal.Click += BtnAddPal_Click;
            btnEditPal.Click += BtnEditPal_Click;
            btnDeletePal.Click += BtnDeletePal_Click;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            // agregar paneles y botón central al formulario
            this.Controls.AddRange(new Control[] { pnlLeft, pnlRight, btnCerrarSesion });
        }

        private void BtnConfirmCruc_Click(object sender, EventArgs e)
        {
            var selected = lbCrucigramas.SelectedItem as Crucigrama;
            if (selected == null)
            {
                MessageBox.Show("Selecciona un crucigrama para confirmar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // rebuild grid (in case) and validate for conflicting overlaps
            selected.ConstruirGrilla();
            var conflicts = ValidateCrucigrama(selected);

            if (conflicts.Count == 0)
            {
                MessageBox.Show("Crucigrama confirmado sin conflictos.", "Confirmado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var msg = new StringBuilder();
                msg.AppendLine("Se encontraron conflictos entre palabras:");
                foreach (var s in conflicts)
                    msg.AppendLine(" - " + s);

                msg.AppendLine();
                msg.AppendLine("Seleccioná las palabras en la lista y usá 'Editar Palabra' para corregirlas.");

                MessageBox.Show(msg.ToString(), "Conflictos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            // hide admin and show login form
            this.Hide();
            using var login = new FormLogin();
            login.ShowDialog();
            this.Close();
        }

        private void BtnSalirApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // returns list of conflict descriptions
        private List<string> ValidateCrucigrama(Crucigrama cruc)
        {
            var map = new Dictionary<(int, int), (char letra, Palabra palabra)>();
            var conflicts = new List<string>();

            foreach (var p in cruc.Palabras)
            {
                for (int i = 0; i < p.Texto.Length; i++)
                {
                    int fila = p.EsHorizontal() ? p.Fila : p.Fila + i;
                    int col = p.EsHorizontal() ? p.Columna + i : p.Columna;

                    if (fila < 0 || col < 0 || fila >= cruc.Nivel.Filas || col >= cruc.Nivel.Columnas)
                    {
                        conflicts.Add($"Palabra '{p.Texto}' sale fuera de la grilla en posición ({fila},{col}).");
                        continue;
                    }

                    var key = (fila, col);
                    var letra = char.ToUpper(p.Texto[i]);
                    if (map.TryGetValue(key, out var existing))
                    {
                        if (existing.letra != letra)
                        {
                            var desc = $"'{p.Texto}' (pos {fila},{col}) choca con '{existing.palabra.Texto}' (pos {fila},{col}): '{letra}' != '{existing.letra}'";
                            // avoid duplicates (order-insensitive)
                            if (!conflicts.Contains(desc))
                                conflicts.Add(desc);
                        }
                    }
                    else
                    {
                        map[key] = (letra, p);
                    }
                }
            }

            return conflicts;
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
            // seleccionar el nivel en el combobox
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
            // seleccionar el nuevo crucigrama en la lista
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

            var pal = ShowPalabraInteractive(selected);
            if (pal == null) return;

            pal.CrucigramaId = selected.Id;
            _palRepo.Agregar(pal);
            LoadPalabras(selected.Id);
        }

        // Interactive dialog: click a grid cell to choose start position, enter text/pista and orientation
        private Palabra ShowPalabraInteractive(Crucigrama cruc)
        {
            cruc.ConstruirGrilla();

            using var f = new Form();
            f.Text = "Agregar Palabra (Interactivo)";
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.ClientSize = new Size(800, 520);
            f.StartPosition = FormStartPosition.CenterParent;

            int filas = cruc.Nivel.Filas;
            int columnas = cruc.Nivel.Columnas;
            int tam = 30;

            var panel = new Panel { Left = 10, Top = 10, Width = Math.Min(600, columnas * tam + 2), Height = Math.Min(440, filas * tam + 2), AutoScroll = true };

            Button selectedBtn = null;
            int selRow = -1, selCol = -1;
            TextBox txtFila = new TextBox();
            TextBox txtCol = new TextBox();

            // render grid as buttons
            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    var cell = cruc.Grilla[r, c];
                    var btn = new Button
                    {
                        Size = new Size(tam, tam),
                        Location = new Point(c * tam, r * tam),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = cell.EstaBloqueada ? Color.Black : Color.White,
                        ForeColor = Color.Black,
                        Tag = new Point(r, c)
                    };

                    if (!cell.EstaBloqueada && cell.LetraCorrecta != '\0')
                        btn.Text = cell.LetraCorrecta.ToString();

                    btn.Click += (s, e) =>
                    {
                        if (selectedBtn != null) selectedBtn.FlatAppearance.BorderSize = 0;
                        selectedBtn = (Button)s;
                        selectedBtn.FlatAppearance.BorderSize = 2;
                        selectedBtn.FlatAppearance.BorderColor = Color.Blue;
                        var p = (Point)selectedBtn.Tag;
                        selRow = p.X; selCol = p.Y;
                        txtFila.Text = selRow.ToString();
                        txtCol.Text = selCol.ToString();
                    };

                    panel.Controls.Add(btn);
                }
            }

            // controls on right
            var lblInfo = new Label { Left = panel.Right + 20, Top = 10, Width = 220, Text = "Seleccione la celda inicial en la grilla" };
            var lblTexto = new Label { Left = panel.Right + 20, Top = 40, Width = 80, Text = "Texto:" };
            var txtTexto = new TextBox { Left = panel.Right + 100, Top = 38, Width = 220 };

            var lblPista = new Label { Left = panel.Right + 20, Top = 78, Width = 80, Text = "Pista:" };
            var txtPista = new TextBox { Left = panel.Right + 100, Top = 76, Width = 220 };

            var lblFila = new Label { Left = panel.Right + 20, Top = 118, Width = 80, Text = "Fila:" };
            txtFila = new TextBox { Left = panel.Right + 100, Top = 116, Width = 80, ReadOnly = true };

            var lblCol = new Label { Left = panel.Right + 20, Top = 156, Width = 80, Text = "Columna:" };
            txtCol = new TextBox { Left = panel.Right + 100, Top = 154, Width = 80, ReadOnly = true };

            var lblOri = new Label { Left = panel.Right + 20, Top = 196, Width = 80, Text = "Orientación:" };
            var cbOri = new ComboBox { Left = panel.Right + 100, Top = 194, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cbOri.Items.AddRange(new string[] { "horizontal", "vertical" });
            cbOri.SelectedIndex = 0;

            var btnOk = new Button { Text = "Agregar", Left = panel.Right + 40, Top = 240, Width = 120, Height = 36, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancelar", Left = panel.Right + 180, Top = 240, Width = 120, Height = 36, DialogResult = DialogResult.Cancel };

            f.Controls.AddRange(new Control[] { panel, lblInfo, lblTexto, txtTexto, lblPista, txtPista, lblFila, txtFila, lblCol, txtCol, lblOri, cbOri, btnOk, btnCancel });

            if (f.ShowDialog(this) != DialogResult.OK) return null;

            var texto = txtTexto.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(texto))
            {
                MessageBox.Show("Ingresa el texto de la palabra.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (selRow < 0 || selCol < 0)
            {
                MessageBox.Show("Selecciona una celda inicial en la grilla.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var orientacion = cbOri.SelectedItem as string ?? "horizontal";

            // validate fits
            int len = texto.Length;
            int endRow = orientacion == "horizontal" ? selRow : selRow + len - 1;
            int endCol = orientacion == "horizontal" ? selCol + len - 1 : selCol;
            if (endRow >= cruc.Nivel.Filas || endCol >= cruc.Nivel.Columnas)
            {
                MessageBox.Show("La palabra no entra en la grilla en la posición/orientación seleccionada.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            // check conflicts: existing letters must match
            for (int i = 0; i < len; i++)
            {
                int r = orientacion == "horizontal" ? selRow : selRow + i;
                int c = orientacion == "horizontal" ? selCol + i : selCol;
                var existing = cruc.Grilla[r, c];
                if (!existing.EstaBloqueada && existing.LetraCorrecta != '\0')
                {
                    if (char.ToUpper(existing.LetraCorrecta) != texto[i])
                    {
                        var ask = MessageBox.Show($"La letra en ({r},{c}) es '{existing.LetraCorrecta}' y no coincide con '{texto[i]}'. Deseas forzar la inserción?", "Conflicto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (ask != DialogResult.Yes) return null;
                    }
                }
            }

            var palabra = new Palabra
            {
                Texto = texto,
                Pista = txtPista.Text.Trim(),
                Fila = selRow,
                Columna = selCol,
                Orientacion = orientacion
            };

            return palabra;
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
