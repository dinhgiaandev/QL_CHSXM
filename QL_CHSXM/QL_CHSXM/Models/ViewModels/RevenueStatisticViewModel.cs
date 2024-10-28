using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_CHSXM.Models.ViewModels
{
    public class RevenueStatisticViewModel
    {
        public DateTime Date { get; set; }
        public decimal Benefit { get; set; }//lợi nhuân
        public decimal Revenues { get; set; }// tổng giá bán
        public int ProductQuantity { get; set; } // Số lượng sản phẩm trong kho
        public int ScheduledCount { get; set; } // Số lượng danh sách đặt lịch
    }
}