using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //lib SQL
using System.Security.Cryptography;
using System.Text.RegularExpressions; //Regex

namespace act2_8
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            PasswordMode();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PasswordMode()
        {
            
            textBox2.Text = "";
           
            textBox2.PasswordChar = '*';
            
            textBox2.MaxLength = 14;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            string usuario = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (isValidPassword(usuario, password))
            {
               
                new Form1().Show();
                this.Hide();
            }
            else
            {
                label4.Visible = true;
                label4.ForeColor = Color.Red;
                label4.Text = "Usuario y/o contraseña incorrecta";
            }

        }

        private bool isValidPassword(string username, string password)
        {
            UserB user = getUserFromDB(username);
            bool isValid = false;

            if (!string.IsNullOrEmpty(user.user))
            {
                byte[] hashedPassword = Encrypt.HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), user.salt);
                if (hashedPassword.SequenceEqual(user.pass))
                {
                    isValid = true;
                    //label3.Text = "correcto";
                }
            }
            return isValid;
        }


        private UserB getUserFromDB(string username)
        {

            UserB user = new UserB();

            string connectionString = @"user id=sa;" +
                @"password=utlaguna1.;
                server=CTEUTLD01\SEGURIDADB;" +
                //   @"Trusted_Connection=yes;" +
                @"database=securityB;" +
                @"connection timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string saltSaved = "select username, salt, password, email from students where username = @username OR email = @username";
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = saltSaved;

                   
                    command.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = username;
                   
                    try
                    {
                        connection.Open();
                        using (SqlDataReader oReader = command.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                user.user = oReader["username"].ToString();
                                user.salt = (byte[])oReader["salt"];
                                user.pass = (byte[])oReader["password"];
                                //MessageBox.Show(user.user);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        label3.Text = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return user;
        }

    }
}
