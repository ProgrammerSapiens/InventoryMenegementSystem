using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryMenegementSystem
{
    public partial class ProductModuleForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\PotatoKiller\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboCat.Items.Clear();
            cmd = new SqlCommand("SELECT catName FROM tbCategory", conn);
            conn.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboCat.Items.Add(dr[0].ToString());
            }
            dr.Close();
            conn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand("INSERT INTO tbProduct(pName, pQty, pPrice, pDescription, pCategory) VALUES(@pName, @pQty, @pPrice, @pDescription, @pCategory)", conn);
                    cmd.Parameters.AddWithValue("@pName", txtPrName.Text);
                    cmd.Parameters.AddWithValue("@pQty", Convert.ToInt32(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@pPrice", Convert.ToInt32(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@pDescription", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@pCategory", comboCat.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Product has been successfully saved");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand("UPDATE tbProduct SET pName=@pName, pQty=@pQty, pPrice=@pPrice, pDescription=@pDescription, pCategory=@pCategory WHERE pId LIKE'" + lblpId.Text + "'", conn);
                    cmd.Parameters.AddWithValue("@pName", txtPrName.Text);
                    cmd.Parameters.AddWithValue("@pQty", Convert.ToInt32(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@pPrice", Convert.ToInt32(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@pDescription", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@pCategory", comboCat.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Product has been successfully updated");
                    Clear();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtPrName.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            comboCat.Text = "";
        }
    }
}
