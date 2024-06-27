using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.AutomaticMigrations.Sample.Data.Entities
{
    //this entity is mapped via data annotations
    [Table("PocoTodos")]
    [Index(nameof(Title))]
    public class TodoPoco
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(200)")]
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; }
    }
}
