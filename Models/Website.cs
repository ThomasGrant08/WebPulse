using System.ComponentModel.DataAnnotations;

namespace WebPulse2023.Models
{
	public class Website
	{
        public int Id { get; set; }
		[Url]
		public string Url { get; set; }
		public string Name { get; set; }
        public bool Active { get; set; }

        public Website()
        {
            Active = true;
        }
    }
}
