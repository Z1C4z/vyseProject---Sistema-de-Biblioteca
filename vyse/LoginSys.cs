using MySql.Data.MySqlClient;

namespace vyse
{
    public partial class LoginSys : Form
    {
        string connectionString = AdmSys.connectionString;


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
    }
}