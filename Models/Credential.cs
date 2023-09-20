namespace WebPulse2023.Models
{
    public class Credential
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public int WebsiteId { get; set; }
        public Website Website { get; set; }

        public string Username { get; set; }

        public string EncryptedPassword { get; set; }

    }
}
