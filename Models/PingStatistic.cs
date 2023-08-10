using System.ComponentModel.DataAnnotations.Schema;

namespace WebPulse2023.Models
{
    public class PingStatistic
    {
        public int Id { get; set; }
        public int WebsiteId { get; set; }
        [ForeignKey("WebsiteId")]  // This attribute links the foreign key property to the navigation property
        public Website Website { get; set; }  // This is the navigation property
        public RollupInterval Interval { get; set; }
        public DateTime Timestamp { get; set; }
        public int UpCount { get; set; }
        public int DownCount { get; set; }

    }

    public enum RollupInterval
    {
        Hourly,
        Daily,
        Weekly,
        Monthly
    }
}
