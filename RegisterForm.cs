using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSLoginRegisterForm.Connection;

namespace CSLoginRegisterForm
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            clearControls();
            firstNameTextBox.Select();
        }

        private void clearControls()
        {
            foreach(TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            loadUserData();
            firstNameTextBox.Select();
        }

        private void loadUserData()
        {
            DataTable userData = ServerConnection.executeSQL("SELECT (First_Name + ' ' + Last_Name) AS Fullname, Username FROM LoginTbl");
            dataGridView1.DataSource = userData;
            dataGridView1.Columns[0].HeaderText = "Full Name";
            dataGridView1.Columns[1].HeaderText = "Username";
            dataGridView1.Columns[0].Width = 234;
            dataGridView1.Columns[1].Width = 234;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Do you want to permanently delete the selected record?",
                    "Delete Data : iBasskung Tutorial", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {

                    ServerConnection.executeSQL("DELETE FROM LoginTbl WHERE Username = '" + dataGridView1.CurrentRow.Cells[1].Value + "'");

                    loadUserData();

                    MessageBox.Show("The record has been deleted.",
                        "Delete Data : iBasskung Tutorial",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    return;
                }

            }
            catch (Exception)
            {
                // An error occured!
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons btn = MessageBoxButtons.OK;
            MessageBoxIcon ico = MessageBoxIcon.Information;
            string caption = "Save Data : iBasskung Tutorial";

            if (string.IsNullOrEmpty(firstNameTextBox.Text))
            {
                MessageBox.Show("Please enter First Name.", caption, btn, ico);
                firstNameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(lastNameTextBox.Text))
            {
                MessageBox.Show("Please enter Last Name.", caption, btn, ico);
                lastNameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(usernameTextBox.Text))
            {
                MessageBox.Show("Please enter Username.", caption, btn, ico);
                usernameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show("Please enter Password.", caption, btn, ico);
                passwordTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(confirmPasswordTextBox.Text))
            {
                MessageBox.Show("Please enter Confirmation Password.", caption, btn, ico);
                confirmPasswordTextBox.Select();
                return;
            }

            if (passwordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Your password and confirmation password do not match.", caption, btn, ico);
                confirmPasswordTextBox.SelectAll();
                return;
            }


            string yourSQL = "SELECT Username FROM LoginTbl WHERE Username = '" + usernameTextBox.Text + "'";
            DataTable checkDuplicates = CSLoginRegisterForm.Connection.ServerConnection.executeSQL(yourSQL);

            if (checkDuplicates.Rows.Count > 0)
            {
                MessageBox.Show("The username already exists. Please try another username.",
                    "C# Registration Form : iBasskung Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                usernameTextBox.SelectAll();
                return;
            }

            DialogResult result;
            result = MessageBox.Show("Do you want to save the record?", "Save Data : iBasskung Tutorial", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string mySQL = string.Empty;

                mySQL += "INSERT INTO LoginTbl (First_Name, Last_Name, Email_Address, Username, Password) ";
                mySQL += "VALUES ('" + firstNameTextBox.Text + "','" + lastNameTextBox.Text + "','" + emailTextBox.Text + "',";
                mySQL += "'" + usernameTextBox.Text + "','" + passwordTextBox.Text + "')";

                CSLoginRegisterForm.Connection.ServerConnection.executeSQL(mySQL);

                MessageBox.Show("The record has been saved successfully.",
                                "Save Data : iBasskung Tutorial", MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);

                loadUserData();
                clearControls();

            }

        }
    }
}