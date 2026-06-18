using BMS_Logistics.Application.Common;
using BMS_Logistics.Application.DTOs;
using BMS_Logistics.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BMS_Logistics.Application.Services
{
    internal class EmailService : IEmailService
    {

        private readonly SmtpSettings _smtpSettings;
        // Dictionary to manage by a Delegate function the method to use based on currentControllerName
        private readonly Dictionary<string, Func<object, UserDto, string>> controllerHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="_smtpSettings">The SMTP settings.</param>
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;

            controllerHandlers = new Dictionary<string, Func<object, UserDto, string>>
            {
                {
                    "verification",
                    (requestData, user) =>
                    {
                        var dto = (VerificationEmailDto)requestData;
                        return CreateVerificationEmail(dto.User!, dto.Code!);
                    }
                },
                {
                    "forget-password",
                    (requestData, user) =>
                    {
                        var dto = (ForgetPasswordEmailDto)requestData;
                        return CreateForgetPasswordEmail(dto.User, dto.Code);
                    }
                }
            };
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="EmailTo">The recipient email address.</param>
        /// <param name="CopyTo">The CC email address.</param>
        /// <param name="Subject">The email subject.</param>
        /// <param name="BodyMessage">The email body message.</param>
        /// <returns>True if the email was sent successfully, otherwise false.</returns>

        public bool Send(string EmailTo, string? CopyTo, string Subject, string BodyMessage)
        {
            try
            {
                using MailMessage mailMessage = new();
                using SmtpClient smtp = new(_smtpSettings.SmtpHost, _smtpSettings.SmtpPort);

                mailMessage.From = new MailAddress(_smtpSettings.FromMail, _smtpSettings.FromAlias, Encoding.UTF8);
                mailMessage.To.Add(EmailTo.Trim());

                if (!string.IsNullOrWhiteSpace(CopyTo))
                    mailMessage.CC.Add(CopyTo!.Trim());

                mailMessage.Subject = Subject.Trim();
                mailMessage.Body = BodyMessage.Trim();
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_smtpSettings.FromMail, _smtpSettings.Password);
                smtp.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                LinkedResource img = new(@"Resources\LOGO.png", "image/png")
                {
                    ContentId = "logo"
                };

                string logoImageHtml = @"<div style='text-align: center; margin-bottom: 20px; '>
                                            <img src='cid:logo' alt='logo' height='280' style='display: inline-block;'>
                                         </div>";

                BodyMessage = $"{logoImageHtml}{BodyMessage}";

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(BodyMessage, Encoding.UTF8, "text/html");
                htmlView.LinkedResources.Add(img);

                mailMessage.AlternateViews.Add(htmlView);

                smtp.Send(mailMessage);

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Generates the body message for an email.
        /// </summary>
        /// <typeparam name="T">The type of the request data.</typeparam>
        /// <param name="user">The user making the request.</param>
        /// <param name="superior">The superior of the user.</param>

        /// <param name="requestData">The request data.</param>
        /// <param name="currentController">The name of the current controller.</param>
        /// <returns>The body message for the email.</returns>
        public string BodyMessage<T>(UserDto user, T requestData, string templateKey)
        {
            if (!controllerHandlers.TryGetValue(templateKey, out var handler))
                throw new ArgumentException($"Plantilla no registrada: {templateKey}");

            return handler(requestData!, user);
        }

        public string CreateForgetPasswordEmail(UserDto user, string code)
        {
            int year = DateTime.Now.Year;
            string name = user.Name;
            string portal = "http://192.168.3.206:1519/gestor-seguros/auth/login";

            return $@"
                      <table width='100%' cellspacing='0' cellpadding='0'>
        <tr>
            <td align='center'>
                <table width='600' cellspacing='0' cellpadding='0'>
                    
                    <tr>
                        <td align='center' bgcolor='#0D3A52'
                            style='padding: 12px 0; border-radius: 10px 10px 0 0;'>
                            <h1 style='color: #ffffff;'>Gestor de Seguros</h1>
                        </td>
                    </tr>

                    <tr>
                        <td style='padding: 20px; font-size: 14px; color: #333'>
                            <p>Hola <strong>{name}</strong>,</p>

                            <p>
                                Hemos recibido una solicitud para restablecer tu contraseña.
                                Usa el siguiente código para continuar:
                            </p>

                            <div style='background:#AAE3F9;padding:15px;text-align:center;
                                        font-size:22px;font-weight:bold;letter-spacing:2px;'>
                                {code}
                            </div>

                            <p style='margin-top:20px'>
                                Utiliza el codigo para iniciar session, y luego cambia tu contraseña.
                            </p>

                            <p style='text-align:center;margin-top:30px'>
                                <a href='{portal}'>
                                    <button style='padding:10px 25px;
                                                   background:#00ACED;
                                                   color:white;
                                                   border:none;
                                                   border-radius:5px;
                                                   cursor:pointer;'>
                                        Ingresar
                                    </button>
                                </a>
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td align='center' bgcolor='#0D3A52'
                            style='padding:15px 0;border-radius:0 0 10px 10px;'>
                            <p style='color:white;font-size:14px'>
                                © {year} Coseg Corredores de Seguros - COSEG
                            </p>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
            ";
        }

        /// <summary>
        /// Generates a code email message.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="code">The temporary code for the user.</param>
        /// <returns>The formatted  email message.</returns>

        public string CreateVerificationEmail(UserDto user, string code)
        {
            int year = DateTime.Now.Year;
            string name = $"{user.Name}";

            string portal = "http://192.168.3.206:1519/gestor-seguros/auth/login";

            string emailFormat = $@"
            <table width='100%' cellspacing='0' cellpadding='0'>
                <tr>
                    <td align='center'>
                        <table width='600' cellspacing='0' cellpadding='0'>
                            <tr>
                                <td align='center' bgcolor='#0D3A52' style='padding: 10px 0;  border-radius: 10px;'>
                                    <h1 style='color: #ffffff; font-size: 24px;'>Gestor de Seguros</h1>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 20px 0;'>
                                    <p>Estimado {name},</p>
                                    <p>Este es el código de verificación para acceder al portal de Gestor de Seguros:</p>
                                    <div style='background-color: #AAE3F9; padding: 10px 0; text-align: center;'>
                                         <span style='font-size: 24px; font-weight: bold; display: inline-block;'>{code}</span>
                                    </div>
                                    <br><br>

                                    <p style='text-align: center'>
                                        <a href={portal}>
                                        <button style='padding: 10px 20px; background-color: #00ACED; color: white; border: none; border-radius: 5px; cursor: pointer;'>Gestor de Seguros - COSEG</button>
                                        </a>
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td align='center' bgcolor='#0D3A52' style='padding: 20px 0; border-radius: 10px;'>
                                    <p style='color: #ffffff; font-size: 14px;'>© {year} Coseg Corredores de Seguros - COSEG</p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>";
            return emailFormat;
        }

    }
}
