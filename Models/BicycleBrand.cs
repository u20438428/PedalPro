using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PedalProAPI.Models
{
    [Table("BicycleBrand")]
    public partial class BicycleBrand
    {
        [Key]
        [Column("BicycleBrand_ID")]
        public int BicycleBrandId { get; set; }

        [Column("BrandImage_ID")]
        public int? BrandImageId { get; set; }

        [Column("BicycleCategoryID")]
        [ForeignKey("BicycleCategory")]
        public int BicycleCategoryId { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? BrandName { get; set; }

        [ForeignKey("BrandImageId")]
        //[InverseProperty("BicycleBrands")]
        public virtual BrandImage? BrandImage { get; set; }

        [ForeignKey("BicycleCategoryId")]
        //[InverseProperty("Clients")]
        public virtual BicycleCategory? BicycleCategory { get; set; }
    }
}
