using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PedalProAPI.Models
{
    [Table("WorkoutType")]
    public partial class WorkoutType
    {
        [Key]
        [Column("WorkoutType_ID")]
        public int WorkoutTypeId { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? WorkoutTypeName { get; set; }

        /*
        [InverseProperty("WorkoutType")]
        public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        */
    }
}
