using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //Tiemout
using System.Windows.Forms;
using System.Data.SqlClient; //lib SQL
using System.Security.Cryptography; //Encrypt
using System.Text.RegularExpressions; //Regex
using System.Net.Mail; //Email




namespace act2_8
{
    
    public partial class Form1 : Form
    {

        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        byte[] plaintext;
        byte[] encryptedtext;
        string emailpattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                           + "@"
                           + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        string lettersPatern = @"/^[a-zA-Z\s]*$";



        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        static public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }

                // MessageBox.Show("entra");
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        // RFC function

        public string CalculteRFC(string textData, int spacesToCut)
        {
            int substringHelper = spacesToCut;
            int countspace = 0;
            string txt = textData;
            int lastSpace = txt.LastIndexOf(' ');
            char[] arTxt = txt.ToArray();

            for (var i = 0; i < txt.Length; i++)
            {
                if (txt[i] == ' ')
                {
                    countspace++;

                }
            }

            // Retrieve rfc data considenring last space input
            if (countspace >= 1 && txt.Trim().Length > 0)
            {
                
                string rfc = txt.Substring(lastSpace+1, substringHelper);
                return rfc.ToUpper();
            }
            else
            {
                string rfc = txt.Substring(0, substringHelper);
                return rfc.ToUpper();
            }
        }


