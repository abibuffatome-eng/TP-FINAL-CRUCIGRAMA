using CrucigramaForms.formularios;
using System.Reflection.Emit;


namespace CrucigramaForms
{
    public partial class Form1 : Form
    {
        private Button btRegistrar, btLogin;
        public Form1()
        {
            InitializeComponent();
            CrearFormulario();
        }
        private void CrearFormulario()
        {
            this.Size = new Size(600, 400); //tamaño del formulario
            this.FormBorderStyle = FormBorderStyle.FixedDialog; //evita q usuario redimensione el formulario
            this.StartPosition = FormStartPosition.CenterParent; // centra el formulario
            this.BackColor = Color.FromArgb(235, 240, 245); //color de fondo
            this.Font = new Font("Segoe UI", 10); // fuente del formulario
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;

            //botones

            int anchoBoton = 243;
            int MitadPantalla = this.ClientSize.Width / 2;


            btRegistrar = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 78, Width = anchoBoton, Height = 48, Text = "Registrar" };
            btLogin = new Button { Left = MitadPantalla - (anchoBoton / 2), Top = 178, Width = anchoBoton, Height = 48, Text = "Login" };

            //color boton
            btRegistrar.BackColor = Color.FromArgb(0, 120, 212); //color de fondo del botón Guardar
            btRegistrar.ForeColor = Color.White; //color del texto del botón Guardar
            btRegistrar.FlatStyle = FlatStyle.Flat; //estilo plano para el botón Guardar

            //Hand cursor, lo cambia al cursor en una mano
            btRegistrar.Cursor = Cursors.Hand;
            btLogin.Cursor = Cursors.Hand;

            btRegistrar.Click += btRegistrar_Click;
            btLogin.Click += btLogin_Click;

            this.Controls.AddRange(new Control[] { btRegistrar, btLogin });

        }
        private void btRegistrar_Click (object sender, EventArgs e)
        {
            FormRegistro formRegistro = new FormRegistro();
            formRegistro.ShowDialog();
        }
        private void btLogin_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
