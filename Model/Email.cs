namespace Inspecciones.Model
{
    public class Email
    {
        public string email { get; set; } = string.Empty;
        public string cc { get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;

        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 587;

        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
