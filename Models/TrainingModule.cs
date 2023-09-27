using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PedalProAPI.Models
{
    [Table("TrainingModule")]
    public partial class TrainingModule
    {
        [Key]
        [Column("TrainingModule_ID")]
        public int TrainingModuleId { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? TrainingModuleName { get; set; }

        [StringLength(1000)]
        [Unicode(false)]
        public string? TrainingModuleDescription { get; set; }

        

        [Column("TrainingModuleStatus_ID")]
        public int? TrainingModuleStatusId { get; set; }

        /*
        [InverseProperty("TrainingModule")]
        public virtual ICollection<TrainingMaterial> TrainingMaterials { get; set; } = new List<TrainingMaterial>();
        */

        [ForeignKey("TrainingModuleStatusId")]
        //[InverseProperty("TrainingModules")]
        public virtual TrainingModuleStatus? TrainingModuleStatus { get; set; }
    }
}
