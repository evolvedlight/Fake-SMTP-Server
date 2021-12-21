using MimeKit;

namespace TestSmtpServer.Background
{
    internal class EmailMessageDto
    {
        public string Subject { get; set; }
        public string BodyPreview { get; set; }
        public List<string> From { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}