using System.ComponentModel.DataAnnotations.Schema;

namespace WebPulse2023.Models
{
	public class WebPing
	{
		public int Id { get; set; }

		
		public int WebsiteId { get; set; }
        public DateTime Timestamp { get; set; }
		public bool? isUp { get; set;}
		public string ResponseTime { get; set; }
        public int StatusCode { get; set; }

        [ForeignKey("WebsiteId")]  // This attribute links the foreign key property to the navigation property
        public Website Website { get; set; }  // This is the navigation property
    }
}
