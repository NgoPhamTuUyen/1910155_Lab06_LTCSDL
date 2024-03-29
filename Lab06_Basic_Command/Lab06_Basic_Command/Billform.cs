﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab06_Basic_Command
{
    public partial class Billform : Form
    {
        string server = @"Data Source=.\SQLEXPRESS;Initial Catalog=RestaurantManagement;Integrated Security=True";
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

        public Billform()
        {
            InitializeComponent();
            
            loadaccount();
            LoadBills();
        }
        public void LoadBills()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=RestaurantManagement;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM Bills";

            connection.Open();

            string categoryName = command.ExecuteScalar().ToString();
            this.Text = "Danh sách toàn bộ hóa đơn";

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataTable table = new DataTable("Food");
            adapter.Fill(table);

            dgrbill.DataSource = table;

            // Prevent user to edit ID
            dgrbill.Columns[0].ReadOnly = true;

            connection.Close();
        }
        private void Billform_Load(object sender, EventArgs e)
        {
            
        }
        private void dgrbill_DoubleClick(object sender, EventArgs e)
        {
            Billdetail billDetails = new Billdetail();
            string billID = dgrbill.SelectedRows[0].Cells[0].Value.ToString();
            billDetails.loadform(int.Parse(billID));
            billDetails.Show();
        }

        
        private void loadaccount()
        {
            sqlConnection = new SqlConnection(server);
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "SELECT * FROM Account ";
            
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                cboAccount.Items.Add(name);
            }
            cboAccount.Text = cboAccount.Items[0].ToString();
            sqlConnection.Close();
        }
        private void loadbill()
        {
            sqlConnection = new SqlConnection(server);
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = " SELECT *  FROM Bills ";
            sqlConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable("Bills");
            adapter.Fill(dt);
            dgrbill.DataSource = dt;
            sqlConnection.Close();
            sqlConnection.Dispose();
            adapter.Dispose();
            
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(server);
            sqlCommand = sqlConnection.CreateCommand();
            sqlConnection.Open();
            loadbill();
            reset();
            sqlConnection.Close();
        }

        private void dgrbill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgrbill.CurrentRow.Index;
            txtID.Text = dgrbill.Rows[i].Cells[0].Value.ToString();
            txtName.Text = dgrbill.Rows[i].Cells[1].Value.ToString();
            cboTableID.Text = dgrbill.Rows[i].Cells[2].Value.ToString();
            cboAccount.Text = dgrbill.Rows[i].Cells[8].Value.ToString();
            txtTax.Text = dgrbill.Rows[i].Cells[5].Value.ToString();
            cbStatus.Checked = false;
            if (Convert.ToBoolean(dgrbill.Rows[i].Cells[6].Value) == true)
            {
                cbStatus.Checked = true;
            }
            if (dgrbill.Rows[i].Cells[7].Value.ToString() == "")
            { dtpcheckoutdate.Value = DateTime.Now; }
            else { dtpcheckoutdate.Value = Convert.ToDateTime(dgrbill.Rows[i].Cells[7].Value); }
            txtdiscount.Text = dgrbill.Rows[i].Cells[4].Value.ToString();
            txtTongtien.Text = dgrbill.Rows[i].Cells[3].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(server);
            sqlCommand = sqlConnection.CreateCommand();
            var text = string.Format("INSERT INTO Bills (Name, TableID, Amount, Discount, Tax, Status, CheckoutDate, Account ) Values (N'{0}', {1}, {2},'{3}','{4}','{5}','{6}',N'{7}')",
                txtName.Text,
                cboTableID.Text, txtTongtien.Text, txtdiscount.Text, txtTax.Text,
                cbStatus.Checked == true ? "true" : "false",dtpcheckoutdate.Value.ToShortDateString(), cboAccount.Text);
            sqlCommand.CommandText = text;
            sqlConnection.Open();
            int numOfRowsEffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if(numOfRowsEffected != 1)
            {
                MessageBox.Show("Đã có lỗi xảy ra , vui lòng thử lại ");
            }
            btnluu.PerformClick();
            reset();
        }

        private void reset()
        {
            txtID.Text = "";
            txtName.Text = "";
            cboTableID.Text = cboTableID.Items[0].ToString();
            cboAccount.Text = cboAccount.Items[0].ToString();
            txtTax.Text = "";
            dtpcheckoutdate.Value = DateTime.Now;
            cbStatus.Checked = false;
            txtdiscount.Text = "";
            txtTongtien.Text = "";
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(server);
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "Delete * from Bills where ID = " + txtID;
            sqlConnection.Open();
            int numOfRowEffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if(numOfRowEffected != 1)
            {
                MessageBox.Show("Đã có lỗi cảy ra , vui lòng thử lại ");
            }
            btnluu.PerformClick();
            reset();
        }

        

        private void btnsua_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(server);
            sqlCommand = sqlConnection.CreateCommand();
            var text = string.Format("Update Bills set Name = N'{0}', TableID = '{1}', Amount = {2}, Discount = '{3}', Tax = '{4}'" +
                ",Status = '{5}', CheckoutDate = '{6}', Account = N'{7}' where ID = '{8}'",
                txtName.Text,
                cboTableID.Text, txtTongtien.Text, txtdiscount.Text, txtTax.Text,
                cbStatus.Checked == true ? "true" : "false", dtpcheckoutdate.Value.ToShortDateString(), cboAccount.Text,txtID.Text).ToString();
            sqlCommand.CommandText = text;
            sqlConnection.Open();
            int numOfRowsEffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (numOfRowsEffected != 1)
            {
                MessageBox.Show("Đã có lỗi xảy ra , vui lòng thử lại ");
            }
            btnluu.PerformClick();
            reset();
        }


    }
}
