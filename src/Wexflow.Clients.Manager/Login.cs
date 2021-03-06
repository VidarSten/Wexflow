﻿using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Wexflow.Core.Service.Client;
using Wexflow.Core.Service.Contracts;

namespace Wexflow.Clients.Manager
{
    public partial class Login : Form
    {
        private static readonly string WexflowWebServiceUri = ConfigurationManager.AppSettings["WexflowWebServiceUri"];

        private readonly WexflowServiceClient _wexflowServiceClient;

        public Login()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            _wexflowServiceClient = new WexflowServiceClient(WexflowWebServiceUri);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Authenticate();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Authenticate();
            }
        }

        private void Authenticate()
        {
            string username = txtUserName.Text;
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Type a username.");
            }
            else
            {
                User user = _wexflowServiceClient.GetUser(username);
                string password = GetMd5(txtPassword.Text);

                if (user == null)
                {
                    MessageBox.Show("The user " + txtUserName.Text + " does not exist.");
                }
                else
                {
                    if (user.Password == password)
                    {
                        Form1 form1 = new Form1();
                        form1.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("The password is incorrect.");
                    }
                }
            }
        }

        public static string GetMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}
