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
        // Display property to format product information
        public string? Display
        {
            get
            {
                return $"{Id}\t{Name}\t\t\t{Quantity}";
            }
        }

        public Product()
        {
            Id = -1;
            Name = string.Empty;
            Quantity = 1;
        }

        // Override ToString method to return Display property
        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
