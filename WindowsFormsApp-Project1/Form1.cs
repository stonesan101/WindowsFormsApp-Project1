using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp_Project1
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=WindowsFormApp;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        SqlCommand cmd;

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'windowsFormAppDataSet.Books' table. You can move, or remove it, as needed.
            this.booksTableAdapter.Fill(this.windowsFormAppDataSet.Books);

        }

        public void displayData()
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Books";
            cmd.ExecuteNonQuery();
            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            dataGridView.DataSource = dataTable;
        }

        private void clearFields()
        {
            txtBookId.Text = "";
            txtBookTitle.Text = "";
            txtBookAuthor.Text = "";
            txtStock.Text = "";
        }

        private void querySQL(string commandText)
        {
            connection.Open();
            SqlCommand query = connection.CreateCommand();
            query.CommandType = CommandType.Text;
            query.CommandText = commandText;
            query.ExecuteNonQuery();
            displayData();
            connection.Close();

            clearFields();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtBookId.Text == "" || txtBookTitle.Text == "" || txtBookAuthor.Text == "" || txtStock.Text == "") { return; }
            querySQL("INSERT INTO Books(BookId, BookTitle, BookAuthor, Stock) VALUES('" + int.Parse(txtBookId.Text) + "','" + txtBookTitle.Text + "','" + txtBookAuthor.Text + "','" + int.Parse(txtStock.Text) + "')");
            MessageBox.Show("Record saved successfully");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            querySQL("DELETE FROM Books WHERE BookId='" + int.Parse(txtBookId.Text) + "'");
            MessageBox.Show("Data Deleted Successfully");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtBookId.Text == "" || txtBookTitle.Text == "" || txtBookAuthor.Text == "" || txtStock.Text == "") { return; }
            querySQL("UPDATE Books SET BookTitle='" + txtBookTitle.Text + "', BookAuthor='" + txtBookAuthor.Text + "', Stock='" + int.Parse(txtStock.Text) + "' WHERE BookId='" + int.Parse(txtBookId.Text) + "'");
            MessageBox.Show("Data Updated Successfully");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            connection.Open();
            string searchTerm = txtSearch.Text;

            SqlCommand query = new SqlCommand("SELECT * FROM Books WHERE BookTitle LIKE '%" + searchTerm + "%' OR BookAuthor LIKE '%" + searchTerm + "%'", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(query);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                dataGridView.DataSource = dataTable;
            }
            else
            {
                MessageBox.Show("No results found for the search term: " + searchTerm);
            }
            connection.Close();
        }
    }
}
