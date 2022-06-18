using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class Base
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public Guid CreatedBy { get; set; }
    }
}
