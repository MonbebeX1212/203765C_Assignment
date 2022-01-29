using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace _203765C_Assignment
{
    public partial class Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Request.Cookies["AuthToken"] != null)
            {
                
                
                Message.Text = "You've successfully logged in";
                Message.ForeColor = System.Drawing.Color.Green;
                logoutbtn.Visible = true;

                string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/Images"));
                List<String> images = new List<string>(filesindirectory.Count());

                foreach (string item in filesindirectory)
                {
                    images.Add(String.Format("~/Images/{0}", Path.GetFileName(item)));
                }

                RepeaterImages.DataSource = images;
                RepeaterImages.DataBind();

                //Photo.ImageUrl = "~/" + imgPath;
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

      
        protected void LogoutMe(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if(Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Request.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Request.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Request.Cookies["AuthToken"].Value = string.Empty;
                Request.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

    }
}