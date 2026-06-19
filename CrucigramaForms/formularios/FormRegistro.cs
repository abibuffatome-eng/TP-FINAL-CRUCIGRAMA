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
            label3 = new Label { Left = xLabel, Top = filaY[2], Width = anchoLabel, Text = "TipoUsuario", AutoSize = true };


            //creamos textBox
            textBox1 = new TextBox { Left = xControl, Top = filaY[0], Width = anchoTextBox, Height = alto };
            textBox2 = new TextBox { Left = xControl, Top = filaY[1], Width = anchoTextBox, Height = alto, PasswordChar = '*' };

            //creacion de comboBox
            cbTipoUsuario = new ComboBox { Left = xControl, Top = filaY[2], Width = anchoTextBox, Height = alto };
            cbTipoUsuario.Items.Add("jugador");
            cbTipoUsuario.Items.Add("admin");
            cbTipoUsuario.SelectedIndex = 0; // selecciona "jugador" por defecto


            //botones
            int posi1 = this.ClientSize.Width / 2 / 2 - 243 / 2;
            int posi2 = this.ClientSize.Width / 2 / 2 * 3 - 243 / 2;

            int anchoBoton = 243;
            int MitadPantalla = this.ClientSize.Width / 2;
            int margen = (MitadPantalla - anchoBoton) / 2;
            int leftBtGuardar = margen;
            int leftBtCerrar = MitadPantalla + margen;

            btRegistrar = new Button { Left = leftBtGuardar, Top = 178, Width = anchoBoton, Height = 48, Text = "Registrar" };
            btCancelar = new Button { Left = leftBtCerrar, Top = 178, Width = anchoBoton, Height = 48, Text = "Cancelar" };
            btMostrarContra = new Button { Left = xControl + anchoTextBox + 5, Top = filaY[1]  , Width = 100, Height = alto + 7, Text = "Mostrar" };

           
            

            //color boton
            btRegistrar.BackColor = Color.FromArgb(0, 120, 212); //color de fondo del botón Guardar
            btRegistrar.ForeColor = Color.White; //color del texto del botón Guardar
            btRegistrar.FlatStyle = FlatStyle.Flat; //estilo plano para el botón Guardar

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
