using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab06_Basic_Command
{
    public partial class AccountManager : Form
    {
        string server = @"Data Source=.\SQLEXPRESS;Initial Catalog=RestaurantManagement;Integrated Security=True";
        SqlConnection conn;
        SqlCommand cmd;
        public AccountManager()
        {
            InitializeComponent();
            LoadAccount();
        }

        private void AccountManager_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'restaurantManagenmentDataSet3.Account' table. You can move, or remove it, as needed.
            //this.accountTableAdapter.Fill(this.restaurantManagenmentDataSet3.Account);
        }

        public void LoadAccount()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=RestaurantManagement;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM Account";

            connection.Open();

            string categoryName = command.ExecuteScalar().ToString();
            this.Text = "Danh sách toàn bộ tài khoản";

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataTable table = new DataTable("Food");
            adapter.Fill(table);

            dgvAccount.DataSource = table;

            // Prevent user to edit ID
            dgvAccount.Columns[0].ReadOnly = true;

            connection.Close();
        }

        private void loadform()
        {
            conn = new SqlConnection(server);
            cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Account";
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Account");
            adapter.Fill(dt);
            dgvAccount.DataSource = dt;
            conn.Close();
            conn.Dispose();
            adapter.Dispose();
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvAccount.CurrentRow.Index;
            txtAccount.Text = dgvAccount.Rows[i].Cells[0].Value.ToString();
            txtpassword.Text = dgvAccount.Rows[i].Cells[1].Value.ToString();
            txthoten.Text = dgvAccount.Rows[i].Cells[2].Value.ToString();
            txtemail.Text = dgvAccount.Rows[i].Cells[3].Value.ToString();
            txtsdt.Text = dgvAccount.Rows[i].Cells[4].Value.ToString();
            if (dgvAccount.Rows[i].Cells[5].Value.ToString() == "")
            { dtpdatecreate.Value = DateTime.Now; }
            else { dtpdatecreate.Value = Convert.ToDateTime(dgvAccount.Rows[i].Cells[5].Value); }
        }

        private void tsmxoa_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(server);
            cmd = conn.CreateCommand();
            cmd.CommandText = "delete from Account where AccountName = N'" + txtAccount.Text +"'";
            conn.Open();
            int num = cmd.ExecuteNonQuery();
            conn.Close();
            if(num !=1 )
            {
                MessageBox.Show("Loi");
            }
            loadform();
        }
    }
}
