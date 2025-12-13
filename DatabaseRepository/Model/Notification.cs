using DatabaseRepository.Model.Enum;

namespace DatabaseRepository.Model
{
    public class Notification : Audit
    {
        public string NotificationType { get; set; } = Enum.NotificationType.Email.ToString();
        public required string SmtpHost { get; set; }
        public int SmtpPort { get; set; } = 587;
        public required string SmtpUser { get; set; }
        public required string SmtpPassword { get; set; }
    }
}
