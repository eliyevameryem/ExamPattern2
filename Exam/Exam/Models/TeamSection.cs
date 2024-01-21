using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class TeamSection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
