using System.Net.Mail;
using System;
using ASTSoft.Desarrollos.Utils;
using System.Collections.Generic;

namespace ASTSoft.Desarrollos.Utils
{
    public class CorreoService
    {
        public List<string> Destinatarios { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
    }

    public static class InterfaceEmail
    {
        public static void SendEmail(CorreoService model)
        {
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("aplicacionesService365@outlook.com", "Service365", System.Text.Encoding.UTF8);

            foreach (string item in model.Destinatarios)
            {
                correo.To.Add(item);
            }

            correo.Subject = model.Asunto;
            correo.Body = model.Cuerpo;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.office365.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("aplicacionesService365@outlook.com", "S3rV1c3365");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(correo);
            }
            catch (Exception)
            {

            }
        }
    }
}
