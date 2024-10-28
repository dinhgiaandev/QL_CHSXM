using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_Order")]
    public class Order:CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [Required(ErrorMessage = "Tên khách hàng không để trống")]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống")]
        [StringLength(11)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không để trống")]
        [StringLength(250)]
        public string Address { get; set; }
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public DateTime? PaymentDate { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}