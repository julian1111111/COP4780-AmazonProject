using AmazonProject.Models;
using Library.eCommerce.DTO;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Library.eCommerce.Models
{
    public class Item
    {
        public Item()
        {
            Product = new ProductDTO();
        }

        public Item(Item i)
        {
            Product = new ProductDTO(i.Product);
            Quantity = i.Quantity;
            Id = i.Id;
        }

        public ProductDTO Product {  get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Display { get { return Product?.Display ?? string.Empty; } }
        public string sId { get { return $"{Id}"; } }
        public string sName { get { return $"{Product.Name}"; } }
        public string sQuantity { get { return $"{Product.Quantity}"; } }
        public string sPrice { get { return $"${Product.Price}"; } }
        public string lineItemPrice { get { return (Product.Price * Product.Quantity).ToString("C2"); } }
    }
}
