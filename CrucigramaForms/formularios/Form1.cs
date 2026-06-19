using CrucigramaForms.formularios;



namespace CrucigramaForms
{
    public partial class Form1 : Form
    {
        //creacion de botones
        private Button btRegistrar, btLogin, btSalirApp;
        public Form1()
        {
            InitializeComponent();
            CrearFormulario();
        }
        
        private void CrearFormulario()
        {
            this.Size = new Size(600, 480); //tamaño del formulario
            this.FormBorderStyle = FormBorderStyle.FixedDialog; //evita q usuario redimensione el formulario
            this.StartPosition = FormStartPosition.CenterParent; // centra el formulario
            this.BackColor = Color.FromArgb(248, 246, 242); //color de fondo
            this.Font = new Font("Segoe UI", 10); // fuente del formulario
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;

            // Título Principal Estilizado
            Label lblTitulo = new Label
            {
                Text = "Crucigrama PICNIC",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 85, 80), // Gris oliva oscuro sutil
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(0, 25, 0, 0)
            };

            // Créditos / Integrantes abajo de todo
            Label lblIntegrantes = new Label
            {
                Text = "By: Abi Buffa, Guillermina Künstler, Rodrigo Tancredi, Javier Guerra, Daniel de los Santos.",
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(140, 135, 130), // Color neutro bien clarito
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.Bottom,
                Height = 40,
                Padding = new Padding(0, 0, 0, 15)
            };

            //botones

            int anchoBoton = 243;
            int MitadPantalla = this.ClientSize.Width / 2;


            btRegistrar = new Button
            {
                Left = MitadPantalla - (anchoBoton / 2),
                Top = 120,
                Width = anchoBoton,
                Height = 46,
                Text = "REGISTRARSE",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btRegistrar.FlatAppearance.BorderSize = 0;

            btLogin = new Button
            {
                Left = MitadPantalla - (anchoBoton / 2),
                Top = 190,
                Width = anchoBoton,
                Height = 46,
                Text = "INICIAR SESIÓN",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(135, 152, 137),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btLogin.FlatAppearance.BorderSize = 0;

            btSalirApp = new Button
            {
                Left = MitadPantalla - (anchoBoton / 2),
                Top = 280,
                Width = anchoBoton,
                Height = 40,
                Text = "SALIR DE LA APP",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(248, 246, 242),
                ForeColor = Color.FromArgb(100, 90, 85),
                FlatStyle = FlatStyle.Flat
            };
            btSalirApp.FlatAppearance.BorderColor = Color.FromArgb(200, 195, 188);
            btSalirApp.FlatAppearance.BorderSize = 1;

            //color boton
            btRegistrar.BackColor = Color.FromArgb(135, 152, 137); //color de fondo del botón Guardar
            btRegistrar.ForeColor = Color.White; //color del texto del botón Guardar
            btRegistrar.FlatStyle = FlatStyle.Flat; //estilo plano para el botón Guardar

            btLogin.BackColor = Color.FromArgb(135, 152, 137); //color de fondo del botón Guardar
            btLogin.ForeColor = Color.White; //color del texto del botón Guardar
            btLogin.FlatStyle = FlatStyle.Flat; //estilo plano para el botón Guardar

            //Hand cursor, lo cambia al cursor en una mano
            btRegistrar.Cursor = Cursors.Hand;
            btLogin.Cursor = Cursors.Hand;
            btSalirApp.Cursor = Cursors.Hand;

            btRegistrar.Click += btRegistrar_Click;
            btLogin.Click += btLogin_Click;
            btSalirApp.Click += btSalirApp_Click;

            this.Controls.AddRange(new Control[] { lblTitulo, lblIntegrantes, btRegistrar, btLogin, btSalirApp });

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
        private void btSalirApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
