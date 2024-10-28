using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_CHSXM.Models.EF
{
    [Table("tb_Invoice")]
    public class HoaDon
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ không được để trống")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Giá tiền không được để trống")]
        [Display(Name = "Giá tiền")]
        public decimal Price { get; set; }

        [Display(Name = "Tổng giá tiền sau VAT")]
        public decimal TotalPriceAfterVAT { get; set; }
    }
}
