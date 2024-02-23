using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryMenegementSystem
{
    public partial class ProductForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\PotatoKiller\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        public ProductForm()
        {
            InitializeComponent();
            LoadProduct();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cmd = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pId, pName, pPrice, pDescription, pCategory) LIKE '%" + txtSearch.Text + "%'", conn);
            conn.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            conn.Close();
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            ProductModuleForm productModule = new ProductModuleForm();
            productModule.btnSave.Enabled = true;
            productModule.btnUpdate.Enabled = false;
            productModule.ShowDialog();
            LoadProduct();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm productModule = new ProductModuleForm();
                productModule.lblpId.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                productModule.txtPrName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                productModule.txtQuantity.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                productModule.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                productModule.txtDescription.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                productModule.comboCat.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                productModule.btnSave.Enabled = false;
                productModule.btnUpdate.Enabled = true;
                productModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this product?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM tbProduct WHERE pId LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }
            LoadProduct();
        }

        private void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            LoadProduct();
        }
    }
}
