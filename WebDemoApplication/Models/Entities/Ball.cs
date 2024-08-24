using System.ComponentModel.DataAnnotations;

namespace WebDemoApplication.Models.Entities
{
    public class Ball
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(100)]
        public decimal Price { get; set; }
        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
    }
}
