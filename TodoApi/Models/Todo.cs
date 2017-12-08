using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TodoApi.Models
{
    public class Todo
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public bool IsComplete { get; set; }
    }
}
