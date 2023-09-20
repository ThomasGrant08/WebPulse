using System.Security.Policy;

namespace WebPulse2023.Models
{
    public class Role
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<Permissions> Permissions { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public Role()
        {
            CreatedOn = DateTime.UtcNow;
            Guid = Guid.NewGuid();
        }
    }

    public class Permissions
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public Resource Resource { get; set; }

        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanShare { get; set; }

        public Role Role { get; set; }
        public int RoleId { get; set; }
    }

    public enum Resource
    {
        Website,
        Credential,
        Group
    }
}
