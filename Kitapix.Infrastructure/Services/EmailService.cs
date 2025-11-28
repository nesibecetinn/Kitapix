using Kitapix.Application.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Kitapix.Infrastructure.Services
{
	public class EmailService : IEmailService
	{
		private readonly SmtpClient _smtpClient;
		private const string smtpServer = "smtp.gmail.com";
		private const int smtpPort = 587; // TLS için 587, SSL için 465
		private const string senderEmail = "nesibecetin3@gmail.com"; // Gönderen e-posta adresin
		private const string senderName = "Nesibe Çetin";
		private const string appPassword = "xzbu vdnz mmpn pheb";

		

		public async Task SendEmailAsync(string to, string subject, string body)
		{
			try
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress(senderName, senderEmail));
				message.To.Add(new MailboxAddress("", to));
				message.Subject = subject;

				var bodyBuilder = new BodyBuilder { HtmlBody = body };
				message.Body = bodyBuilder.ToMessageBody();

				using var smtpClient = new SmtpClient();
				await smtpClient.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);

				// `Authenticate` metodunu kullanarak kimlik doğrulama yapalım
				smtpClient.Authenticate(senderEmail, appPassword);

				await smtpClient.SendAsync(message);
				await smtpClient.DisconnectAsync(true);

				Console.WriteLine("E-posta başarıyla gönderildi.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"E-posta gönderimi sırasında hata oluştu: {ex.Message}");
			}


		}

		public async Task SendTemplatedEmailAsync(string to, string subject, string templateName, Dictionary<string, string> keys)
		{
			var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplates", templateName);
			if (!File.Exists(templatePath))
				throw new FileNotFoundException("Mail template bulunamadı.", templatePath);

			var body = await File.ReadAllTextAsync(templatePath);

			// Placeholder'ları değiştir
			foreach (var kvp in keys)
			{
				body = body.Replace($"{{{kvp.Key}}}", kvp.Value);
			}

			await SendEmailAsync(to, subject, body);
		}
	}
}
