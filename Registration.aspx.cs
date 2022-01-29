using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace _203765C_Assignment
{
    public partial class Registration : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int checkpassword(string password)
        {
            int score = 0;


            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if ((Regex.IsMatch(password, "[0-9]")))
            {
                score++;
            }

            if ((Regex.IsMatch(password, "[A-Z]")))
            {
                score++;
            }


            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }

            return score;
        }

        protected void checkbtn_Click(object sender, EventArgs e)
        {
            


            int scores = checkpassword(pw_tb.Text);
            string status = "check";
            switch (scores)
            {
                case 1:
                    pwchecker.Text = "Very Weak";
         
                    break;
                case 2:
                    pwchecker.Text = "Weak";
                    break;
                case 3:
                    pwchecker.Text = "Medium";
                    break;
                case 4:
                    pwchecker.Text = "Strong";
                    break;
                case 5:
                    pwchecker.Text = "Very Strong";
                    break;
                default:
                    break;
            }
            checkbtn.Text = status;
            if (scores < 4)
            {
                pwchecker.ForeColor = Color.Red;
                return;
            }
            pwchecker.ForeColor = Color.Green;
        }

        protected void submitbtn_Click(object sender, EventArgs e)
        {


            if (first_tb.Text.Length == 0)
            {
                namechecker.Text = "Required field";
                namechecker.ForeColor = Color.Red;
            }
            

            if (last_tb.Text.Length == 0)
            {
                lastchecker.Text = "Required field";
                lastchecker.ForeColor = Color.Red;
            }

            //if(email_tb.Text.Length == 0)
            //{
            //    emailchecker.Text = "Required field";
            //    emailchecker.ForeColor= Color.Red;


            //    SqlConnection conn = new SqlConnection(MYDBConnectionString);
            //    string query = "Select * from Register Where Email = '" + email_tb.Text.Trim() + "'";

            //    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            //    DataTable dtbl = new DataTable();
            //    sda.Fill(dtbl);

            //    if (dtbl.Rows.Count == 1)
            //    {


            //        emailchecker.Text = "Email has been registered";
            //        emailchecker.ForeColor = Color.Red;



            //    }

            //}

            if (pw_tb.Text.Length == 0)
            {
                pwchecker.Text = "Required field";
                pwchecker.ForeColor = Color.Red;
            }

            if (credit_tb.Text.Length == 0)
            {
                creditchecker.Text = "Required field";
                creditchecker.ForeColor = Color.Red;
            }

            if (date_tb.Text.Length == 0)
            {
                datechecker.Text = "Required field";
                datechecker.ForeColor = Color.Red;
            }

            //SqlConnection conn = new SqlConnection(MYDBConnectionString);
            //string query = "Select * from Register Where Email = '" + email_tb.Text.Trim() + "'";

            //SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            //DataTable dtbl = new DataTable();
            //sda.Fill(dtbl);

            //if (dtbl.Rows.Count == 1)
            //{


            //    emailchecker.Text = "Email has been registered";
            //    emailchecker.ForeColor = Color.Red;



            //}

            else
            {
                namechecker.Text = " ";
                lastchecker.Text = " ";
                creditchecker.Text = " ";
                datechecker.Text = " ";
                emailchecker.Text = " ";
                pwchecker.Text = " ";

                //string pwd = get value from your Textbox
                string pwd = pw_tb.Text.ToString().Trim();

                //Generate random "salt"
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                finalHash = Convert.ToBase64String(hashWithSalt);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;


                createregister();
                startUpload();
               
                Response.Redirect("Login.aspx", false);
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }


        protected void createregister()
        {
            //string path = Path.GetFileName(photoUpload.FileName);
            //path = path.Replace(" ", "");
            //String photoupload = path;
            //photoUpload.SaveAs(Server.MapPath("~/Image/") + path);



            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Register VALUES(@Firstname,@Lastname, @Creditcard, @Email, @EmailVerified, @Password, @PasswordHash, @PasswordSalt, @Password2, @DOB, @IV, @Key, @AccountStatus, @Attempt, @LockTime, @ChangepasswordTime, @ChangepasswordMaxTime)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Firstname", first_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@Lastname", last_tb.Text.Trim());
                            //cmd.Parameters.AddWithValue("@Creditcard", credit_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", pw_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@Password2", pw_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@DOB", date_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", email_tb.Text.Trim());
                            cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Creditcard", Convert.ToBase64String(encryptData(credit_tb.Text.Trim())));
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@AccountStatus", "Active");
                            cmd.Parameters.AddWithValue("@Attempt", 3);
                            cmd.Parameters.AddWithValue("@LockTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ChangepasswordTime", DateTime.Now);
                            var changepw = DateTime.Now.AddMinutes(5);
                            cmd.Parameters.AddWithValue("@ChangepasswordMaxTime", changepw);


                            cmd.Connection = con;

                            try
                            {
                               
                                con.Open();
                                cmd.ExecuteNonQuery();

                            }
                            catch(Exception ex)
                            {
                                //err msg
                            }
                            finally
                            {
                                con.Close();

                            }
                            
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        protected void startUpload()
        {
            string imgName = photoUpload.FileName;

            string imgPath = "Images/" + imgName;

            int imgSize = photoUpload.PostedFile.ContentLength;

            if (photoUpload.PostedFile != null && photoUpload.PostedFile.FileName != " ")
            {
                photoUpload.SaveAs(Server.MapPath(imgPath));
                //photoUpload.SaveAs(Server.MapPath("/Images/") + imgName);
                //ImgUpload.SaveAs(Server.MapPath("/Images/") + path);
                //Photo.ImageUrl = "~/" + imgPath;
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Image saved!')", true);
            }

        }

      
    }
}