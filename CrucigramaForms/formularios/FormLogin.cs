using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace CrucigramaForms.formularios
{
    public partial class FormLogin : Form
    {
        private Label label1, label2, label3;
        private TextBox textBox1, textBox2, textBox3;
        private Button btIniciar, btCancelar;

        public FormLogin()
        {
            InitializeComponent();
            CrearFormularioLogin();
            InicializarModificar();
        }

        private void CrearFormularioLogin()
        {
            this.Size = new Size(600, 300); //tamaño del formulario
            this.FormBorderStyle = FormBorderStyle.FixedDialog; //evita q usuario redimensione el formulario
            this.StartPosition = FormStartPosition.CenterParent; // centra el formulario
            this.BackColor = Color.FromArgb(235, 240, 245); //color de fondo
            this.Font = new Font("Segoe UI", 10); // fuente del formulario
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;

            int xLabel = 120;
            int xControl = 230;
            int anchoLabel = 160;
            int anchoTextBox = 240;
            int alto = 24;
            int[] filaY = { 30, 80, 130, 180, 230 };

            //creamos label
            label1 = new Label { Left = xLabel, Top = filaY[0], Width = anchoLabel, Text = "Nombre: ", AutoSize = true };
            label2 = new Label { Left = xLabel, Top = filaY[1], Width = anchoLabel, Text = "Contraseña", AutoSize = true };

            //creamos textBox
            textBox1 = new TextBox { Left = xControl, Top = filaY[0], Width = anchoTextBox, Height = alto };
            textBox2 = new TextBox { Left = xControl, Top = filaY[1], Width = anchoTextBox, Height = alto };

            //botones
            int posi1 = this.ClientSize.Width / 2 / 2 - 243 / 2;
            int posi2 = this.ClientSize.Width / 2 / 2 * 3 - 243 / 2;

            int anchoBoton = 243;
            int MitadPantalla = this.ClientSize.Width / 2;
            int margen = (MitadPantalla - anchoBoton) / 2;
            int leftBtGuardar = margen;
            int leftBtCerrar = MitadPantalla + margen;

            btIniciar = new Button { Left = leftBtGuardar, Top = 178, Width = anchoBoton, Height = 48, Text = "Iniciar" };
            btCancelar = new Button { Left = leftBtCerrar, Top = 178, Width = anchoBoton, Height = 48, Text = "Cancelar" };
            //ancho boton = 243
            //areaUtil = this.ClientSize.Width , //ancho real del form

            //color boton
            btIniciar.BackColor = Color.FromArgb(0, 120, 212); //color de fondo del botón Guardar
            btIniciar.ForeColor = Color.White; //color del texto del botón Guardar
            btIniciar.FlatStyle = FlatStyle.Flat; //estilo plano para el botón Guardar

            //Hand cursor, lo cambia al cursor en una mano
            btIniciar.Cursor = Cursors.Hand;
            btCancelar.Cursor = Cursors.Hand;

            //btIniciar.Click += btIniciar_Click;
            //btCancelar.Click += btCancelar_Click;

            this.Controls.AddRange(new Control[] { label1, label2, textBox1, textBox2, btIniciar, btCancelar });


        }
        private void InicializarModificar()
        {

        }
    }
}
