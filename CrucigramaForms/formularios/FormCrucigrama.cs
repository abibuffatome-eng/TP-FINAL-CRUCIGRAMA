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
        private ListBox lbPistas;
        private Label lbPuntaje;
        private bool seAdvirtio = false; 
        private bool huboError = false;

        // constructor de prueba — sin parámetros
        public FormCrucigrama()
        {
            InitializeComponent();
            CrearFormulario();
        }

        // constructor real — recibe crucigrama y usuario
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
            this.Text = "  " + _crucigrama.Titulo;
            // Aumentamos el tamaño total del formulario
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 246, 242); //color de fondo crema
            this.Font = new Font("Segoe UI", 11); // Fuente base un poco más grande

            panelGrilla = new Panel();
            panelGrilla.Location = new Point(35, 35); // Un poco más de margen inicial
            panelGrilla.AutoSize = true;
            panelGrilla.BackColor = Color.Transparent;

            ArmarGrilla();


            // LISTBOX DE PISTAS: más a la derecha y con fuente más grande
            lbPistas = new ListBox
            {
                Location = new Point(800, 45), // Posición fija a la derecha para que no se solape
                Size = new Size(350, 500),    // Más ancho y más alto
                Font = new Font("Segoe UI", 10.5F), // Letra más legible
                ForeColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            lbPistas.Items.Add("--- PISTAS ---");
            foreach (var p in _crucigrama.Palabras)
            {
                string orientacion = p.EsHorizontal() ? "Horizontal" : "Vertical";
                lbPistas.Items.Add($"({p.Fila}, {p.Columna}) [{orientacion}]");
                lbPistas.Items.Add($"  -> {p.Pista}");
                lbPistas.Items.Add(""); // Espacio extra
            }
            lbPuntaje = new Label
            {
                Text = $"Puntaje: {_crucigrama.Nivel.PuntajeBase}",
                Location = new Point(800, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 212)
            };

            // BOTONES más grandes y con mejor disposición
            btVerificar = new Button
            {
                Text = "VERIFICAR",
                Size = new Size(180, 46), // Botón más grande equilibrado
                Location = new Point(35, 620), // Debajo de la grilla
                BackColor = Color.FromArgb(135, 152, 137), // Verde Sage
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btVerificar.FlatAppearance.BorderSize = 0;


            btSalir = new Button
            {
                Text = "SALIR",
                Size = new Size(180, 46),
                Location = new Point(235, 620), // A la derecha del de verificar
                BackColor = Color.FromArgb(248, 246, 242),
                ForeColor = Color.FromArgb(100, 90, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btSalir.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 188);
            btSalir.FlatAppearance.BorderSize = 1;

            btVerificar.Click += btVerificar_Click;
            btSalir.Click += btSalir_Click;

            this.Controls.AddRange(new Control[] { panelGrilla, btVerificar, btSalir, lbPistas, lbPuntaje });


        }
        private void ArmarGrilla()
        {
            int filas = _crucigrama.Nivel.Filas;
            int columnas = _crucigrama.Nivel.Columnas;
            int tamCelda = 38;

            _celdas = new TextBox[filas, columnas];

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
                        Font = new Font("Segoe UI", 13, FontStyle.Regular),
                        ForeColor = Color.FromArgb(60, 60, 60),
                        MaxLength = 1,
                        Tag = new Point(f, c),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    if (celda.EstaBloqueada)
                    {
                        tb.BackColor = Color.FromArgb(120, 110, 102);
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
        }

        private void lbPistas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtener la palabra seleccionada
            var palabra = _crucigrama.Palabras[lbPistas.SelectedIndex];

            // Resaltar el inicio de la palabra en la grilla (puedes cambiar el color del borde o fondo)
            TextBox tbInicio = _celdas[palabra.Fila, palabra.Columna];
            tbInicio.Focus();
            tbInicio.BackColor = Color.LightYellow; // Un color que indique selección
        }
        private void Celda_KeyPress(object sender, KeyPressEventArgs e)
        {
            var tb = (TextBox)sender;
            Point pos = (Point)tb.Tag;
            Celda celda = _crucigrama.Grilla[pos.X, pos.Y];
            if (celda.EstaVerificada)
                e.Handled = true; // bloquea cualquier tecla
        }
        private void Celda_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            Point pos = (Point)tb.Tag;
            int fila = pos.X;
            int columna = pos.Y;

            // si la celda ya fue verificada como correcta, ignorar
            if (_crucigrama.Grilla[fila, columna].EstaVerificada) return;

            // resetear color rojo al escribir de nuevo
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
                MessageBox.Show($"cada verificacion que tenga un error restara 10 puntos al puntaje total",
                    "advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                seAdvirtio = true;
            }
            else
            {
                int filas = _crucigrama.Nivel.Filas;
                int columnas = _crucigrama.Nivel.Columnas;

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
                            tb.BackColor = Color.LightGreen;
                            tb.ReadOnly = true;
                        }
                        else
                        {
                            tb.BackColor = Color.LightCoral;
                            huboError = true;
                        }
                    }
                }

                // verificar si ya ganó
                if (cJuego.VerificarCrucigrama(_crucigrama))
                {
                    var partida = cJuego.FinalizarPartida(_crucigrama, _usuario?.Id ?? 0);
                    if (_usuario != null)
                        new pPartida().Agregar(partida);

                    lbPuntaje.Text = $"Puntaje: {partida.Puntaje}";

                    MessageBox.Show($"¡Ganaste! Puntaje: {partida.Puntaje}",
                                    "¡Felicitaciones!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    //MessageBox.Show($"se te descontaron 10 punto",
                    //    "advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //verificacion fallida
                    if (huboError)
                    {
                        _crucigrama.IntentosFallidos++;
                        huboError = false;
                    }
                    lbPuntaje.Text = $"Puntaje: {cPuntaje.CalcularPuntajeActual(_crucigrama)}";
                }
            }
        }
        private void btSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}