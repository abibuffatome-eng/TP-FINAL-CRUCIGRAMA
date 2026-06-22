using CrucigramaForms.Modelos;
using CrucigramaForms.Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CrucigramaForms.formularios
{
    public partial class FormRegistro : Form
    {
        private Label label1, label2, label3;
        private TextBox textBox1, textBox2, textBox3;
        private Button btRegistrar, btCancelar, btMostrarContra;
        private ComboBox cbTipoUsuario;
        public FormRegistro()
        {
            InitializeComponent();
            CrearFormularioRegistro();
        }

        private void FormRegistro_Load(object sender, EventArgs e)
        {
                
        }
        private void CrearFormularioRegistro()
        {
            this.Size = new Size(580, 320); 
            this.FormBorderStyle = FormBorderStyle.FixedDialog; 
            this.StartPosition = FormStartPosition.CenterParent; 
            this.BackColor = Color.FromArgb(248, 246, 242); 
            this.Font = new Font("Segoe UI", 10); 
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;

            int xLabel = 80;
            int xControl = 200;
            int anchoLabel = 110;
            int anchoTextBox = 240;
            int alto = 26;
            int[] filaY = { 35, 85, 135 };

            //creamos label
            label1 = new Label { Left = xLabel, Top = filaY[0] + 3, Width = anchoLabel, Text = "USUARIO", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(110, 105, 100) };
            label2 = new Label { Left = xLabel, Top = filaY[1] + 3, Width = anchoLabel, Text = "CONTRASEÑA", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(110, 105, 100) };
            label3 = new Label { Left = xLabel, Top = filaY[2] + 3, Width = anchoLabel, Text = "TIPO USUARIO", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(110, 105, 100) };


            //creamos textBox
            textBox1 = new TextBox { Left = xControl, Top = filaY[0], Width = anchoTextBox, Height = alto, BorderStyle = BorderStyle.FixedSingle, ForeColor = Color.FromArgb(60, 60, 60) };
            textBox2 = new TextBox { Left = xControl, Top = filaY[1], Width = anchoTextBox, Height = alto, PasswordChar = '*', BorderStyle = BorderStyle.FixedSingle, ForeColor = Color.FromArgb(60, 60, 60) };

            //creacion de comboBox
            cbTipoUsuario = new ComboBox
            {
                Left = xControl,
                Top = filaY[2],
                Width = anchoTextBox,
                Height = alto,
                FlatStyle = FlatStyle.Flat, // Estilo plano para que no tenga relieve viejo
                ForeColor = Color.FromArgb(60, 60, 60),
                DropDownStyle = ComboBoxStyle.DropDownList // Evita que el usuario escriba cualquier cosa en el combo
            };
            cbTipoUsuario.Items.Add("jugador");
            cbTipoUsuario.Items.Add("admin");
            cbTipoUsuario.SelectedIndex = 0; // selecciona "jugador" por defecto


            //botones
            int anchoBoton = 190;
            int espacioEntreBotones = 30;
            int inicioX = (this.ClientSize.Width - (anchoBoton * 2 + espacioEntreBotones)) / 2;

            btRegistrar = new Button
            {
                Left = inicioX,
                Top = filaY[2] + 45,
                Width = anchoBoton,
                Height = 42,
                Text = "REGISTRARSE",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btRegistrar.FlatAppearance.BorderSize = 0;

            btCancelar = new Button
            {
                Left = inicioX + anchoBoton + espacioEntreBotones,
                Top = filaY[2] + 45,
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

            //Hand cursor, lo cambia al cursor en una mano
            btRegistrar.Cursor = Cursors.Hand;
            btCancelar.Cursor = Cursors.Hand;
            btMostrarContra.Cursor = Cursors.Hand;

            btRegistrar.Click += btRegistrar_Click;
            btCancelar.Click += btCancelar_Click;
            btMostrarContra.Click += btMostrarContra_Click;

            this.Controls.AddRange(new Control[] { label1, label2, label3, textBox1, textBox2, cbTipoUsuario, btRegistrar, btCancelar, btMostrarContra });


        }
        private void btMostrarContra_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
            {
                textBox2.PasswordChar = '\0';        //  '\0' esto sirve para poder mostraar la contrasena y sacar el (*)
                btMostrarContra.Text = "Ocultar";
            }
            else
            {
                textBox2.PasswordChar = '*';
                btMostrarContra.Text = "Mostrar";
            }
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btRegistrar_Click(object sender, EventArgs e)
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

            // validación: nombre ya existe
            if (repo.ExisteNombre(nombre))
            {
                MessageBox.Show("Ese nombre de usuario ya existe.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // crear y guardar el usuario (usar el tipo seleccionado en el combo)
            var tipoSeleccionado = cbTipoUsuario.SelectedItem as string ?? "jugador";
            var nuevoUsuario = new Usuario(0, nombre, contrasena, tipoSeleccionado);
            repo.Agregar(nuevoUsuario);

            MessageBox.Show($"Usuario '{nombre}' registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
