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
            string id = Convert.ToString(comm.ExecuteScalar());
            if (id != "")
            {
                comm.CommandText = $"SELECT tipo FROM dim_usuario WHERE login = '{login_textBox_usu.Text}'";
                AdmSys.typeUser = Convert.ToInt32(comm.ExecuteScalar());
                AdmSys.idUser = Convert.ToInt32(id);
                new AdmSys().Show();
                this.Hide();
            }
            else
                MessageBox.Show("Este usuario nao existe no universo","Erro Existencial");
            conn.Close();
        }

        private void cad_button_confirm_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand mySqlCommand = conn.CreateCommand();
            MySqlCommand comm = mySqlCommand;

            comm.CommandText = $"SELECT login FROM dim_usuario WHERE login = '{cad_textBox_usu.Text}'";

            if (cad_textBox_usu.Text == "" || cad_textBox_password.Text == "" || cad_taxtBox_confirmword.Text == "")
            {
                MessageBox.Show("Preencha todas as informações","Falha ao efetuar Cadastro");
                return;
            }

            if (Convert.ToString(comm.ExecuteScalar()) != "")
            {
                MessageBox.Show("Usuario ja cadastrado, Porfavor ensira um nome diferente","Falha ao Cadastrar Usuario");
                return;
            }

            if (cad_taxtBox_confirmword.Text != cad_textBox_password.Text)
            {
                MessageBox.Show("Os dois campos de Senha devem conter a mesma senha","Falha ao cadastrar senha");
                return;
            }

            comm.CommandText = $"SELECT id FROM fato_genero WHERE genero = '{cad_textBox_gender.Text}'";
            string tempIdGender = Convert.ToString(comm.ExecuteScalar());

            comm.CommandText = $"INSERT INTO dim_usuario(login,senha,genero) VALUES(?login,?senha,?genero)";

            comm.Parameters.AddWithValue($"?login", cad_textBox_usu.Text);
            comm.Parameters.AddWithValue($"?senha", cad_textBox_password.Text);

            if (tempIdGender == "" && cad_textBox_gender.Text == "")
                comm.Parameters.AddWithValue($"?genero", DBNull.Value);
            else if (tempIdGender == "" && cad_textBox_gender.Text != "")
            {
                MessageBox.Show("Genero nao Encontrado","Falha ao buscar genero");
                cad_textBox_gender.Text = "";
                return;
            }
            else
                comm.Parameters.AddWithValue($"?genero", Convert.ToInt32(tempIdGender));

            comm.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Cadastro feito com sucesso");

            tabControl1.SelectedIndex = 0;

            cad_textBox_usu.Text = "";
            cad_textBox_password.Text = "";
            cad_taxtBox_confirmword.Text = "";
            cad_textBox_gender.Text = "";
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