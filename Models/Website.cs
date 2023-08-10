using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPulse2023.Models
{
	public class Website
	{
        public int Id { get; set; }
		[Url]
        [ValidateBaseDomain(ErrorMessage = "Invalid base URL format.")]
        public string Url { get; set; }
		public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        public Website()
        {
            Active = true;
            CreatedAt = DateTime.UtcNow;
        }
    }

    public class ValidateBaseDomainAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var url = value as string;

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            var baseDomain = uri.Host;

            // Check if the URL contains a path or query
            if (!string.IsNullOrEmpty(uri.PathAndQuery) && (uri.PathAndQuery != "/" || !string.IsNullOrEmpty(uri.Query)))
            {
                return new ValidationResult(ErrorMessage);
            }

            // You can add additional checks here if needed
            // For example, check if the scheme is HTTP or HTTPS

            return ValidationResult.Success;
        }
    }
}
