using System.ComponentModel.DataAnnotations.Schema;

namespace WebPulse2023.Models
{
	public class WebPing
	{
		public int Id { get; set; }

		[ForeignKey("Website")]
		public int WebsiteId { get; set; }
		public int StatusCode { get; set; }
		public DateTime PingTime { get; set; }
	}
}
