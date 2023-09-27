using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedalProAPI.Models
{
    [Table("DatabaseTimer")]
    public partial class DatabaseTimer
    {
        [Key]
        [Column("DatabaseTimer_ID")]
        public int DatabaseTimerId { get; set; }

        public int? DatabaseTimerHours { get; set; }
    }
}
