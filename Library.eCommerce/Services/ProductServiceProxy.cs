using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonProject.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        // Private constructor to prevent instantiation
        private ProductServiceProxy() { }

        // Singleton pattern
        private static ProductServiceProxy? instance;
        private static object instanceLock = new object();
        public static ProductServiceProxy Current
        {
            get
            {
                lock(instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }
                }
                return instance;
            }
        }

        // Separate inventory and cart product lists.
        // Should be private...
        public List<Product?> InventoryProducts { get; } = new List<Product?>();
        public List<Product?> CartProducts { get; } = new List<Product?>();

        // Last key used for product ID
        public int LastKey { get; private set; }

        // Add or update inventory
        public Product AddOrUpdateInventory(Product product)
        {
            var existingProduct = InventoryProducts.FirstOrDefault(p => p?.Id == product.Id);
            // If product doesn't exist, add it to inventory
            if (existingProduct == null)
            {
                Console.WriteLine("Adding new product to inventory");
                product.Id = ++LastKey;
                InventoryProducts.Add(product);
            }
            // If product exists, update its name and quantity
            else
            {
                existingProduct.Name = product.Name;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Price = product.Price;
            }

            return product;
        }

        // Add a product to cart 
        public void AddToCart(int productId, int quantity)
        {
            // Prevent program from crashing if product is not found
            try
            {
                var inventoryProduct = InventoryProducts.FirstOrDefault(p => p?.Id == productId);
                var cartProduct = CartProducts.FirstOrDefault(p => p?.Id == productId);
                // If given product is not found in inventory, throw an exception
                if (inventoryProduct == null)
                {
                    throw new InvalidOperationException("Product not found in inventory");
                }
                // If there is not enough inventory, throw an exception
                else if (inventoryProduct.Quantity < quantity)
                {
                    throw new InvalidOperationException("Not enough inventory");
                }

                // Remove appropriate quantity from given item in inventory
                inventoryProduct.Quantity -= quantity;

                // If product is not in cart, add it to cart
                if (cartProduct == null)
                {
                    CartProducts.Add(new Product { Id = productId, Name = inventoryProduct.Name, Quantity = quantity, Price = inventoryProduct.Price });
                }
                // If product is already in cart, add quantity to it
                else
                {
                    cartProduct.Quantity += quantity;
                }

                // If user has added the last of an item to cart, remove it from inventory
                if (inventoryProduct.Quantity == 0)
                {
                    InventoryProducts.Remove(inventoryProduct);
                }
            }
            // Print error messages 
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n");
            }
        }

        // Remove a product from cart
        public void RemoveFromCart(int productId, int quantity)
        {
            // Prevent program from crashing if exception is raised 
            try
            {
                var cartProduct = CartProducts.FirstOrDefault(p => p?.Id == productId);
                // If product is not in cart or requested removal quantity is greater than cart quantity, throw an exception
                if (cartProduct == null || cartProduct.Quantity < quantity)
                {
                    throw new InvalidOperationException("Not enough items in the cart.");
                }

                // Remove quantity from cart product, remove the product from cart if quantity is now 0
                cartProduct.Quantity -= quantity;
                if (cartProduct.Quantity == 0)
                {
                    CartProducts.Remove(cartProduct);
                }

                // Add removed quantity back to inventory
                var inventoryProduct = InventoryProducts.FirstOrDefault(p => p?.Id == productId);
                if (inventoryProduct != null)
                {
                    inventoryProduct.Quantity += quantity;
                }
                // Create new inventory item if it doesn't already exist 
                else
                {
                    InventoryProducts.Add(new Product { Id = productId, Name = cartProduct.Name, Quantity = quantity, Price = cartProduct.Price });
                }
            }
            // Print error messages
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n");
            }
        }

        // Checkout function to print receipt
        public void Checkout()
        {
            const double salesTax = 0.07;
            double total = CartProducts.Sum(p => p?.Quantity * p?.Price ?? 0); // Assuming each product costs 10 units
            double totalWithTax = total * (1 + salesTax);

            Console.WriteLine("\t\t\tReceipt\n\nProduct Name\t\tQuantity\t\tPrice");
            foreach (var product in CartProducts)
            {
                Console.WriteLine($"{product?.Name}\t\t\t{product?.Quantity}\t\t\t{(product?.Price * product?.Quantity):C}");
            }
            Console.WriteLine($"7% Sales Tax\t\t\t\t\t{(totalWithTax - total):C}\n\n\t\t\t\t\tTotal:\t{totalWithTax:C}");
        }
    }
}
