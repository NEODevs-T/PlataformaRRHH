using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using Inspecciones.Model;

namespace Inspecciones.Services
{
    public interface IEmailServices
    {
        Task SendEmailAsync(Email request);

        Task<bool> ContruccionEmail(string personas, string cuerpo, string asunto);

        string ConstruccionCuerpo(Inspeccion inspeccion, List<InspecDatum> listData);
    }

    public class EmailServices : IEmailServices
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailServices(IConfiguration config)
        {
            _smtpSettings = config.GetSection("Smtp").Get<SmtpSettings>() ?? new SmtpSettings();
        }

        /// <summary>
        /// Envía un correo usando MailKit. Email.email y Email.cc se esperan como string con emails separados por ';' o ','.
        /// </summary>
        public async Task SendEmailAsync(Email request)
        {
            try
            {
                if (request is null) throw new ArgumentNullException(nameof(request));

                // Normaliza listas de destinatarios desde strings (separadas por ';' o ',')
                var toList = SplitEmails(request.email);
                var ccList = SplitEmails(request.cc);

                var message = new MimeMessage();

                // From (usa settings; si faltan, usa valores por defecto)
                var fromName = string.IsNullOrWhiteSpace(_smtpSettings.SenderName)  ? "Inspecciones"           : _smtpSettings.SenderName;
                var fromMail = string.IsNullOrWhiteSpace(_smtpSettings.SenderEmail) ? "no-reply@tu-dominio.com" : _smtpSettings.SenderEmail;
                message.From.Add(new MailboxAddress(fromName, fromMail));

                // To
                foreach (var addr in toList)
                {
                    message.To.Add(MailboxAddress.Parse(addr));
                }

                // CC
                foreach (var addr in ccList)
                {
                    message.Cc.Add(MailboxAddress.Parse(addr));
                }

                // Subject & Body
                message.Subject = request.subject ?? string.Empty;
                message.Body = new TextPart(TextFormat.Html) { Text = request.body ?? string.Empty };

                // SMTP
                var host = string.IsNullOrWhiteSpace(_smtpSettings.Server) ? "localhost" : _smtpSettings.Server;
                var port = _smtpSettings.Port > 0 ? _smtpSettings.Port : 25;

                using var client = new SmtpClient();

                await client.ConnectAsync(host, port, SecureSocketOptions.StartTlsWhenAvailable);

                if (!string.IsNullOrWhiteSpace(_smtpSettings.UserName) &&
                    !string.IsNullOrWhiteSpace(_smtpSettings.Password))
                {
                    await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch
            {
                // Mantiene el stack original
                throw;
            }
        }

        /// <summary>
        /// Construye el Email a partir de alias separados por ';' (sin dominio) y envía.
        /// Ejemplo personas: "gabriel.arcila;otro.usuario"
        /// </summary>
        public async Task<bool> ContruccionEmail(string personas, string cuerpo, string asunto)
        {
            try
            {
                // Personas → lista de correos con dominio
                var usuarios = (personas ?? string.Empty)
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(u => $"{u}@paveca.com.ve")
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // Validación mínima: al menos un destinatario correcto
                if (usuarios.Count == 0 || usuarios.All(x => string.Equals(x, "@paveca.com.ve", StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                // Como Email.email y Email.cc son strings, se pasan como cadena separada por ';'
                var mail = new Email
                {
                    email   = string.Join(';', usuarios),
                    cc      = string.Empty, // ajusta si quieres CC fijo
                    subject = asunto ?? string.Empty,
                    body    = cuerpo ?? string.Empty
                };

                await SendEmailAsync(mail);
                return true;
            }
            catch
            {
                // Devuelve false según la semántica original
                return false;
            }
        }

        /// <summary>
        /// Genera un HTML con las preguntas marcadas como defectuosas (Iddata == 0).
        /// </summary>
        public string ConstruccionCuerpo(Inspeccion inspeccion, List<InspecDatum> listData)
        {
            var defectuosos = (listData ?? new List<InspecDatum>())
                .Where(l => l != null && l.Iddata == 0)
                .ToList();

            if (defectuosos.Count == 0)
            {
                return "<div>Ningún defecto</div>";
            }

            var html = @"
<div style='height: 99vh;'>
  <table style='width: 100%; border-collapse: collapse;' class='tableImpresion'>
    <tr class='encabezadoTableImpresion'>
      <th>#</th>
      <th>Pregunta</th>
      <th>Defectuoso</th>
      <th>Observación</th>
    </tr>";

            for (int i = 0; i < defectuosos.Count; i++)
            {
                var data = defectuosos[i];
                var num = i + 1;
                var pregunta = data?.IdMaqPreNavigation?.IdPreguntaNavigation?.Pdescri ?? "(N/A)";
                var observ = data?.Idobserv ?? string.Empty;

                html += $@"
    <tr>
      <td style='border: 1px solid black; text-align:center;'>{num}</td>
      <td style='border: 1px solid black; text-align:left; padding: 4px;'>{System.Net.WebUtility.HtmlEncode(pregunta)}</td>
      <td style='border: 1px solid black; text-align:center;'>Defectuoso</td>
      <td style='border: 1px solid black; text-align:left; padding: 4px;'>{System.Net.WebUtility.HtmlEncode(observ)}</td>
    </tr>";
            }

            html += @"
</table>
</div>

<style>
.tableImpresion tr { text-align: center; }
.encabezadoTableImpresion {
    color: white;
    background-color: rgb(0, 120, 83);
    font-size: 1.1em;
    font-weight: 800;
    text-align: center;
}
.encabezadoTableImpresion th {
    padding: 6px;
    border: 1px solid black;
}
.tableImpresion tr td {
    border: 0.5px solid black;
    height: 40px;
}
</style>";

            return html;
        }

        /// <summary>
        /// Divide una cadena con emails separados por ';' o ',' en una lista de direcciones válidas.
        /// </summary>
        private static List<string> SplitEmails(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return new List<string>();

            var parts = raw.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Filtra direcciones mínimamente válidas (contienen '@')
            return parts
                .Where(p => p.Contains('@'))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
