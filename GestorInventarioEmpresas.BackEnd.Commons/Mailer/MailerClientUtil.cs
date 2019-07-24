using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Commons.Mailer
{
    public static class MailerClientUtil
    {
        private static readonly string EmailTemplatesFolderName = "EmailTemplates";

        public static readonly string PostulationNotificacionTemplateFileName;
        public static readonly string QuestionNotificacionTemplateFileName;
        public static readonly string AnswerNotificacionTemplateFileName;
        public static readonly string TaskNotificacionTemplateFileName;

        static MailerClientUtil()
        {
            PostulationNotificacionTemplateFileName = "Postulation_Notificacion.html";
            QuestionNotificacionTemplateFileName = "Question_Notificacion.html";
            AnswerNotificacionTemplateFileName = "Answer_Notificacion.html";
            EmailTemplatesFolderName = "EmailTemplates";
            TaskNotificacionTemplateFileName = "Task_Notificacion.html";
        }

        public static string CreateBodyFromTemplateFile(string templateName, object data)
        {
            try
            {
                var result = string.Empty;

                var executableLocation = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath);
                var emailLocation = Path.Combine(executableLocation, EmailTemplatesFolderName);
                var filename = Path.Combine(emailLocation, templateName);

                if (File.Exists(filename))
                {
                    var content = File.ReadAllText(filename, Encoding.UTF8);
                    result = CreateBodyFromTemplateString(content, data);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public static string CreateBodyFromTemplateString(string content, object data)
        {
            try
            {
                var result = string.Empty;

                if (!string.IsNullOrEmpty(content))
                {
                    var template = Handlebars.Compile(content);
                    result = template(data);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Envio de correo usando la configuracion para el protocolo SMTP en el web.config o app.config
        /// </summary>
        /// <param name="model">Datos del mensaje</param>
        public static void SendMail(EmailModel model)
        {
            try
            {
                var message = CreateMailMessageFromEmailModel(model);
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Envio de correo usando la configuracion para el protocolo SMTP especificada por parametros
        /// </summary>
        /// <param name="model">Datos del mensaje</param>
        /// <param name="userName">Usuario</param>
        /// <param name="password">Contrasenna</param>
        /// <param name="host">Servdor SMTP</param>
        /// <param name="port">Puerto</param>
        /// <param name="enableSsl">Especifica uso de SSL</param>
        /// <param name="from">Remitente del mensaje</param>
        /// <param name="fromDisplayName">Nombre del remitente del mensaje</param>
        public static void SendMail(EmailModel model, string userName, string password, string host, int port, bool enableSsl, string from, string fromDisplayName = null)
        {
            try
            {
                var message = CreateMailMessageFromEmailModel(model, from, fromDisplayName);
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = userName,
                        Password = password
                    };
                    smtp.Credentials = credential;
                    smtp.Host = host;
                    smtp.Port = port;
                    smtp.EnableSsl = enableSsl;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Send(message);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Envio de correo de manera asincronica usando la configuracion para el protocolo SMTP en el web.config o app.config
        /// </summary>
        /// <param name="model">Datos del mensaje</param>
        public async static Task SendMailAsync(EmailModel model)
        {
            try
            {
                var message = CreateMailMessageFromEmailModel(model);
                await Task.Run(() =>
                {
                    using (var smtp = new SmtpClient())
                    {
                        try
                        {
                            smtp.Send(message);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Envio de correo de manera asincronica usando la configuracion para el protocolo SMTP especificada por parametros
        /// </summary>
        /// <param name="model">Datos del mensaje</param>
        /// <param name="userName">Usuario</param>
        /// <param name="password">Contrasenna</param>
        /// <param name="host">Servdor SMTP</param>
        /// <param name="port">Puerto</param>
        /// <param name="enableSsl">Especifica uso de SSL</param>
        /// <param name="from">Remitente del mensaje</param>
        /// <param name="fromDisplayName">Nombre del remitente del mensaje</param>
        public async static Task SendMailAsync(EmailModel model, string userName, string password, string host, int port, bool enableSsl, string from, string fromDisplayName = null)
        {
            try
            {
                var message = CreateMailMessageFromEmailModel(model, from, fromDisplayName);
                await Task.Run(() =>
                {
                    using (var smtp = new SmtpClient())
                    {
                        try
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = userName,
                                Password = password
                            };
                            smtp.Credentials = credential;
                            smtp.Host = host;
                            smtp.Port = port;
                            smtp.EnableSsl = enableSsl;

                            smtp.Send(message);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                });
            }
            catch
            {
                throw;
            }
        }

        private static MailMessage CreateMailMessageFromEmailModel(EmailModel model, string from = null, string fromDisplayName = null)
        {
            var message = new MailMessage
            {
                Subject = model.Subject,
                Body = model.Body
            };
            message.IsBodyHtml = model.IsBodyHtml;
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;

            if (!string.IsNullOrEmpty(from))
            {
                if (!string.IsNullOrEmpty(fromDisplayName))
                {
                    message.From = new MailAddress(from, fromDisplayName);
                }
                else
                {
                    message.From = new MailAddress(from);
                }
            }

            if (model.Recipients != null && model.Recipients.Count > 0)
            {
                var recipients = from r in model.Recipients where !string.IsNullOrEmpty(r) select new MailAddress(r);
                foreach (var r in recipients)
                {
                    message.To.Add(r);
                }
            }

            if (model.CcAddress != null && model.CcAddress.Count > 0)
            {
                var ccAddress = from cc in model.CcAddress where !string.IsNullOrEmpty(cc) select new MailAddress(cc);
                foreach (var cc in ccAddress)
                {
                    message.CC.Add(cc);
                }
            }

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                foreach (var a in model.Attachments)
                {
                    message.Attachments.Add(new Attachment(a.Content, a.Name, a.MediaType));
                }
            }
            return message;
        }

        /// <summary>
        /// Datos del mensaje a enviar por correo
        /// </summary>
        public struct EmailModel
        {
            /// <summary>
            /// Destinatarios
            /// </summary>
            public ICollection<string> Recipients { get; set; }

            /// <summary>
            /// Con copia a:
            /// </summary>
            public ICollection<string> CcAddress { get; set; }

            /// <summary>
            /// Asunto del mensaje
            /// </summary>
            public string Subject { get; set; }

            /// <summary>
            /// Cuerpo del mensaje
            /// </summary>
            public string Body { get; set; }

            /// <summary>
            /// Especifica si el cuerpo del mensaje esta en formato HTML
            /// </summary>
            public bool IsBodyHtml { get; set; }

            /// <summary>
            /// Documentos adjuntos
            /// </summary>
            public ICollection<EmailAttachmentModel> Attachments { get; set; }
        }

        /// <summary>
        /// Datos del documento adjunto
        /// </summary>
        public struct EmailAttachmentModel
        {
            /// <summary>
            /// Contenido a adjuntar
            /// </summary>
            public Stream Content { get; set; }

            /// <summary>
            /// Nombre del fichero adjunto
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Usar la clase System.Net.Mime.MediaTypeNames para especificar este valor, ejemplo: MediaTypeNames.Text.Plain
            /// </summary>
            public string MediaType { get; set; }
        }
    }
}
