using Microsoft.AspNetCore.SignalR;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System.Buffers;
using TestSmtpServer.Hubs;

namespace TestSmtpServer.Background
{
    public class SignalRMessageStore : MessageStore
    {
        private IHubContext<UiUpdateHub> _hubContext;

        public SignalRMessageStore(IHubContext<UiUpdateHub> hubContext)
        {
             _hubContext = hubContext;
        }

        /// <summary>
        /// Save the given message to the underlying storage system.
        /// </summary>
        /// <param name="context">The session context.</param>
        /// <param name="transaction">The SMTP message transaction to store.</param>
        /// <param name="buffer">The buffer that contains the message content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A unique identifier that represents this message in the underlying message store.</returns>
        public override async Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            await using var stream = new MemoryStream();

            var position = buffer.GetPosition(0);
            while (buffer.TryGet(ref position, out var memory))
            {
                stream.Write(memory.Span);
            }

            stream.Position = 0;

            var message = await MimeKit.MimeMessage.LoadAsync(stream, cancellationToken);

            var newEmailMessage = new EmailMessageDto()
            {
                Subject = message.Subject,
                BodyPreview = message.TextBody,
                From = message.From.Select(x => x.Name).ToList(),
                Date = message.Date
            };

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", newEmailMessage);

            return SmtpResponse.Ok;
        }
    }
}
