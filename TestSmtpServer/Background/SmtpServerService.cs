using SmtpServer;

namespace TestSmtpServer.Background
{
    public class SmtpServerService : BackgroundService
    {
        private readonly SignalRMessageStore _signalRMessageStore;

        public SmtpServerService(SignalRMessageStore signalRMessageStore)
        {
            _signalRMessageStore = signalRMessageStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new SmtpServerOptionsBuilder()
                .ServerName("localhost")
                .Port(25, 587)
                .Build();

            var serviceProvider = new SmtpServer.ComponentModel.ServiceProvider();
            serviceProvider.Add(_signalRMessageStore);

            var smtpServer = new SmtpServer.SmtpServer(options, serviceProvider);
            await smtpServer.StartAsync(stoppingToken);
        }
    }
}
