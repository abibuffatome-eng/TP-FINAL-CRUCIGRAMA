using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable

using CrucigramaForms.Modelos;
using CrucigramaForms.Persistencia;
using CrucigramaForms.Logica;
using System.Globalization;

namespace CrucigramaForms.formularios
{
    public partial class FormCrucigrama : Form 
    {
        private Crucigrama _crucigrama;
        private Usuario _usuario;
        private TextBox[,] _celdas;
        private Button btVerificar, btSalir;
        private Panel panelGrilla;

        private ListBox lbHorizontales;
        private ListBox lbVerticales;
        private Label lblHorizontales;
        private Label lblVerticales;

        private Label lbPuntaje;
        private bool seAdvirtio = false;

        public FormCrucigrama()
        {
            InitializeComponent();
            CrearFormulario();
        }

        internal FormCrucigrama(Crucigrama crucigrama, Usuario usuario)
        {
            InitializeComponent();
            _crucigrama = crucigrama;
            _usuario = usuario;
            cJuego.IniciarPartida(_crucigrama);
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            this.Text = "  Crucigrama: " + _crucigrama.Titulo;
            this.StartPosition = FormStartPosition.CenterScreen; 
            this.BackColor = Color.FromArgb(248, 246, 242); 
            this.Font = new Font("Segoe UI", 11); 
            this.FormBorderStyle = FormBorderStyle.FixedDialog; 
            this.MaximizeBox = false;

            panelGrilla = new Panel();
            panelGrilla.Location = new Point(55, 80);
            panelGrilla.BackColor = Color.Transparent;
            ArmarGrilla();

            int margenDerechoGrilla = panelGrilla.Left + panelGrilla.Width + 40;

            lbPuntaje = new Label
            {
                Text = $"PUNTUACIÓN: {cPuntaje.CalcularPuntajeActual(_crucigrama)}",
                Location = new Point(35, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 152, 137)
            };

            lblHorizontales = new Label
            {
                Text = "PALABRAS HORIZONTALES",
                Location = new Point(margenDerechoGrilla, 25),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 90, 85)
            };

            lbHorizontales = new ListBox
            {
                Location = new Point(margenDerechoGrilla, 50),
                Size = new Size(340, 220),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                HorizontalScrollbar = true
            };

            lblVerticales = new Label
            {
                Text = "PALABRAS VERTICALES",
                Location = new Point(margenDerechoGrilla, 295),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 90, 85)
            };

            lbVerticales = new ListBox
            {
                Location = new Point(margenDerechoGrilla, 320),
                Size = new Size(340, 220),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                HorizontalScrollbar = true
            };

            // Carga las pistas en sus respectivas listas
            CargarPistasSeparadas();

            lbHorizontales.SelectedIndexChanged += lbHorizontales_SelectedIndexChanged;
            lbVerticales.SelectedIndexChanged += lbVerticales_SelectedIndexChanged;

            int botonY = Math.Max(panelGrilla.Top + panelGrilla.Height + 30, lbVerticales.Top + lbVerticales.Height + 30);

            // Botón Verificar
            btVerificar = new Button
            {
                Text = "VERIFICAR RESPUESTAS",
                Size = new Size(200, 46),
                Location = new Point(35, botonY),
                BackColor = Color.FromArgb(135, 152, 137),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            btVerificar.FlatAppearance.BorderSize = 0;

            // Botón Abandonar
            btSalir = new Button
            {
                Text = "ABANDONAR",
                Size = new Size(150, 46),
                Location = new Point(250, botonY),
                BackColor = Color.FromArgb(248, 246, 242),
                ForeColor = Color.FromArgb(140, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            btSalir.FlatAppearance.BorderColor = Color.FromArgb(220, 200, 200);
            btSalir.FlatAppearance.BorderSize = 1;

            btVerificar.Click += btVerificar_Click;
            btSalir.Click += btSalir_Click;

            this.ClientSize = new Size(lbHorizontales.Left + lbHorizontales.Width + 40, btVerificar.Top + btVerificar.Height + 40);

            this.Controls.AddRange(new Control[] {
                panelGrilla, lbPuntaje, lblHorizontales, lbHorizontales,
                lblVerticales, lbVerticales, btVerificar, btSalir
            });
        }

        private void ArmarGrilla()
        {
            int filas = _crucigrama.Nivel.Filas;
            int columnas = _crucigrama.Nivel.Columnas;
            int tamCelda = 40;                              // Tamaño de cada cuadradito

            _celdas = new TextBox[filas, columnas];

            // agrega numeros de columnas, parte superior
            for (int c = 0; c < columnas; c++)
            {
                Label lblCol = new Label
                {
                    Text = (c + 1).ToString(),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 90, 85),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(tamCelda, 20),

                    Location = new Point(panelGrilla.Left + (c * (tamCelda - 1)), panelGrilla.Top - 22)
                };
                this.Controls.Add(lblCol); 
            }

            // agrega numeros a las filas
            for (int f = 0; f < filas; f++)
            {
                Label lblFil = new Label
                {
                    Text = (f + 1).ToString(),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 90, 85),
                    TextAlign = ContentAlignment.MiddleRight,
                    Size = new Size(25, tamCelda),
                    // Se posiciona justo a la izquierda de cada fila de la grilla
                    Location = new Point(panelGrilla.Left - 30, panelGrilla.Top + (f * (tamCelda - 1)))
                };
                this.Controls.Add(lblFil); // Se agrega directo al Form, no al panel
            }

            // creacion de la grilla 
            for (int f = 0; f < filas; f++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    Celda celda = _crucigrama.Grilla[f, c];

                    TextBox tb = new TextBox
                    {
                        Size = new Size(tamCelda, tamCelda),
                        Location = new Point(c * (tamCelda - 1), f * (tamCelda - 1)),
                        TextAlign = HorizontalAlignment.Center,
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.FromArgb(50, 50, 50),
                        CharacterCasing = CharacterCasing.Upper, // Letras en mayúscula automáticamente
                        MaxLength = 1,
                        Tag = new Point(f, c),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    if (celda.EstaBloqueada)
                    {
                        tb.BackColor = Color.FromArgb(70, 65, 60); // Color oscuro para celdas inactivas
                        tb.Enabled = false;
                    }
                    else
                    {
                        tb.BackColor = Color.White;
                        tb.TextChanged += Celda_TextChanged;
                        tb.KeyPress += Celda_KeyPress;
                    }

                    _celdas[f, c] = tb;
                    panelGrilla.Controls.Add(tb);
                }
            }

            panelGrilla.Size = new Size(columnas * (tamCelda - 1) + 1, filas * (tamCelda - 1) + 1);
        }

