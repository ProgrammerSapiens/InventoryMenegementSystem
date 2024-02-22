using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryMenegementSystem
{
    public partial class CategoryForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\PotatoKiller\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        public CategoryForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            cmd = new SqlCommand("SELECT * FROM tbCategory", conn);
            conn.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            conn.Close();
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            CategoryModuleForm categoryModule = new CategoryModuleForm();
            categoryModule.btnSave.Enabled = true;
            categoryModule.btnUpdate.Enabled = false;
            categoryModule.ShowDialog();
            LoadCategory();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CategoryModuleForm categoryModule = new CategoryModuleForm();
                categoryModule.lblCatId.Text = dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                categoryModule.txtCatName.Text = dgvCategory.Rows[e.RowIndex].Cells[2].Value.ToString();

                categoryModule.btnSave.Enabled = false;
                categoryModule.btnUpdate.Enabled = true;
                categoryModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM tbCategory WHERE catId LIKE '" + dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }
            LoadCategory();
        }
    }
}
