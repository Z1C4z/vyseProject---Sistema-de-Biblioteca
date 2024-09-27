/*
Coisas que tem para ser feitas,
Separação de ADM para Usuario;
tela de emprestimos funcional
{
    selecione um livro;
    clique em fazer emprestimo;
    Selecione uma data de devolução em ate 30 dias;
    BOOM;
}
tela de historico de emprestismos com o ultimo emprestimo efetuado pela pessoa;
*/

using MySql.Data.MySqlClient;
using System.Data;

namespace vyse
{
    public partial class AdmSys : Form
    {
        public static string connectionString = "Server=127.0.0.1;Port=3306;Database=bibliotecavyse;User ID=root;Password=acesso123;";
        private string nowTable = "";
        private string dataTypeOnTable = "";
        private string senderNow = "";
        public static int typeUser = 0;
        public static int idUser = 0;

        public AdmSys()
        {
            InitializeComponent();
            if (typeUser == 0)
            {
                MessageBox.Show("Logado como Cliente", "Sucesso no Login");
                button1.Text = "Fazer um Emprestimo";
                tabControl1.TabPages.Remove(tabPage6);
            }
            else if (typeUser == 1)
            {
                MessageBox.Show("Logado como Administrador", "Sucesso no Login");
                emp_datagrid_actual.Visible = false;
            }

            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage_doEmp);
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
                            emp_datagrid.DataSource = ds.Tables[0];
                        else if (datagrid == 3)
                            emp_datagrid_actual.DataSource = ds.Tables[0];
                    }
                }
                generalStatusCount.Text = $"Quantidade de {senderNow}: {generalDataGrid.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um Erro: {ex.Message}","Falha ao exibir dados");
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
                generalAddTextBox.Text = Convert.ToString(generalDataGrid.SelectedRows[0].Cells[dataTypeOnTable].Value);
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
                        s.status AS status
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
                        fato_veiculos v ON l.veiculo = v.id
                    JOIN
                        fato_status s ON l.s;",
                    
                1);
                tabControl1.TabPages.Remove(tabPage2);
                book_statusStripLabel_totalBooks.Text = $"Quantidade Total de Livros [ {book_datagrid.Rows.Count} ]";
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                if (typeUser == 1)
                {
                    CommandSQLGeral("SELECT * FROM dim_emprestimos", 2);
                }
                else if (typeUser == 0)
                {
                    CommandSQLGeral($"SELECT * FROM dim_emprestimos WHERE usuario = {idUser} AND NOT (status = 3 OR status = 4)", 2);
                    CommandSQLGeral($"SELECT * FROM dim_emprestimos WHERE usuario = {idUser} AND (status = 3 OR status = 4)", 3);
                    emp_label_limit.Text = $"{emp_datagrid_actual.Rows.Count}/3";
                    emp_label_quats.Text = $"Quantidade de Emprestimos Feitos [ {emp_datagrid.Rows.Count + emp_datagrid_actual.Rows.Count} ].";
                }
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                tabControl1.TabPages.Remove(tabPage2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text == "Adicionar um Livro")
            {
                tabControl1.TabPages.Insert(3, tabPage2);
                tabControl1.SelectedIndex = 3;
            }
            else
            {
                if (book_datagrid.SelectedRows.Count < 0)
                    MessageBox.Show("Selecione um livro para fazer emprestimos");
                else
                {
                    doEmp_textBox_tittle.Text = book_datagrid.SelectedRows[0].Cells["id"].Value.ToString();
                    tabControl1.TabPages.Insert(2, tabPage_doEmp);
                    tabControl1.SelectedIndex = 2;
                    doEmp_dateTimePicker_choose.MaxDate = DateTime.Today.AddDays(30);
                    doEmp_dateTimePicker_choose.MinDate = DateTime.Today.AddDays(1);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> inputs = new List<string> { comboBox1.Text, comboBox2.Text, comboBox3.Text, comboBox4.Text, comboBox5.Text };
            if (inputs.Contains(""))
            {
                MessageBox.Show("Há valores não preenchidos", "Erro na adição");
                return;
            }

            List<int> tempList = new List<int>();
            List<string> fatos = new List<string> { "fato_titulo", "fato_autor", "fato_genero", "fato_idioma", "fato_veiculos" };
            List<string> type = new List<string> { "titulo", "autor", "genero", "idioma", "placa" };
            string text = "";

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();

            for (int i = 0; i < inputs.Count; i++)
            {
                comm.CommandText = $"SELECT id FROM {fatos[i]} WHERE {type[i]} = '{inputs[i]}'";
                int id = Convert.ToInt32(comm.ExecuteScalar());
                if (id == 0)
                {
                    if (text != "")
                        text += $"\n{type[i]} : {inputs[i]}";
                    else
                        text += $"{type[i]} : {inputs[i]}";
                }
                tempList.Add(id);
            }

            text += "\nDeseja adiciona-los? Selecione abaixo";
            DialogResult dialogResult = MessageBox.Show(text, "Os Seguintes Dados não forem encontrados", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i] == 0)
                    {
                        comm.CommandText = $"INSERT INTO {fatos[i]}({type[i]}) VALUES(@{type[i]})";
                        comm.Parameters.AddWithValue($"@{type[i]}", inputs[i]);
                        comm.ExecuteNonQuery();
                    }
                }

                comm.CommandText = $"INSERT INTO dim_livros(titulo,autor,genero,idioma,veiculo) VALUES(@titulo,@autor,@genero,@idioma,@veiculo)";
                comm.Parameters.AddWithValue($"@titulo", tempList[0]);
                comm.Parameters.AddWithValue($"@autor", tempList[1]);
                comm.Parameters.AddWithValue($"@genero", tempList[2]);
                comm.Parameters.AddWithValue($"@idioma", tempList[3]);
                comm.Parameters.AddWithValue($"@veiculo", tempList[4]);
                comm.ExecuteNonQuery();
                MessageBox.Show("Os valores foram adicionados no sistema");
                conn.Close();

                if (((Button)sender).Text == "Adicionar e Continuar")
                {
                    tabControl1.TabPages.Remove(tabPage2);
                    tabControl1.SelectedIndex = 0;
                    nowTable = "dim_livros";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearAddBookScreen(sender, e);
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.SelectedIndex = 0;
            nowTable = "dim_livros";
        }

        private void ClearAddBookScreen(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            ClearAddBookScreen(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            doEmp_textBox_tittle.Text = "";
            tabControl1.TabPages.Remove(tabPage_doEmp);
            tabControl1.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = $"INSERT INTO dim_emprestimos(livro,usuario,retirada,devolucao) VALUES(@livro,@usuario,@retirada,@devolucao)";
            comm.Parameters.AddWithValue($"@livro", Convert.ToInt32(doEmp_textBox_tittle.Text));
            comm.Parameters.AddWithValue($"@usuario", idUser);
            comm.Parameters.AddWithValue($"@retirada", doEmp_dateTimePicker_today.Value.ToString("dd/MM/yyyy"));
            comm.Parameters.AddWithValue($"@devolucao", doEmp_dateTimePicker_choose.Value.ToString("dd/MM/yyyy"));
            comm.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Emprestimo Concluido com sucesso","Sucesso ao Efetuar Emprestimo");
            doEmp_textBox_tittle.Text = "";
            tabControl1.TabPages.Remove(tabPage_doEmp);
            tabControl1.SelectedIndex = 0;
        }
    }
}