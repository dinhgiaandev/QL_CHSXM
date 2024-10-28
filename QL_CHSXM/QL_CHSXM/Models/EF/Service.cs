using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_Service")]
    public class Service : CommonAbstract
    {
        public Service()
        {
            this.ServiceImage = new HashSet<ServiceImage>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Alias { get; set; }

        [StringLength(50)]
        public string ServiceCode { get; set; }
        [StringLength(250)]
        public string Description { get; set; }

        [AllowHtml]
        [StringLength(250)]
        public string Detail { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá gốc không hợp lệ.")]
        public decimal OriginalPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không hợp lệ.")]
        public decimal Price { get; set; }

        public decimal? PriceSale { get; set; }

        public int ViewCount { get; set; }

        public int ServiceCategoryId { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public virtual ServiceCategory ServiceCategory { get; set; }

        public virtual ICollection<ServiceImage> ServiceImage { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<BookService> BookServices { get; set; }
    }
}
