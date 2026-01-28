using BakeItCountApi.Services;
using Quartz;

[DisallowConcurrentExecution]
public class SendEmailJob : IJob
{
    private readonly EmailService _emailService;
    private readonly ILogger<SendEmailJob> _logger;

    public SendEmailJob(EmailService emailService, ILogger<SendEmailJob> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var today = DateTimeOffset.Now.DayOfWeek;
        if (today == DayOfWeek.Monday || today == DayOfWeek.Friday)
        {
            _logger.LogInformation("Hoje é {Day}. Preparando para enviar e-mail.", today);
            try
            {
                await _emailService.SendEmailAsync("",
                    "Essa semana o bolo é seu!", "Entre no site para confirmar ou solicitar uma troca.");
                _logger.LogInformation("E-mail enviado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar e-mail");
            }
        }
    }
}
