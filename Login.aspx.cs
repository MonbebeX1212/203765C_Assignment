using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data;
using System.Configuration;

namespace _203765C_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        


        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6Lc8AkUeAAAAAO8bdi2aqFoWmaZShLiGI8YtYRnO &response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        ////To show the JSON response string for learning purpose
                        //Captscore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

       
        protected void LoginMe(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {


                DataTable dtbl = new DataTable();

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MYDBConnection"].ToString());

                using (conn)
                {
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    string query = string.Format("SELECT * from Register WHERE Email = @0");
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtbl);

                    //valid email
                    if (dtbl.Rows.Count == 1)
                    {
                        if (dtbl.Rows[0]["AccountStatus"].ToString() != "Active")
                        {
                            DateTime t = DateTime.Parse(dtbl.Rows[0]["LockTime"].ToString());
                            var now = DateTime.Now;

                            if (now >= t)
                            {

                                SqlDataAdapter sdastatus = new SqlDataAdapter();
                                string updatestatus = string.Format("Update Register set AccountStatus= 'Active' where email= @0");
                                SqlCommand cmdstatus = new SqlCommand(updatestatus, conn);
                                cmdstatus.CommandType = CommandType.Text;
                                cmdstatus.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                                sdastatus.SelectCommand = cmdstatus;
                                sdastatus.Fill(dtbl);

                                SqlDataAdapter sdaattempt = new SqlDataAdapter();
                                string updateattempt = string.Format("Update Register set Attempt= '3' where email= @0");
                                SqlCommand cmdattempt = new SqlCommand(updateattempt, conn);
                                cmdattempt.CommandType = CommandType.Text;
                                cmdattempt.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                                sdaattempt.SelectCommand = cmdattempt;
                                sdaattempt.Fill(dtbl);

                            }
                            else
                            {
                                lockmsg.Text = "Your Account is disabled, please contact the administrators.";
                                lockmsg.ForeColor = System.Drawing.Color.Red;

                            }

                        }
                        else
                        {

                            if (dtbl.Rows[0]["Password2"].ToString() == loginpw_tb.Text.Trim())
                            {
                                Session["LoggedIn"] = loginemail_tb.Text.Trim();

                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                if ((int)dtbl.Rows[0]["Attempt"] != 3)
                                {

                                    SqlDataAdapter sdaattempt = new SqlDataAdapter();
                                    string updateattempt = string.Format("Update Register set Attempt= '3' where email= @0");
                                    SqlCommand cmdattempt = new SqlCommand(updateattempt, conn);
                                    cmdattempt.CommandType = CommandType.Text;
                                    cmdattempt.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                                    sdaattempt.SelectCommand = cmdattempt;
                                    sdaattempt.Fill(dtbl);

                                }

                                var Maxnow = DateTime.Now;
                                DateTime Maxt = DateTime.Parse(dtbl.Rows[0]["ChangepasswordMaxtime"].ToString());

                                if (Maxnow >= Maxt)
                                {
                                    Response.Redirect("ForgetPassword.aspx", false);
                                }
                                else
                                {
                                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                    Response.Redirect("Homepage.aspx", false);
                                }

                            }
                            else
                            {
                                error.Text = "Wrong email or password ";
                                error.ForeColor = System.Drawing.Color.Red;

                                int failure = (int)dtbl.Rows[0]["Attempt"];
                                failure--;

                                SqlDataAdapter sdaattempt = new SqlDataAdapter();
                                string updateattempt = string.Format("Update Register set Attempt= '" + failure + "' where email= @0");
                                SqlCommand cmdattempt = new SqlCommand(updateattempt, conn);
                                cmdattempt.CommandType = CommandType.Text;
                                cmdattempt.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                                sdaattempt.SelectCommand = cmdattempt;
                                sdaattempt.Fill(dtbl);


                                if (failure <= 0)
                                {


                                    SqlDataAdapter sdastatus = new SqlDataAdapter();
                                    string updatestatus = string.Format("Update Register set AccountStatus= 'Disabled' where email= @0");
                                    SqlCommand cmdstatus = new SqlCommand(updatestatus, conn);
                                    cmdstatus.CommandType = CommandType.Text;
                                    cmdstatus.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                                    sdastatus.SelectCommand = cmdstatus;
                                    sdastatus.Fill(dtbl);


                                    var time = DateTime.Now.AddSeconds(60);
                                    SqlDataAdapter sdatime = new SqlDataAdapter();
                                    string updatetime = string.Format("Update Register set LockTime= @0 where email= @1");
                                    SqlCommand cmdtime = new SqlCommand(updatetime, conn);

                                    cmdtime.Parameters.AddWithValue("@0", time);
                                    cmdtime.Parameters.AddWithValue("@1", loginemail_tb.Text.Trim());
                                    sdatime.SelectCommand = cmdtime;
                                    sdatime.Fill(dtbl);

                                    error.Text = "Maximum login attempt has reached, please try again later";
                                    error.ForeColor = System.Drawing.Color.Red;
                                    lockmsg.Text = "Your Account is disabled, please contact the administrators.";
                                    lockmsg.ForeColor = System.Drawing.Color.Red;

                                }

                            }

                        }

                    }
                    else
                    {
                        error.Text = "Account cannot be found";
                        error.ForeColor = System.Drawing.Color.Red;
                    }

                }
            }
            else
            {

            }


                //if (ValidateCaptcha())
                //{
                //    DataTable dtbl = new DataTable();

                //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MYDBConnection"].ToString());

                //    using (conn)
                //    {
                //        conn.Open();
                //        SqlDataAdapter sda = new SqlDataAdapter();
                //        string query = string.Format("SELECT * from Register WHERE Email = @0");
                //        SqlCommand cmd = new SqlCommand(query, conn);
                //        cmd.CommandType = CommandType.Text;
                //        cmd.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                //        sda.SelectCommand = cmd;
                //        sda.Fill(dtbl);

                //        //valid email
                //        if (dtbl.Rows.Count == 1)
                //        {
                //            if (dtbl.Rows[0]["AccountStatus"].ToString() != "Active")
                //            {
                //                DateTime t = DateTime.Parse(dtbl.Rows[0]["LockTime"].ToString());
                //                var now = DateTime.Now;

                //                if (now >= t)
                //                {

                //                    SqlDataAdapter sdastatus = new SqlDataAdapter();
                //                    string updatestatus = string.Format("Update Register set AccountStatus= 'Active' where email= @0");
                //                    SqlCommand cmdstatus = new SqlCommand(updatestatus, conn);
                //                    cmdstatus.CommandType = CommandType.Text;
                //                    cmdstatus.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                //                    sdastatus.SelectCommand = cmdstatus;
                //                    sdastatus.Fill(dtbl);

                //                    SqlDataAdapter sdaattempt = new SqlDataAdapter();
                //                    string updateattempt = string.Format("Update Register set Attempt= '3' where email= @0");
                //                    SqlCommand cmdattempt = new SqlCommand(updateattempt, conn);
                //                    cmdattempt.CommandType = CommandType.Text;
                //                    cmdattempt.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                //                    sdaattempt.SelectCommand = cmdattempt;
                //                    sdaattempt.Fill(dtbl);

                //                }
                //                else
                //                {
                //                    lockmsg.Text = "Your Account is disabled, please contact the administrators.";
                //                    lockmsg.ForeColor = System.Drawing.Color.Red;

                //                }

                //            }
                //            else
                //            {

                //                if (dtbl.Rows[0]["Password2"].ToString() == loginpw_tb.Text.Trim())
                //                {
                //                    Session["LoggedIn"] = loginemail_tb.Text.Trim();

                //                    string guid = Guid.NewGuid().ToString();
                //                    Session["AuthToken"] = guid;

                //                    if ((int)dtbl.Rows[0]["Attempt"] != 3)
                //                    {

                //                        SqlDataAdapter sdaattempt = new SqlDataAdapter();
                //                        string updateattempt = string.Format("Update Register set Attempt= '3' where email= @0");
                //                        SqlCommand cmdattempt = new SqlCommand(updateattempt, conn);
                //                        cmdattempt.CommandType = CommandType.Text;
                //                        cmdattempt.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                //                        sdaattempt.SelectCommand = cmdattempt;
                //                        sdaattempt.Fill(dtbl);

                //                    }

                //                    var Maxnow = DateTime.Now;
                //                    DateTime Maxt = DateTime.Parse(dtbl.Rows[0]["ChangepasswordMaxtime"].ToString());

                //                    if (Maxnow >= Maxt)
                //                    {
                //                        Response.Redirect("ForgetPassword.aspx", false);
                //                    }
                //                    else
                //                    {
                //                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                //                        Response.Redirect("Homepage.aspx", false);
                //                    }

                //                }
                //                else
                //                {
                //                    error.Text = "Wrong email or password ";
                //                    error.ForeColor = System.Drawing.Color.Red;

                //                    int failure = (int)dtbl.Rows[0]["Attempt"];
                //                    failure--;

                //                    SqlDataAdapter sdaattempt = new SqlDataAdapter();
                //                    string updateattempt = string.Format("Update Register set Attempt= '" + failure + "' where email= @0");
                //                    SqlCommand cmdattempt = new SqlCommand(updateattempt, conn);
                //                    cmdattempt.CommandType = CommandType.Text;
                //                    cmdattempt.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                //                    sdaattempt.SelectCommand = cmdattempt;
                //                    sdaattempt.Fill(dtbl);


                //                    if (failure <= 0)
                //                    {


                //                        SqlDataAdapter sdastatus = new SqlDataAdapter();
                //                        string updatestatus = string.Format("Update Register set AccountStatus= 'Disabled' where email= @0");
                //                        SqlCommand cmdstatus = new SqlCommand(updatestatus, conn);
                //                        cmdstatus.CommandType = CommandType.Text;
                //                        cmdstatus.Parameters.AddWithValue("@0", loginemail_tb.Text.Trim());
                //                        sdastatus.SelectCommand = cmdstatus;
                //                        sdastatus.Fill(dtbl);


                //                        var time = DateTime.Now.AddSeconds(60);
                //                        SqlDataAdapter sdatime = new SqlDataAdapter();
                //                        string updatetime = string.Format("Update Register set LockTime= @0 where email= @1");
                //                        SqlCommand cmdtime = new SqlCommand(updatetime, conn);

                //                        cmdtime.Parameters.AddWithValue("@0", time);
                //                        cmdtime.Parameters.AddWithValue("@1", loginemail_tb.Text.Trim());
                //                        sdatime.SelectCommand = cmdtime;
                //                        sdatime.Fill(dtbl);

                //                        error.Text = "Maximum login attempt has reached, please try again later";
                //                        error.ForeColor = System.Drawing.Color.Red;
                //                        lockmsg.Text = "Your Account is disabled, please contact the administrators.";
                //                        lockmsg.ForeColor = System.Drawing.Color.Red;

                //                    }

                //                }

                //            }

                //        }
                //        else
                //        {
                //            error.Text = "Account cannot be found";
                //            error.ForeColor = System.Drawing.Color.Red;
                //        }

                //    }
                //}
                //else
                //{
                //    error.Text = "Fail to authenticate";
                //    error.ForeColor = System.Drawing.Color.Red;
                //}


            }

            protected void forgotbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgetPassword.aspx", false);
        }
    }
}






//if (ValidateCaptcha())
//{
//    int failure = 1;

//    if (loginemail_tb.Text.Trim() == "@" && loginpw_tb.Text.Trim() == "p")
//    {
//        Session["LoggedIn"] = loginemail_tb.Text.Trim();

//        string guid = Guid.NewGuid().ToString();
//        Session["AuthToken"] = guid;

//        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

//        Response.Redirect("Homepage.aspx", false);
//    }
//    else
//    {
//        failure += 1;
//        error.Text = "Wrong email or password";
//        if(failure == 3){
//            error.Text = "Maximum login attempt has reached, please try again later";
//        }
//    }
//}
//else
//{
//    

//}