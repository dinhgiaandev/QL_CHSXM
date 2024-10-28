using System;
using System.ComponentModel.DataAnnotations;

namespace QL_CHSXM.Models
{
    public class BookServiceViewModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal VAT { get; set; } // Change to public set

        public DateTime BookingDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        // Constructor
        public BookServiceViewModel()
        {
            VAT = CalculateVAT(); // Call CalculateVAT() in constructor
        }

        // Calculate VAT based on Price
        public decimal CalculateVAT() // Change to public method
        {
            return Price * 0.1m; // Assuming VAT rate is 10%
        }

        // Calculate total price including VAT
        public decimal CalculateTotalPrice()
        {
            return Price + VAT;
        }

        // Example validation method
        public bool IsValid()
        {
            // Perform validation checks here (e.g., required fields, valid email/phone, etc.)
            return !string.IsNullOrEmpty(CustomerName)
                   && !string.IsNullOrEmpty(CustomerPhone)
                   && !string.IsNullOrEmpty(CustomerEmail)
                   && ServiceId > 0
                   && Price > 0
                   && BookingDate > DateTime.MinValue;
        }
    }

}