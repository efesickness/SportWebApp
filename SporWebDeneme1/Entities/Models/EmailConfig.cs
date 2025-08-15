using System.ComponentModel.DataAnnotations;

namespace SporWebDeneme1.Entities.Models
{
    public class EmailConfig
    {
        [Key]
        public int Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
    }

}
