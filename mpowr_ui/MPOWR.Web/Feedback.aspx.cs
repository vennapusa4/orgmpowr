using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.IO;
namespace MPOWR.Web.components.feedback
{
    public partial class Feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("ravindrareddy422@gmail.com");
                mail.To.Add(txtTo.Text);
                mail.Subject = txtSubject.Text
                    ;
                mail.Body = txtBody.
                    Text;
                mail.IsBodyHtml = true;

                if (fuAttachment.HasFile)
                {
                    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                    mail.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                }
                //    mail.Attachments.Add(new Attachment("C:\\Users\\raveendra.v\\Desktop\\1.jpg"));
                bool enableSSL = true;

                using (SmtpClient smtp = new SmtpClient())
                {

                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                    // System.Web.UI.Page.RegisterStartupScript("UserMsg", "<script>alert('Successfully Send...');if(alert){ window.location='SendMail.aspx';}</script>");
                }
            }
            Response.Redirect("/#");
        }
    }
}