        private void CargarPistasSeparadas()
        {
            lbHorizontales.Items.Clear();
            lbVerticales.Items.Clear();

            foreach (var p in _crucigrama.Palabras)
            {
                string itemTexto = $"({p.Fila + 1}, {p.Columna + 1}) — {p.Pista}";

                if (p.EsHorizontal())
                {
                    lbHorizontales.Items.Add(itemTexto);
                }
                else
                {
                    lbVerticales.Items.Add(itemTexto);
                }
            }

            if (lbHorizontales.Items.Count == 0) lbHorizontales.Items.Add("No hay pistas horizontales.");
            if (lbVerticales.Items.Count == 0) lbVerticales.Items.Add("No hay pistas verticales.");
        }

        private void lbHorizontales_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnfocarCeldaDesdePista(lbHorizontales.SelectedItem?.ToString());
        }

        private void lbVerticales_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnfocarCeldaDesdePista(lbVerticales.SelectedItem?.ToString());
        }

        private void EnfocarCeldaDesdePista(string textoPista)
        {
            if (string.IsNullOrEmpty(textoPista) || !textoPista.Contains("—")) return;

            foreach (var p in _crucigrama.Palabras)
            {
                string comparador = $"({p.Fila + 1}, {p.Columna + 1}) — {p.Pista}";
                if (comparador == textoPista)
                {
                    TextBox tbInicio = _celdas[p.Fila, p.Columna];
                    if (tbInicio.Enabled)
                    {
                        tbInicio.Focus();
                    }
                    break;
                }
            }
        }

        private void Celda_KeyPress(object sender, KeyPressEventArgs e)
        {
            var tb = (TextBox)sender;
            Point pos = (Point)tb.Tag;
            Celda celda = _crucigrama.Grilla[pos.X, pos.Y];
            if (celda.EstaVerificada)
                e.Handled = true; // Bloquea la celda si ya fue adivinada correctamente
        }

        private void Celda_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            Point pos = (Point)tb.Tag;
            int fila = pos.X;
            int columna = pos.Y;

            if (_crucigrama.Grilla[fila, columna].EstaVerificada) return;

            if (tb.BackColor == Color.LightCoral)
                tb.BackColor = Color.White;

            if (tb.Text.Length > 0)
                cJuego.IngresarLetra(_crucigrama, fila, columna, tb.Text[0]);
            else
                cJuego.BorrarLetra(_crucigrama, fila, columna);
        }

        private void btVerificar_Click(object sender, EventArgs e)
        {
            if (!seAdvirtio)
            {
                MessageBox.Show("Cada letra incorrecta que verifiques restará 10 puntos de tu puntuación base.",
                    "Reglas de Juego", MessageBoxButtons.OK, MessageBoxIcon.Information);
                seAdvirtio = true;
                return;
            }

            int filas = _crucigrama.Nivel.Filas;
            int columnas = _crucigrama.Nivel.Columnas;
            int letrasIncorrectasEnEstaVerificacion = 0;

            // Recorre toda la grilla para validar las respuestas
            for (int f = 0; f < filas; f++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    Celda celda = _crucigrama.Grilla[f, c];
                    TextBox tb = _celdas[f, c];

                    if (celda.EstaBloqueada || celda.EstaVerificada) continue;
                    if (celda.EstaVacia()) continue;

                    if (celda.EsCorrecta())
                    {
                        celda.EstaVerificada = true;
                        tb.BackColor = Color.LightGreen; // Verde si está bien
                        tb.ReadOnly = true;
                    }
                    else
                    {
                        tb.BackColor = Color.LightCoral; // Rojo si está mal
                        letrasIncorrectasEnEstaVerificacion++;
                    }
                }
            }

            // Resta puntos solo por la cantidad de celdas incorrectas encontradas
            if (letrasIncorrectasEnEstaVerificacion > 0)
            {
                _crucigrama.IntentosFallidos += letrasIncorrectasEnEstaVerificacion;
            }

            lbPuntaje.Text = $"PUNTUACIÓN: {cPuntaje.CalcularPuntajeActual(_crucigrama)}";

            // verificar si ya ganó
            if (cJuego.VerificarCrucigrama(_crucigrama))
            {
                var partida = cJuego.FinalizarPartida(_crucigrama, _usuario?.Id ?? 0);
                if (_usuario != null)
                    new pPartida().Agregar(partida);

                lbPuntaje.Text = $"PUNTUACIÓN FINAL: {partida.Puntaje}";

                MessageBox.Show($"¡Felicidades, resolviste el crucigrama!\nPuntaje total obtenido: {partida.Puntaje}",
                                "¡Victoria!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}