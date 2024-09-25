using MySql.Data.MySqlClient;

namespace vyse
{
    public partial class LoginSys : Form
    {
        //string connectionString = "Server=127.0.0.1;Port=3306;Database=mydb;User ID=root;Password=acesso123;";


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



    }
}