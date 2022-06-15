using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class Base
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
    }
}
