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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            usernameTextBox.Select();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenRegisterFromLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.ShowDialog();
        }

        private void ShowPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(showPasswordCheckBox.Checked == true)
            {
                passwordTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                passwordTextBox.UseSystemPasswordChar = true;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(usernameTextBox.Text) &&
               !string.IsNullOrEmpty(passwordTextBox.Text))
            {

                string mySQL = string.Empty;

                mySQL += "SELECT * FROM LoginTbl ";
                mySQL += "WHERE Username = '" + usernameTextBox.Text + "' ";
                mySQL += "AND Password = '" + passwordTextBox.Text + "'";

                DataTable userData = ServerConnection.executeSQL(mySQL);

                if(userData.Rows.Count > 0)
                {

                    usernameTextBox.Clear();
                    passwordTextBox.Clear();
                    showPasswordCheckBox.Checked = false;

                    this.Hide();

                    MainForm formMain = new MainForm();
                    formMain.ShowDialog();
                    formMain = null;

                    this.Show();
                    this.usernameTextBox.Select();

                }
                else
                {
                    MessageBox.Show("The username or password is incorrect. Try again.",
                        "C# Login Form : iBasskung Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    usernameTextBox.Focus();
                    usernameTextBox.SelectAll();
                }

            }
            else
            {
                MessageBox.Show("Please enter username and password.", "C# Login Form : iBasskung Tutorial",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                usernameTextBox.Select();
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}