using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class Base
    {
        /*
         * The 'Key' attribute signals to entity framework that this should be used as the primary key in the database.
         * Required signals that it is required for insertion so null values will now be allowed.
         * GUIDs (Global Unique Identifiers) will be used throughout the project. This is so that the IDs cannot be guessed like if they were normal integers.
         */
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
