using Library.eCommerce.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonProject.Models
{
    public class Product
    {
        // Product properties
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }


        public Product()
        {
            Id = -1;
            Name = "Default";
            Quantity = 1;
            Price = 0.0;
        }

        public Product(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            Quantity = p.Quantity;
            Price = p.Price;
        }

        public Product(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
            Quantity = p.Quantity;
            Price = p.Price;
        }
    }
}
