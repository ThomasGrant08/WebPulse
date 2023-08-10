using System.ComponentModel.DataAnnotations.Schema;

namespace WebPulse2023.ViewModels
{
    public class WebsiteViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int UpAmount { get; set; }
        public int DownAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
    }

    public class WebsiteEditViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int UpAmount { get; set; }
        public int DownAmount { get; set; }
    }

}
