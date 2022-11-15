using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAPI.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }
        [NotMapped]
        public IFormFile files { get; set; }
        public string anexoName { get; set; }
    }
}
