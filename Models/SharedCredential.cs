namespace WebPulse2023.Models
{
    public class SharedCredential
    {
        public int Id { get; set; }
        public int CredentialId { get; set; }
        public Credential Credential { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string UserId { get; set; }
        public SystemUser User { get; set; }
    }
}