        // Calculate age
        static string CalculateYourAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
                DateTime PastYearDate = Dob.AddYears(Years);
                int Months = 0;
                for (int i = 1; i <= 12; i++)
                {
                    if (PastYearDate.AddMonths(i) == Now)
                    {
                        Months = i;
                        break;
                    }
                    else if (PastYearDate.AddMonths(i) >= Now)
                    {
                        Months = i - 1;
                        break;
                    }
                }
                int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
                int Hours = Now.Subtract(PastYearDate).Hours;
                int Minutes = Now.Subtract(PastYearDate).Minutes;
                int Seconds = Now.Subtract(PastYearDate).Seconds;
                return String.Format("{0}", Years);
            
        }

        // Empty textboxs
        private void EmptyForm()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox12.Text = "";
            textBox0.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            textBox4.Text = "";
            textBox5.Text = "RFC data will be displayed here";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox3.Text = "";
        }
        
        //Display RFC
        public void GetRFC()
        {
            string rfcRgx = @"[A-Z0-9]{13}";
            Regex rx = new Regex(rfcRgx);

            //homoclave
            Random homoclave = new Random();
            string h1 = homoclave.Next(0, 9).ToString();
            string h2 = homoclave.Next(0, 9).ToString();
            string h3 = homoclave.Next(0, 9).ToString();

            string nom = textBox1.Text.Trim();
            string fln = textBox2.Text.Trim();
            string sln = textBox12.Text.Trim();
            string dob = dateTimePicker1.Value.Date.ToString("dd").Trim();
            string mob = dateTimePicker1.Value.Date.ToString("MM").Trim();
            string yob = dateTimePicker1.Value.Date.ToString("yy").Trim();
            string RFC = textBox5.Text.Trim();

            if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(fln) && !string.IsNullOrEmpty(sln) && !string.IsNullOrEmpty(dob)
                && !string.IsNullOrEmpty(mob) && !string.IsNullOrEmpty(yob))
            {
                if (nom.Length == 1 || fln.Length == 1 || sln.Length == 1)
                {
                    label18.Text = "RFC was not created";
                    label18.Visible = true;

                }
                else
                {
                    string automaticRFC = CalculteRFC(fln, 2) + CalculteRFC(sln, 1) + nom.Substring(0, 1).ToUpper() + CalculteRFC(yob, 2) +
                                      CalculteRFC(mob, 2) + CalculteRFC(dob, 2) + h1 + h2 + h3;

                    if (rx.IsMatch(automaticRFC))
                    {

                        label18.Visible = false;
                        textBox5.Text = automaticRFC; //ABCD99002800 
                        //MessageBox.Show(automaticRFC);
                    }
                    else
                    {
                        label18.Text = "*RFC was not created";
                        //MessageBox.Show(automaticRFC);
                        label18.Visible = true;
                    }

                }

            }
            else
            {

                if (string.IsNullOrEmpty(nom))
                {
                    label11.Visible = true;
                }

                if (string.IsNullOrEmpty(fln))
                {
                    label12.Visible = true;
                }

                if (string.IsNullOrEmpty(sln))
                {
                    label13.Visible = true;
                }
            }
        }

 

        public Form1()
        {
            InitializeComponent();
            this.ActiveControl = textBox0;

        }

        public void SetMyCustomFormat()
        {
            // Set the Format type and the CustomFormat string.
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            ERROR.Visible = false;
            label8.Visible = false;
            SetMyCustomFormat();

           
         }

      

        // Only text - input validation
        private void validarTexto( KeyPressEventArgs e)
        {

            if (!char.IsLetter(e.KeyChar) && !(e.KeyChar == (char)Keys.Left) && !(e.KeyChar == (char)Keys.Right) && !(e.KeyChar == (char)Keys.Space) && !(e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }


        // Only numbers - input validation
        private void validarNumero(KeyPressEventArgs e, object sender)

        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        // Only alphanumeric values

        private void validarAlfanumerico(KeyPressEventArgs e, object sender)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar)
                 && !(e.KeyChar == (char)Keys.Left) && !(e.KeyChar == (char)Keys.Right))
            {
                e.Handled = true;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            string conn = @"user id=sa;" + @"password=utlaguna1.; " +
                @"server = CTEUTLD01\SEGURIDADB;"
                + @"database = securityB;"
                + @"connection timeout=30";


            SqlConnection myConnection = new SqlConnection(conn);
            
            //Validate conection
            bool SQLconn = false;


            //Conn to DB
            try
            {
                myConnection.Open(); //Opens con to DB
                SQLconn = true;
            }
            catch (SqlException sqlEx)
            {
                ERROR.Visible = true;
                label8.Visible = true;
                label8.Text = "Error 1420: Connection failed.";
                SQLconn = false;
            }


            //New Query command
            string query1;
            query1 = "INSERT INTO students VALUES (@enrol, @name, @first_last_name, " +
                        "@second_last_name, @birthday, @age, @irsn,@username ,@password, @salt, @email);";
            
            SqlCommand cmd1 = new SqlCommand(query1, myConnection);

            // ID
            string id_student = textBox0.Text.Trim();
            bool idValid = false;
            if(id_student.Length > 0 )
            {
                if (textBox0.Text.Length == 5)
                {
                    cmd1.Parameters.Add("@enrol", SqlDbType.VarChar).Value = Convert.ToInt32(id_student);
                    idValid = true;
                   
                }
                else
                {
                    label14.Text = "ID must include 5 digits";
                    label14.Visible = true;
                    idValid = false;
                }
               
            }
            else
            {
                idValid = false;
                label14.Visible = true;
            }


            // Name
            Regex validText = new Regex(lettersPatern);
            string alumn_name = textBox1.Text.Trim();
            bool nameValid = false;
            if (alumn_name.Length > 0)
            {
                cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = alumn_name;
                nameValid = true;

            }
            else
            {
                nameValid = false;
                label11.Visible = true;
            }
            

            // First Last name 
            string alumn_lastName = textBox2.Text.Trim();
            bool flnValid = false;
            if (alumn_lastName.Length > 0)
            {
                cmd1.Parameters.Add("@first_last_name", SqlDbType.VarChar).Value = alumn_lastName;
                flnValid = true;
            }
            else
            {
                flnValid = false;
                label12.Visible = true;
            }
            

            // Second Last name
            string lastName2 = textBox12.Text.Trim();
            bool slnValid = false;
            if (lastName2.Length > 0)
            {
                cmd1.Parameters.Add("@second_last_name", SqlDbType.VarChar).Value = lastName2;
                slnValid = true;
            }
            else
            {
                slnValid = false;
                label13.Visible = true;
            }
           
            // Birthdat
            string alumn_birth = dateTimePicker1.Value.Date.ToString("yyyy/MM/dd").Trim();
            bool dobValid = false;
            if (alumn_birth.Length > 0)
            {
                cmd1.Parameters.Add("@birthday", SqlDbType.Date).Value = alumn_birth;
                dobValid = true;
            }
            else
            {
                dobValid = false;
                label21.Visible = true;
            }

            // Age 
            string alumn_age = CalculateYourAge(dateTimePicker1.Value.Date);
            bool ageValid = false;
            if (textBox4.Text.Length >0)
            {
                // Age Restriction
                if (Convert.ToInt32(CalculateYourAge(dateTimePicker1.Value.Date)) >= 18)
                {
                   
                    cmd1.Parameters.Add("@age", SqlDbType.Int).Value = Convert.ToInt32(alumn_age); //to int
                    ageValid = true;
                }
                else
                {
                    MessageBox.Show("Student must be at least 18 years old");
                    label17.Text = "*User is underage";
                    label17.Visible = true;
                }
            }
            else
            {
                ageValid = false;
                label17.Visible = true;
            }
            

            // RFC - IRSN
            string irsn = textBox5.Text.Trim();
            bool irsnValid = false;
            if (irsn.Length < 0 || irsn == "RFC data will be displayed here")
            {
                irsnValid = false;
                label18.Visible = true;
            }
            else
            {
                
                cmd1.Parameters.Add("@irsn", SqlDbType.Char).Value = irsn;
                irsnValid = true;
            }
           

            // Username
            string user = textBox6.Text.Trim();
            bool userValid = false;
            if (user.Length > 0)
            {
                cmd1.Parameters.Add("@username", SqlDbType.NChar).Value = user;
                userValid = true;
            }
            else
            {
                label15.Location = new Point(923, 218);
                label15.Text = "*This field is empty";
                label15.ForeColor = Color.Red;
                label15.Visible = true;
                userValid = false;
            }
            

            // Salt
            byte[] salt = Encrypt.GenerateSalt();
            cmd1.Parameters.Add("@salt", SqlDbType.VarBinary).Value = salt;

            // Hashed password
            string pass = textBox7.Text.Trim();
            bool pasValid = false;
            if (pass.Length > 0)
            {
                cmd1.Parameters.Add("@password", SqlDbType.VarBinary).Value = Encrypt.HashPasswordWithSalt(Encoding.UTF8.GetBytes(pass), salt);
                pasValid = true;
            }
            else
            {
                label16.Visible = true;
                pasValid = false;
            }
            


            // Email
            string email_stdn = textBox3.Text.Trim();
            bool emailValid = false;
            Regex rgxEmail = new Regex(emailpattern);
            if (email_stdn.Length > 0 && rgxEmail.IsMatch(email_stdn))
            {
                cmd1.Parameters.Add("@email", SqlDbType.VarChar).Value = email_stdn;
                emailValid = true;
            }
            else
            {
                label20.Text = "Insert a valid email address";
                label20.Visible = true;
                emailValid = false;
            }

            


            //  Executing Query
             
                //Data validation
                if(idValid == true && nameValid == true && flnValid == true && slnValid == true && dobValid == true 
                    && ageValid == true && irsnValid == true && userValid == true && emailValid == true && pasValid == true
                    && SQLconn == true)
                {

                    bool newElement = false;

                   try
                    {
                        cmd1.ExecuteNonQuery();
                        ERROR.ForeColor = Color.Green;
                        ERROR.Text = "Nuevo Alumno registrado correctamente!";
                        ERROR.Visible = true;
                        newElement = true;
                        
                        myConnection.Close();

                        
                    }
                    catch (SqlException err)
                    {
                        ERROR.Visible = true;
                        ERROR.Text = "Error 1919: Query failed to execute (" + err + ")";
                        myConnection.Close();
                        newElement = false;
                        return;
                    }
                    finally
                    {
                        myConnection.Close();
                        ERROR.ForeColor = Color.Red;
                        EmptyForm();
                        label11.Visible = false;
                        label12.Visible = false;
                        label13.Visible = false;
                        

                    }

             
                    

                }
                else
                {
                    if(SQLconn == false)
                    {
                        ERROR.Text = "Error 999: Please contact an administrator.";
                    }
                    else
                    {
                        ERROR.Text = "Error 407: Information is missing!";
                        ERROR.Visible = true;
                    }
                           
                }
               
        }
    
        // Name Keypress
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            label11.Visible = false;
            validarTexto(e);
            ERROR.Visible = false;
        }

        //Last name 1 keypress
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

            label12.Visible = false;
            validarTexto(e);
            ERROR.Visible = false;

        }

        // Age KP
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            validarNumero(e, sender);
            ERROR.Visible = false;
        }

        // Deprecated
        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            label14.Visible = false;
            validarNumero(e, sender);
            ERROR.Visible = false;
        }

        // Deprecated
        private void button2_Click(object sender, EventArgs e)
        {
           

        }

        //Decrypt example
     
        //Encrypted example
        private void button4_Click(object sender, EventArgs e)
        {
          // textBox10.Text = ByteConverter.GetString(Encrypt.GetenerateSalt());

            byte[] pas = Encoding.UTF8.GetBytes(textBox7.Text);
            byte[] salt = Encrypt.GenerateSalt();

        }

        //Student ID TC
        private void textBox0_TextChanged(object sender, EventArgs e)
        {
           
        }

        // First last name TC
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 2 && textBox12.Text.Length > 1)
            {
                GetRFC();
            }
        }

        
            // Name Leave event
        private void textBox1_Leave(object sender, EventArgs e)
        {
            

        }

        // Last name 1 leave event
        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

        // 2nd Last Name
        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            label13.Visible = false;
            validarTexto(e);
           
        }

        // Last Name 2 - Retrieve RFC data
        private void textBox12_Leave(object sender, EventArgs e)
        {

        }

        // Name TC
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           if(textBox1.Text.Length >0 && textBox2.Text.Length>2 && textBox12.Text.Length > 1)
            {
                GetRFC();
            }
        }

        //Name Enter
        private void textBox1_Enter(object sender, EventArgs e)
        {

        }

        //NAme KD
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        // Student ID Leave
        private void textBox0_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox0.Text))
            {


                if (textBox0.Text.Length < 5)
                {
                    label14.Text = "ID must include 5 digits";
                    label14.Visible = true;
                }
            }
        }

        // DTP leave event - 
        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            
        }

        // Get Age btn
        private void button5_Click(object sender, EventArgs e)
        {
            label17.Visible = false;
            textBox4.Text = CalculateYourAge(dateTimePicker1.Value.Date).ToString();
        }

        // Get RFC btn
        private void button6_Click(object sender, EventArgs e)
        {

            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label14.Visible = false;
            ERROR.Visible = false;
            Random randomID = new Random();
            textBox0.Text = randomID.Next(19000, 21999).ToString();
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            label15.Location = new Point(837, 218);
            label15.Text = "Username must include numbers and letters";
            label15.ForeColor = Color.Black;
            label15.Visible = true;
            validarAlfanumerico(e,sender);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {

        }


        private void textBox3_Leave(object sender, EventArgs e)
        {
            string email = textBox3.Text;

            string patter = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                            + "@" 
                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            Regex reg = new Regex(patter);

            if (!reg.IsMatch(email))
            {
                label20.Text = "Insert a valid email address";
                label20.Visible = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            label16.Visible = false;
            validarAlfanumerico(e, sender);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            label20.Visible = false;
        }

        private void Close(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox4.Text = CalculateYourAge(dateTimePicker1.Value.Date).ToString();
            GetRFC();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 2 && textBox12.Text.Length > 1)
            {
                GetRFC();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            EmptyForm();
        }
    }
}
