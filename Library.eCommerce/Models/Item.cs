using AmazonProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    public class Item
    {
        public Item()
        {
            Product = new Product();
        }

        public Product Product {  get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Display { get { return Product?.Display ?? string.Empty; } }
        public string sId { get { return $"{Id}"; } }
        public string sName { get { return $"{Product.Name}"; } }
        public string sQuantity { get { return $"{Product.Quantity}"; } }
        public string sPrice { get { return $"${Product.Price}"; } }
    }
}
