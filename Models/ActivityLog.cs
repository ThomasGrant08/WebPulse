using System.ComponentModel.DataAnnotations.Schema;

namespace WebPulse2023.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }

        [ForeignKey("AspNetUser")]
        public string UserId { get; set; }

        [ForeignKey("AspNetUser")]
        public string Impersonated { get; set; }
        public ActionType action { get; set; }
        public string AffectedObjectType { get; set; }
        public int AffectedObjectId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ActivityLog()
        {

        }

    }

    public enum ActionType
    {
        Create,
        Update,
        Delete
    }
}
