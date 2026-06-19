using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrucigramaForms.Modelos;
using CrucigramaForms.Persistencia;


namespace CrucigramaForms.formularios
{
    public partial class FormLogin : Form
    {
        //creacion cosas
        private Label label1, label2, label3;
        private TextBox textBox1, textBox2, textBox3;
        private Button btIniciar, btCancelar, btMostrarContra;

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
            textBox2 = new TextBox { Left = xControl, Top = filaY[1], Width = anchoTextBox, Height = alto, PasswordChar = '*' };

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
            btMostrarContra = new Button { Left = xControl + anchoTextBox + 5, Top = filaY[1], Width = 100, Height = alto + 7, Text = "Mostrar" };
            //ancho boton = 243
            //areaUtil = this.ClientSize.Width , //ancho real del form

            //color boton
            btIniciar.BackColor = Color.FromArgb(0, 120, 212); //color de fondo del botón Guardar
            btIniciar.ForeColor = Color.White; //color del texto del botón Guardar
            btIniciar.FlatStyle = FlatStyle.Flat; //estilo plano para el botón Guardar

            //Hand cursor, lo cambia al cursor en una mano
            btIniciar.Cursor = Cursors.Hand;
            btCancelar.Cursor = Cursors.Hand;
            btMostrarContra.Cursor = Cursors.Hand;

            btIniciar.Click += btIniciar_Click;
            btCancelar.Click += btCancelar_Click;
            btMostrarContra.Click += btMostrarContra_Click;

            this.Controls.AddRange(new Control[] { label1, label2, textBox1, textBox2, btIniciar, btCancelar, btMostrarContra });


        }
        private void btMostrarContra_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
            {
                textBox2.PasswordChar = '\0';        //  '\0' significa "sin carácter", muestra el texto normal
                btMostrarContra.Text = "Ocultar";
            }
            else
            {
                textBox2.PasswordChar = '*';
                btMostrarContra.Text = "Mostrar";
            }
        }
        private void btIniciar_Click(object sender, EventArgs e)
        {
            string nombre = textBox1.Text.Trim();
            string contrasena = textBox2.Text.Trim();

            // validación: campos vacíos
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Completá todos los campos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var repo = new pUsuario();
            Usuario usuario = repo.Login(nombre, contrasena);

            // si no encontró el usuario, Login devuelve null
            if (usuario == null)
            {
                MessageBox.Show("Nombre o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // login exitoso — abrís la pantalla según el tipo de usuario
            MessageBox.Show($"Bienvenido, {usuario.Nombre}!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (usuario.EsAdmin())
            {
                var admin = new FormAdmin();
                this.Hide();
                admin.ShowDialog();
            }
            else
            {
                FormNiveles niveles = new FormNiveles(usuario);
                this.Hide();
                niveles.ShowDialog();
            }

            this.Close();
        }
        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InicializarModificar()
        {

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
