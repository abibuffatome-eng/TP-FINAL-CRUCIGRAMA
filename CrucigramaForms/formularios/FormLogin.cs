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
            this.Size = new Size(580, 320); //tamaño del formulario
            this.FormBorderStyle = FormBorderStyle.FixedDialog; //evita q usuario redimensione el formulario
            this.StartPosition = FormStartPosition.CenterParent; // centra el formulario
            this.BackColor = Color.FromArgb(248, 246, 242); //color de fondo
            this.Font = new Font("Segoe UI", 10); // fuente del formulario
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;

            int xLabel = 80;
            int xControl = 200;
            int anchoLabel = 110;
            int anchoTextBox = 240;
            int alto = 26;
            int[] filaY = { 45, 95, 145 };

            //creamos label
            label1 = new Label { Left = xLabel, Top = filaY[0] + 3, Width = anchoLabel, Text = "USUARIO", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(110, 105, 100) };
            label2 = new Label { Left = xLabel, Top = filaY[1] + 3, Width = anchoLabel, Text = "CONTRASEÑA", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(110, 105, 100) };

            //creamos textBox
            textBox1 = new TextBox { Left = xControl, Top = filaY[0], Width = anchoTextBox, Height = alto, BorderStyle = BorderStyle.FixedSingle, ForeColor = Color.FromArgb(60, 60, 60) };
            textBox2 = new TextBox { Left = xControl, Top = filaY[1], Width = anchoTextBox, Height = alto, PasswordChar = '*', BorderStyle = BorderStyle.FixedSingle, ForeColor = Color.FromArgb(60, 60, 60) };

            //botones
            int anchoBoton = 190;
            int espacioEntreBotones = 30;
            int inicioX = (this.ClientSize.Width - (anchoBoton * 2 + espacioEntreBotones)) / 2;

            btIniciar = new Button
            {
                Left = inicioX,
                Top = filaY[2] + 20,
                Width = anchoBoton,
                Height = 42,
                Text = "INICIAR SESIÓN",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btIniciar.FlatAppearance.BorderSize = 0;

            btCancelar = new Button
            {
                Left = inicioX + anchoBoton + espacioEntreBotones,
                Top = filaY[2] + 20,
                Width = anchoBoton,
                Height = 42,
                Text = "CANCELAR",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(248, 246, 242),
                ForeColor = Color.FromArgb(100, 90, 85),
                FlatStyle = FlatStyle.Flat
            };
            btCancelar.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 188);
            btCancelar.FlatAppearance.BorderSize = 1;

            btMostrarContra = new Button
            {
                Left = xControl + anchoTextBox + 8,
                Top = filaY[1] - 1,
                Width = 85,
                Height = alto + 2,
                Text = "Mostrar",
                Font = new Font("Segoe UI", 8.5F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(100, 90, 85),
                FlatStyle = FlatStyle.Flat
            };
            btMostrarContra.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 188);

            //ancho boton = 243
            //areaUtil = this.ClientSize.Width , //ancho real del form

            //color boton
            // (Los colores, bordes limpios y flats ahora se configuran directamente arriba en la creación de cada botón)

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
