using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ServiceId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }  // Add IsActive property
        public decimal Discount { get; set; }
        public decimal VAT { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        public virtual Service Service { get; set; }
        public virtual BookService BookService { get; set; }

    }
}