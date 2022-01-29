using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _203765C_Assignment
{
    public partial class ForgetPassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void proceedbtn_Click(object sender, EventArgs e)
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
                cmd.Parameters.AddWithValue("@0", recoveremail_tb.Text.Trim());
                sda.SelectCommand = cmd;
                sda.Fill(dtbl);

                if (dtbl.Rows.Count == 1)
                {
                    if (newpassword_tb.Text.Trim() == newpassword2_tb.Text.Trim())
                    {
                        DateTime t = DateTime.Parse(dtbl.Rows[0]["ChangepasswordTime"].ToString());
                        var now = DateTime.Now;

                        if (now >= t)
                        {

                            if (dtbl.Rows[0]["Password"].ToString() == newpassword2_tb.Text.Trim() && dtbl.Rows[0]["Password2"].ToString() == newpassword2_tb.Text.Trim())
                            {
                                passworderror.Text = "Password cannot be the same as previous 2 password";
                                passworderror.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                SqlDataAdapter sdapassword = new SqlDataAdapter();
                                string updatepassword = string.Format("Update Register set Password = @0 WHERE Email = @1");
                                SqlCommand cmdpassword = new SqlCommand(updatepassword, conn);
                                cmdpassword.Parameters.AddWithValue("@0", dtbl.Rows[0]["Password2"].ToString());
                                cmdpassword.Parameters.AddWithValue("@1", recoveremail_tb.Text.Trim());
                                sdapassword.SelectCommand = cmdpassword;
                                sdapassword.Fill(dtbl);


                                SqlDataAdapter sdapassword2 = new SqlDataAdapter();
                                string updatepassword2 = string.Format("Update Register set Password2 = @0 WHERE Email = @1");
                                SqlCommand cmdpassword2 = new SqlCommand(updatepassword2, conn);
                                cmdpassword2.Parameters.AddWithValue("@0", newpassword2_tb.Text.Trim());
                                cmdpassword2.Parameters.AddWithValue("@1", recoveremail_tb.Text.Trim());
                                sdapassword2.SelectCommand = cmdpassword2;
                                sdapassword2.Fill(dtbl);

                                var time = DateTime.Now.AddSeconds(60);
                                SqlDataAdapter sdatime = new SqlDataAdapter();
                                string updatetime = string.Format("Update Register set ChangepasswordTime= @0 where email= @1");
                                SqlCommand cmdtime = new SqlCommand(updatetime, conn);
                                cmdtime.Parameters.AddWithValue("@0", time);
                                cmdtime.Parameters.AddWithValue("@1", recoveremail_tb.Text.Trim());
                                sdatime.SelectCommand = cmdtime;
                                sdatime.Fill(dtbl);


                                var Maxtime = DateTime.Now.AddSeconds(300);
                                SqlDataAdapter sdaMaxtime = new SqlDataAdapter();
                                string updateMaxtime = string.Format("Update Register set ChangepasswordMaxTime= @0 where email= @1");
                                SqlCommand cmdMaxtime = new SqlCommand(updateMaxtime, conn);
                                cmdMaxtime.Parameters.AddWithValue("@0", Maxtime);
                                cmdMaxtime.Parameters.AddWithValue("@1", recoveremail_tb.Text.Trim());
                                sdaMaxtime.SelectCommand = cmdMaxtime;
                                sdaMaxtime.Fill(dtbl);


                                Response.Redirect("Login.aspx", false);
                            }
                        }
                        else
                        {
                            passworderror.Text = "Password change too frequently, it can only be change within 1 minute from the last change of password";
                            passworderror.ForeColor = System.Drawing.Color.Red;
                        }

                    }
                    else
                    {
                        passworderror.Text = "Password is not the same";
                     
                        passworderror.ForeColor = System.Drawing.Color.Red;
                    }

                        



                }
                else
                {
                    recovererror.Text = " **No email found!";
                    recovererror.ForeColor = System.Drawing.Color.Red;

                }


            }

            //SqlConnection conn = new SqlConnection(MYDBConnectionString);
            //string query = string.Format("SELECT * from Register WHERE Email = '{0}'", recoveremail_tb.Text.Trim());
            //SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            //DataTable dtbl = new DataTable();
            //sda.Fill(dtbl);

            //if (dtbl.Rows.Count == 1)
            //{
            //    if(newpassword_tb.Text.Trim() == newpassword2_tb.Text.Trim())
            //    {
            //        if (dtbl.Rows[0]["Password"].ToString() == newpassword2_tb.Text.Trim() && dtbl.Rows[0]["Password2"].ToString() == newpassword2_tb.Text.Trim())
            //        {
            //            passworderror.Text = "Password cannot be the same as previous password";
            //            passworderror.ForeColor = System.Drawing.Color.Red;
            //        }
            //        else
            //        {
            //            string updatepassword = string.Format("Update Register set Password = '{0}' WHERE Email = '{1}'", dtbl.Rows[0]["Password2"].ToString(), recoveremail_tb.Text.Trim());
            //            SqlDataAdapter sdapassword = new SqlDataAdapter(updatepassword, conn);
            //            DataTable dtblpassword = new DataTable();
            //            sdapassword.Fill(dtblpassword);

            //            string updatepassword2 = string.Format("Update Register set Password2 = '{0}' WHERE Email = '{1}'", newpassword2_tb.Text.Trim(), recoveremail_tb.Text.Trim());
            //            SqlDataAdapter sdapassword2 = new SqlDataAdapter(updatepassword, conn);
            //            DataTable dtblpassword2 = new DataTable();
            //            sdapassword2.Fill(dtblpassword2);

            //            Response.Redirect("Resetsuccess.aspx", false);
            //        }

            //    }
            //    else
            //    {
            //        passworderror.Text = "Password must be the same!";
            //        passworderror.ForeColor = System.Drawing.Color.Red;
            //    }
            //}
            //else
            //{
            //    recovererror.Text = " **No email found!";
            //    recovererror.ForeColor = System.Drawing.Color.Red;
            //}

        }
    }
}