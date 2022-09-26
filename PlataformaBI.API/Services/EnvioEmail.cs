using Domain;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace PlataformaBI.API.Servicos
{
    public class EnvioEmail
    {

        private SmtpClient client;
        private MailMessage mailMessage;
        private Email email;
        public EnvioEmail(Email email)
        {
            this.email = email;
            this.configurarEmail();
        }
        public async Task<string> enviarEmail(Empresas empresas)
        {
            try
            {
                this.mailMessage.Body = repleceWords(empresas);
                this.mailMessage.Subject = this.email.tituloEmail;
                string[] subs = empresas.Email.Split(';');

                foreach (var sub in subs)
                {
                    this.mailMessage.To.Add(sub);
                }
   
                try
                {
                    await this.client.SendMailAsync(this.mailMessage);
                    return "E-mail enviado com sucesso!";
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private void configurarEmail()
        {
            this.client = new SmtpClient(this.email.smtp);
            this.client.UseDefaultCredentials = false;
            this.client.Credentials = new NetworkCredential(this.email.email, this.email.senha);
            this.client.Port = this.email.porta;
            this.client.EnableSsl = this.email.SSL;

            this.mailMessage = new MailMessage();
            this.mailMessage.From = new MailAddress(this.email.email);
            this.mailMessage.IsBodyHtml = true;
        }


        private string repleceWords(Empresas empresa)
        {
            var texto = this.email.corpoEmail;
            var words = texto.Split(" ");
            var alterar = words.Where(p => p.Contains("#")).ToList() ;
            PropertyInfo[] properties = typeof(Empresas).GetProperties();

            foreach (var s in alterar) {
                foreach (PropertyInfo property in properties)
                {
                    if (s.ToUpper().Replace("#","").Equals(property.Name.ToUpper()))
                    {
                        texto = texto.Replace(s, property.GetValue(empresa).ToString());
                    }
                }
            }
            return texto;
        }


    }
}
