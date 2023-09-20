using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Security.Policy;

namespace WebPulse2023.Models
{

    public class Group 
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }

        public Group()
        {
            DateCreated = DateTime.UtcNow;
            Guid = Guid.NewGuid();
        }

    }

}