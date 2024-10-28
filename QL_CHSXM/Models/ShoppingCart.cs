using System;
using System.Collections.Generic;
using System.Linq;

namespace QL_CHSXM.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }

        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
        }

        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExits != null)
            {
                checkExits.Quantity += Quantity;
                var totalPriceBeforeVAT = checkExits.Price * checkExits.Quantity;
                checkExits.VAT = totalPriceBeforeVAT * 0.10m; // Tính VAT
                checkExits.TotalPrice = totalPriceBeforeVAT + checkExits.VAT; // Tổng tiền với VAT
            }
            else
            {
                item.Quantity = Quantity;
                var totalPriceBeforeVAT = item.Price * item.Quantity;
                item.VAT = totalPriceBeforeVAT * 0.10m; // Tính VAT
                item.TotalPrice = totalPriceBeforeVAT + item.VAT; // Tổng tiền với VAT
                Items.Add(item);
            }
        }

        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }
        }

        public void UpdateQuantity(int id, int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                var totalPriceBeforeVAT = checkExits.Price * checkExits.Quantity;
                checkExits.VAT = totalPriceBeforeVAT * 0.10m; // Tính VAT
                checkExits.TotalPrice = totalPriceBeforeVAT + checkExits.VAT; // Tổng tiền với VAT
            }
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TotalPrice);
        }

        public decimal GetTotalVAT()
        {
            return Items.Sum(x => x.VAT);
        }

        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }

        public void ClearCart()
        {
            Items.Clear();
        }
    }

    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalPrice { get; set; } // Sẽ bao gồm VAT
    }
}
