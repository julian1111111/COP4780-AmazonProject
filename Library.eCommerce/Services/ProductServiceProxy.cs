using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonProject.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        private static ProductServiceProxy? _current;
        public static ProductServiceProxy Current => _current ??= new ProductServiceProxy();

        public List<Product?> InventoryProducts { get; } = new List<Product?>();
        public List<Product?> CartProducts { get; } = new List<Product?>();

        public int LastKey { get; private set; }

        public Product AddOrUpdateInventory(Product product)
        {
            var existingProduct = InventoryProducts.FirstOrDefault(p => p?.Id == product.Id);
            if (existingProduct == null)
            {
                Console.WriteLine("Adding new product to inventory");
                product.Id = ++LastKey;
                InventoryProducts.Add(product);
            }
            else
            {
                existingProduct.Name = product.Name;
                existingProduct.Quantity = product.Quantity;
            }

            return product;
        }

        public void AddToCart(int productId, int quantity)
        {
            try
            {
                var inventoryProduct = InventoryProducts.FirstOrDefault(p => p?.Id == productId);
                if (inventoryProduct == null)
                {
                    throw new InvalidOperationException("Product not found in inventory");
                }
                else if (inventoryProduct.Quantity < quantity)
                {
                    throw new InvalidOperationException("Not enough inventory");
                }

                inventoryProduct.Quantity -= quantity;

                var cartProduct = CartProducts.FirstOrDefault(p => p?.Id == productId);
                if (cartProduct == null)
                {
                    CartProducts.Add(new Product { Id = productId, Name = inventoryProduct.Name, Quantity = quantity });
                }
                else
                {
                    cartProduct.Quantity += quantity;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n");
            }
        }

        public void RemoveFromCart(int productId, int quantity)
        {
            var cartProduct = CartProducts.FirstOrDefault(p => p?.Id == productId);
            if (cartProduct == null || cartProduct.Quantity < quantity)
            {
                throw new InvalidOperationException("Not enough items in the cart.");
            }

            cartProduct.Quantity -= quantity;
            if (cartProduct.Quantity == 0)
            {
                CartProducts.Remove(cartProduct);
            }

            var inventoryProduct = InventoryProducts.FirstOrDefault(p => p?.Id == productId);
            if (inventoryProduct != null)
            {
                inventoryProduct.Quantity += quantity;
            }
        }

        public void Checkout()
        {
            const double salesTax = 0.07;
            double total = CartProducts.Sum(p => p?.Quantity * 10 ?? 0); // Assuming each product costs 10 units
            double totalWithTax = total * (1 + salesTax);

            Console.WriteLine("Receipt:");
            foreach (var product in CartProducts)
            {
                Console.WriteLine($"{product?.Name}: {product?.Quantity}");
            }
            Console.WriteLine($"Total: {totalWithTax:C}");
        }
    }
}
