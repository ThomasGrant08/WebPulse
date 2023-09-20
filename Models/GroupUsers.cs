using System.ComponentModel.DataAnnotations.Schema;

namespace WebPulse2023.Models
{
    public class GroupUsers
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        [ForeignKey("Group")]
        public string GroupGuid { get; set; }
        public Group Group { get; set; }


        [ForeignKey("User")]
        public string UserGuid { get; set; }
        public SystemUser User { get; set; }

        [ForeignKey("Role")]

        public string RoleGuid { get; set; }
        public Role Role { get; set; }
        public bool Accepted { get; set; }
    }
}
