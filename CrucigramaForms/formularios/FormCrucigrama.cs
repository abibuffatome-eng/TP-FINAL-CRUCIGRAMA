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
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrucigramaForms.formularios
{
    public partial class FormCrucigrama : Form
    {
        private Crucigrama _crucigrama;
        private Usuario _usuario;
        private TextBox[,] _celdas;
        private Button btVerificar, btSalir;
        private Panel panelGrilla;

        // constructor de prueba — sin parámetros
        public FormCrucigrama()
        {
            InitializeComponent();

            var nivel = new Nivel(2, "Medio", 10, 10, 250);
            _crucigrama = new Crucigrama(1, "Crucigrama Medio", nivel);
            _crucigrama.Palabras.Add(new Palabra(1, 1, "GATO", "Animal doméstico", 0, 0, "horizontal"));
            _crucigrama.Palabras.Add(new Palabra(2, 1, "GUERRA", "Conflicto armado", 0, 0, "vertical"));

            cJuego.IniciarPartida(_crucigrama);
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
            this.Size = new Size(820, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 246, 242); 
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            panelGrilla = new Panel();
            panelGrilla.Location = new Point(35, 35); 
            panelGrilla.AutoSize = true;
            panelGrilla.BackColor = Color.Transparent; 

            ArmarGrilla();

            
            btVerificar = new Button
            {
                Text = "VERIFICAR",
                Size = new Size(160, 42),
                Location = new Point(35, 450),
                BackColor = Color.FromArgb(135, 152, 137), 
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btVerificar.FlatAppearance.BorderSize = 0; 

            
            btSalir = new Button
            {
                Text = "SALIR",
                Size = new Size(160, 42),
                Location = new Point(210, 450),
                BackColor = Color.FromArgb(248, 246, 242), 
                ForeColor = Color.FromArgb(100, 90, 85),   
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btSalir.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 188); 
            btSalir.FlatAppearance.BorderSize = 1;

            btVerificar.Click += btVerificar_Click;
            btSalir.Click += btSalir_Click;

            this.Controls.AddRange(new Control[] { panelGrilla, btVerificar, btSalir });
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
            int filas = _crucigrama.Nivel.Filas;
            int columnas = _crucigrama.Nivel.Columnas;

            for (int f = 0; f < filas; f++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    Celda celda = _crucigrama.Grilla[f, c];
                    TextBox tb = _celdas[f, c];

                    // saltear bloqueadas (negras) y ya verificadas (verdes)
                    if (celda.EstaBloqueada || celda.EstaVerificada) continue;

                    // saltear celdas vacías
                    if (celda.EstaVacia()) continue;

                    if (celda.EsCorrecta())
                    {
                        celda.EstaVerificada = true;
                        tb.BackColor = Color.LightGreen;
                        tb.ReadOnly = true;         // no se puede editar más
                    }
                    else
                    {
                        tb.BackColor = Color.LightCoral; // rojo suave para incorrectas
                    }
                }
            }

            // verificar si ya ganó
            if (cJuego.VerificarCrucigrama(_crucigrama))
            {
                var partida = cJuego.FinalizarPartida(_crucigrama, _usuario?.Id ?? 0);
                if (_usuario != null)
                    new pPartida().Agregar(partida);

                MessageBox.Show($"¡Ganaste! Puntaje: {partida.Puntaje}",
                                "¡Felicitaciones!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}