using MySql.Data.MySqlClient;

namespace vyse
{
    public partial class LoginSys : Form
    {
        string connectionString = AdmSys.connectionString;
        bool vissiblePassWord = false;
        Image img1 = Image.FromFile("C:\\Users\\1016051\\source\\repos\\vyse\\vyse\\images\\blockEye.jpg");
        Image img2 = Image.FromFile("C:\\Users\\1016051\\source\\repos\\vyse\\vyse\\images\\openEye.jpg");

        public LoginSys()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void login_button_confirm_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand mySqlCommand = conn.CreateCommand();
            MySqlCommand comm = mySqlCommand;
            comm.CommandText = $"SELECT id FROM dim_usuario WHERE login = '{login_textBox_usu.Text}' AND senha = '{login_textBox_password.Text}'";
            string a = Convert.ToString(comm.ExecuteScalar());
            conn.Close();
            if (a != "")
            {
                AdmSys.typeUser = Convert.ToInt32(a);
                new AdmSys().Show();
                this.Hide();
            }
            else
                MessageBox.Show("Este usuario nao existe no universo","Erro Existencial");
        }

        private void cad_button_confirm_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand mySqlCommand = conn.CreateCommand();
            MySqlCommand comm = mySqlCommand;
            comm.CommandText = $"SELECT login FROM dim_usuario WHERE login = '{cad_textBox_usu.Text}'";
            if (Convert.ToString(comm.ExecuteScalar()) == "")
            {
                MessageBox.Show("Usuario ja cadastrado, Porfavor ensira um nome diferente");
            }
            //cad_textBox_usu.Text;
            //cad_textBox_password.Text;
            //cad_taxtBox_confirmword.Text;
            //cad_textBox_gender.Text;

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (vissiblePassWord)
            {
                btn.Image = new Bitmap(img1, new Size(btn.Width, btn.Height));
                btn.Image.Tag = "imagem1";
                if (btn.Name == "login_vissebleButton")
                {
                    login_textBox_password.PasswordChar = '*';
                }
                else
                {
                    cad_taxtBox_confirmword.PasswordChar = '*';
                    cad_textBox_password.PasswordChar = '*';
                }
                vissiblePassWord = false;
            }
            else
            {
                btn.Image = new Bitmap(img2, new Size(btn.Width, btn.Height));
                btn.Image.Tag = "imagem2";
                if (btn.Name == "login_vissebleButton")
                {
                    login_textBox_password.PasswordChar = '\0';
                }
                else
                {
                    cad_taxtBox_confirmword.PasswordChar = '\0';
                    cad_textBox_password.PasswordChar = '\0';
                }
                vissiblePassWord = true;
            }

            btn.BackgroundImageLayout = ImageLayout.Zoom;


        }
    }
}