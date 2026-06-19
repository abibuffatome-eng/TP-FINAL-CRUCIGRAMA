using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CrucigramaForms.Modelos;

namespace CrucigramaForms.formularios
{
    internal class FormPreviewCrucigrama : Form
    {
        private Crucigrama _cruc;
        private Panel panelGrid;
        private Button btnClose;

        public FormPreviewCrucigrama(Crucigrama crucigrama)
        {
            _cruc = crucigrama ?? throw new ArgumentNullException(nameof(crucigrama));
            InitializeComponent();
            BuildUi();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        private void BuildUi()
        {
            this.Text = "Vista previa - " + _cruc.Titulo;
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            panelGrid = new Panel { Left = 10, Top = 10, AutoSize = false, BackColor = Color.FromArgb(235, 240, 245), AutoScroll = true };

            btnClose = new Button { Text = "Cerrar", Width = 120, Height = 40, Cursor = Cursors.Hand };
            btnClose.Click += (s, e) => this.Close();

            // list of palabras/pistas at the right
            var lbPistas = new ListBox { Left = 10, Top = 10, Width = 260, Height = 560 }; // placeholder, will be repositioned

            this.Controls.AddRange(new Control[] { panelGrid, lbPistas, btnClose });

            RenderGrid(lbPistas);
        }

        private void RenderGrid(ListBox lbPistas)
        {
            _cruc.ConstruirGrilla();

            int filas = _cruc.Nivel.Filas;
            int columnas = _cruc.Nivel.Columnas;
            int tam = 35;

            panelGrid.Controls.Clear();
            int gridWidth = columnas * tam + 2;
            int gridHeight = filas * tam + 2;
            panelGrid.Size = new Size(gridWidth, gridHeight);

            // position listbox to the right of grid
            lbPistas.Left = panelGrid.Right + 10;
            lbPistas.Top = panelGrid.Top;
            lbPistas.Width = 260;
            lbPistas.Height = Math.Min(gridHeight, this.ClientSize.Height - 80);

            // position close button under listbox
            btnClose.Left = lbPistas.Left;
            btnClose.Top = lbPistas.Top + lbPistas.Height + 10;

            lbPistas.Items.Clear();

            // detect conflicts similar to admin validator
            var conflicts = new HashSet<(int, int)>();
            var map = new Dictionary<(int, int), char>();
            foreach (var p in _cruc.Palabras)
            {
                for (int i = 0; i < p.Texto.Length; i++)
                {
                    int fila = p.EsHorizontal() ? p.Fila : p.Fila + i;
                    int col = p.EsHorizontal() ? p.Columna + i : p.Columna;
                    if (fila < 0 || col < 0 || fila >= filas || col >= columnas) continue;
                    var ch = char.ToUpper(p.Texto[i]);
                    var key = (fila, col);
                    if (map.TryGetValue(key, out var existing))
                    {
                        if (existing != ch) conflicts.Add(key);
                    }
                    else map[key] = ch;
                }
            }

            for (int r = 0; r < filas; r++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    var cell = _cruc.Grilla[r, c];
                    var tb = new TextBox
                    {
                        Size = new Size(tam, tam),
                        Location = new Point(c * tam, r * tam),
                        TextAlign = HorizontalAlignment.Center,
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ReadOnly = true,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = cell.EstaBloqueada ? Color.Black : Color.White,
                        ForeColor = cell.EstaBloqueada ? Color.Black : Color.Black
                    };

                    if (!cell.EstaBloqueada && cell.LetraCorrecta != '\0')
                        tb.Text = cell.LetraCorrecta.ToString();

                    if (conflicts.Contains((r, c)))
                    {
                        tb.BackColor = Color.LightCoral;
                    }

                    panelGrid.Controls.Add(tb);
                }
            }

            // fill pistas list
            int idx = 1;
            foreach (var p in _cruc.Palabras)
            {
                lbPistas.Items.Add($"{idx}. {p.Texto} — {p.Pista} ({p.Fila},{p.Columna}) [{p.Orientacion}]");
                idx++;
            }
        }
    }
}
