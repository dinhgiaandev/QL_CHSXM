using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_BookService")]
    public class BookService : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        [StringLength(150)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(12)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên xe không được để trống")]
        [StringLength(150)]
        public string NameCar { get; set; }
        public int VisitCount { get; set; }

        [StringLength(150)]
        public string Note { get; set; }

        public string ServiceTitle { get; set; }
        public string ServiceCategoryId { get; set; }
        public int TypePayment { get; set; }
        public string ProductCategoryId { get; set; }

        public int? ServiceId { get; set; }
        public int? ProductId { get; set; }

        // Sửa đổi từ int sang List<int> cho đa lựa chọn
        public List<int> ServiceIds { get; set; }
        public List<int> ProductIds { get; set; }
        [NotMapped]
        public List<int> SelectedServiceIds { get; set; }
        [NotMapped]
        public List<int> SelectedProductIds { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                decimal serviceTotal = Services?.Sum(s => s.Price) ?? 0;
                decimal productTotal = Products?.Sum(p => p.Price) ?? 0;
                return serviceTotal + productTotal;
            }
        }

        public BookService()
        {
            ProductIds = new List<int>();
            ServiceIds = new List<int>();
            Products = new HashSet<Product>();
            Services = new HashSet<Service>();
        }

        [Required(ErrorMessage = "Ngày hẹn không được để trống")]
        public DateTime BookingDate { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<ServiceCategory> ServiceCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
