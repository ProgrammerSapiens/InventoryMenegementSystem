using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryMenegementSystem
{
    public partial class OrderForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\PotatoKiller\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        public OrderForm()
        {
            InitializeComponent();
            LoadOrder();
        }

        public void LoadOrder()
        {
            int i = 0;
            dgvOrder.Rows.Clear();
            cmd = new SqlCommand("SELECT orderId, odate, tbOrder.pId, pName, tbOrder.cId, cname, qty, price, total FROM tbOrder JOIN tbCustomer ON tbOrder.cId=tbCustomer.cId JOIN tbProduct ON tbOrder.pId=tbProduct.pId WHERE CONCAT(orderId, odate, tbOrder.pId, pName, tbOrder.cId, cname, qty, price) LIKE '%" + txtSearch.Text + "%' ", conn);
            conn.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvOrder.Rows.Add(i, dr[0].ToString(), Convert.ToDateTime(dr[1].ToString()).ToString("dd/MM/yyyy"), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
            }
            dr.Close();
            conn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OrderModuleForm moduleForm = new OrderModuleForm();
            moduleForm.ShowDialog();
            LoadOrder();
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOrder.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this order?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM tbOrder WHERE orderId LIKE '" + dgvOrder.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully deleted!");

                    cmd = new SqlCommand("UPDATE tbProduct SET pQty=(pQty+@pQty) WHERE pId LIKE '" + dgvOrder.Rows[e.RowIndex].Cells[3].Value.ToString() + "' ", conn);
                    cmd.Parameters.AddWithValue("@pQty", Convert.ToInt32(dgvOrder.Rows[e.RowIndex].Cells[5].Value.ToString()));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            LoadOrder();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadOrder();
        }
    }
}
