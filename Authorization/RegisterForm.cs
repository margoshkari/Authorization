using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Authorization
{
    public partial class RegisterForm : Form
    {
        private string pass = string.Empty;
        string resp = string.Empty;
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            if(Check())
            {
                pass = ComputeSha256Hash(this.passwordTB.Text);
                SendRequestAsync();
            }
        }
        async private void SendRequestAsync()
        {
            WebRequest request = WebRequest.Create($"http://botserver-001-site1.itempurl.com/Register?login={this.loginTB.Text}&email={this.emailTB.Text}&password={pass}");
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    resp = reader.ReadToEnd();
                    MessageBox.Show(resp);
                }
            }
            //margoshka.ri6@gmail.com
            response.Close();
        }
        private bool Check()
        {
            if (this.loginTB.Text == String.Empty || this.emailTB.Text == String.Empty || this.passwordTB.Text == String.Empty)
            {
                MessageBox.Show("Please fill all fields!");
            }
            else if (this.loginTB.Text.Length < 5)
            {
                MessageBox.Show("Login must be more than 5 characters");
            }
            else if (this.passwordTB.Text.Length < 8)
            {
                MessageBox.Show("Password must be more than 8 characters");
            }
            else if (!IsValidEmail(this.emailTB.Text))
            {
                MessageBox.Show("Email is not valid!");
            }
            else
            {
                return true;
            }
            return false;
        }
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            loginForm.FormClosed += LoginForm_FormClosed;
        }

        private void LoginForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);    
        }
    }
}
