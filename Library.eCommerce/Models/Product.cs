using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? Display
        {
            get
            {
                return $"{Id}\t{Name}\t{Quantity}";
            }
        }

        public Product()
        {
            Id = -1;
            Name = string.Empty;
            Quantity = 1;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
