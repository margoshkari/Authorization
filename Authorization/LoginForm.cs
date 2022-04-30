using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Authorization
{
    public partial class LoginForm : Form
    {
        private string pass = string.Empty;
        string resp = string.Empty;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                pass = ComputeSha256Hash(this.passwordTB.Text);
                SendRequestAsync();
            }
        }
        private bool Check()
        {
            if (this.loginTB.Text == String.Empty || this.passwordTB.Text == String.Empty)
            {
                MessageBox.Show("Please fill all fields!");
            }
            else
            {
                return true;
            }
            return false;
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
        async private void SendRequestAsync()
        {
            WebRequest request = WebRequest.Create($"http://botserver-001-site1.itempurl.com/Login?login={this.loginTB.Text}&password={pass}");
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
    }
}
