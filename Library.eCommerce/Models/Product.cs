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

        // Display property to format product information
        public string? Display
        {
            get
            {
                return string.Format("{0,-10} {1, -30} {2, -10} {3, -10}", Id, Name, Quantity, Price.ToString("C"));
            }
        }

        public Product()
        {
            Id = -1;
            Name = "Default";
            Quantity = 1;
            Price = 0.0;
        }

        // Override ToString method to return Display property
        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
