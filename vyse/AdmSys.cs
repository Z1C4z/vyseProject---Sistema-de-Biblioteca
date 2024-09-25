using MySql.Data.MySqlClient;
using System.Data;

namespace vyse
{
    public partial class AdmSys : Form
    {
        private const string connectionString = "Server=127.0.0.1;Port=3306;Database=bibliotecavyse;User ID=root;Password=acesso123;";
        private string nowTable = "";
        private string dataTypeOnTable = "";
        private string senderNow = "";

        public AdmSys()
        {
            InitializeComponent();
            tabControl1.TabPages.Remove(tabPage2);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            senderNow = sender.ToString();

            switch (sender.ToString())
            {
                case "Autores":
                    nowTable = "fato_autor";
                    dataTypeOnTable = "autor";
                    break;
                case "Generos":
                    nowTable = "fato_genero";
                    dataTypeOnTable = "genero";
                    break;
                case "Idiomas":
                    nowTable = "fato_idioma";
                    dataTypeOnTable = "idioma";
                    break;
                case "Titulos":
                    nowTable = "fato_titulo";
                    dataTypeOnTable = "titulo";
                    break;
                case "Status":
                    nowTable = "fato_status";
                    dataTypeOnTable = "status";
                    break;
                case "Veiculos":
                    nowTable = "fato_veiculos";
                    dataTypeOnTable = "placa";
                    break;
            }
            CommandSQLGeral($"SELECT * FROM {nowTable}", 0);
        }

        private void CommandSQLGeral(string query, int datagrid)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        if (datagrid == 0)
                            generalDataGrid.DataSource = ds.Tables[0];
                        else if (datagrid == 1)
                            book_datagrid.DataSource = ds.Tables[0];
                        else if (datagrid == 2)
                            generalDataGrid.DataSource = ds.Tables[0];
                    }
                }
                generalStatusCount.Text = $"Quantidade de {senderNow}: {generalDataGrid.Rows.Count}";
            }
            catch
            {

            }
        }

        private void generalButtonAdd_Click(object sender, EventArgs e)
        {
            switch (generalButtonAdd.Text)
            {
                case "Adicionar":
                    if (generalAddTextBox.Text == "")
                    {
                        MessageBox.Show("Adição não pode ser efetua.\nDigite o Nome do novo item", "Erro");
                    }
                    else
                    {
                        MySqlConnection conn = new MySqlConnection(connectionString);
                        conn.Open();
                        MySqlCommand comm = conn.CreateCommand();
                        comm.CommandText = $"INSERT INTO {nowTable}({dataTypeOnTable}) VALUES(?{dataTypeOnTable})";
                        comm.Parameters.Add($"?{dataTypeOnTable}", MySqlDbType.VarChar).Value = generalAddTextBox.Text;
                        comm.ExecuteNonQuery();
                        conn.Close();
                        generalAddTextBox.Text = "";
                        CommandSQLGeral($"SELECT * FROM {nowTable}", 0);
                    }
                    break;

                case "Confirmar Edição":
                    if (generalAddTextBox.Text == generalDataGrid.SelectedRows[0].Cells[dataTypeOnTable].Value.ToString())
                    {
                        MessageBox.Show("erro", "erro");
                        generalAddTextBox.Text = "";
                    }
                    else
                    {
                        CommandSQLGeral($"UPDATE {nowTable} SET {dataTypeOnTable} = '{generalAddTextBox.Text}'" +
                            $" WHERE id = {generalDataGrid.SelectedRows[0].Cells["id"].Value}", 0);
                        CommandSQLGeral($"SELECT * FROM {nowTable}", 0);
                    }
                    generalButtonAdd.Text = "Adicionar";
                    break;
            }
        }

        private void generalButtonSearch_Click(object sender, EventArgs e)
        {
            CommandSQLGeral($"SELECT * FROM {nowTable} WHERE {dataTypeOnTable} LIKE '{generalTextBox.Text}%'", 0);
        }

        private void generalEditButton_Click(object sender, EventArgs e)
        {

            if (generalDataGrid.SelectedRows.Count > 0)
            {
                generalButtonAdd.Text = "Confirmar Edição";
                generalAddTextBox.Text = generalDataGrid.SelectedRows[0].Cells[dataTypeOnTable].Value.ToString();
            }
            else
            {
                MessageBox.Show("A Edição não pode ser, pois não há uma linha selecionada", "Erro");
            }
        }

        private void generalRemoveButton_Click(object sender, EventArgs e)
        {
            if (generalDataGrid.SelectedRows.Count > 0)
            {
                CommandSQLGeral($"DELETE FROM {nowTable} WHERE id = {generalDataGrid.SelectedRows[0].Cells["id"].Value};", 0);
                CommandSQLGeral($"SELECT * FROM {nowTable}", 0);
            }
            else
            {
                MessageBox.Show("A Exclusão não pode ser, pois não há uma linha selecionada", "Erro");
            }
        }

        private void IniciateDataGridRegistro(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage6)
            {
                nowTable = "fato_autor";
                CommandSQLGeral($"SELECT * FROM {nowTable}", 0);
                tabControl1.TabPages.Remove(tabPage2);
            }
            else if (tabControl1.SelectedTab == tabPage1)
            {
                nowTable = "dim_livros";
                CommandSQLGeral(
                    @"SELECT 
                        l.id,
                        i.idioma AS idioma, 
                        t.titulo AS titulo,
                        g.genero AS genero,
                        a.autor AS autor,
                        v.placa AS placa
                    FROM 
                        dim_livros l
                    JOIN 
                        fato_idioma i ON l.idioma = i.id
                    JOIN 
                        fato_titulo t ON l.titulo = t.id
                    JOIN 
                        fato_genero g ON l.genero = g.id
                    JOIN 
                        fato_autor a ON l.autor = a.id
                    JOIN 
                        fato_veiculos v ON l.veiculo = v.id;",
                1);

                //CommandSQLGeral($"SELECT * FROM {nowTable}",1);
                tabControl1.TabPages.Remove(tabPage2);
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                tabControl1.TabPages.Remove(tabPage2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Insert(3, tabPage2);
            tabControl1.SelectedIndex = 3;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string auxTitulo = comboBox1.Text;
            
            //string auxAutor = comboBox2.Text;
            //string auxGenero = comboBox3.Text;
            //string auxIdioma = comboBox4.Text;
            //string auxVeiculo = comboBox5.Text;

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = $"SELECT id FROM fato_titulo WHERE titulo = '{auxTitulo}'";
            string a = (string)comm.ExecuteScalar();
            MessageBox.Show(a);
            //comm.CommandText = $"INSERT INTO dim_livros(titulo,autor,genero,idioma,veiculo) VALUES(@titulo,@autor,@genero,@idioma,@veiculo)";
            //comm.Parameters.AddWithValue($"@titulo", comboBox1.Text);
            //comm.Parameters.AddWithValue($"@autor", comboBox2.Text);
            //comm.Parameters.AddWithValue($"@genero", comboBox3.Text);
            //comm.Parameters.AddWithValue($"@idioma", comboBox4.Text);
            //comm.Parameters.AddWithValue($"@veiculo", comboBox5.Text);
            //comm.ExecuteNonQuery();
            conn.Close();
            //tabControl1.TabPages.Remove(tabPage2);
            //tabControl1.SelectedIndex = 0;
            //nowTable = "dim_livros";


        }
    }
}

