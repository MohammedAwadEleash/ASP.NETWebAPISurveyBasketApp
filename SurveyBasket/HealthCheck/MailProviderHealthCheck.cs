using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SurveyBasket.Settings;

namespace SurveyBasket.HealthCheck
{
    public class MailProviderHealthCheck(IOptions<MailSettings> mailSettings) : IHealthCheck
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            try
            {
                using var smtp = new SmtpClient();


                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, cancellationToken);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                return await Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception exception)
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy(exception: exception));
            }
        }
    }
}
