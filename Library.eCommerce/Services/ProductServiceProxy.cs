using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonProject.Models;
using Library.eCommerce.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        // Private constructor to prevent instantiation
        private ProductServiceProxy() {
            InventoryProducts = new List<Item?>
            {
                new Item { Product = new Product{ Id = ++LastKey, Name = "Mango", Quantity = 10, Price = 1.00 }, Id = ++LastKey, Quantity = 1 },
                new Item { Product = new Product{ Id = ++LastKey, Name = "Banana", Quantity = 10, Price = 1.00 }, Id = ++LastKey, Quantity = 1 },
                new Item { Product = new Product{ Id = ++LastKey, Name = "Orange", Quantity = 10, Price = 1.00 }, Id = ++LastKey, Quantity = 1 },
            };

            CartProducts = new List<Item?>();
        }

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
        // Should be private...?
        public List<Item?> InventoryProducts { get; private set; } = new List<Item?>();
        public List<Item?> CartProducts { get; private set; } = new List<Item?>();

        // Last key used for product ID
        public int LastKey { get; private set; }

        public Item? GetById(int id)
        {
            return InventoryProducts.FirstOrDefault(p => p.Id == id);
        }

        // Add or update inventory
        public Item AddOrUpdateInventory(Item item)
        {
            var existingProduct = InventoryProducts.FirstOrDefault(p => p?.Id == item.Id);
            // If product doesn't exist, add it to inventory
            if (existingProduct == null)
            {
                Console.WriteLine("Adding new product to inventory");
                item.Id = ++LastKey;
                item.Product.Id = item.Id;
                InventoryProducts.Add(item);
            }
            // If product exists, update its name and quantity
            else
            {
                existingProduct.Product.Name = item.Product.Name;
                existingProduct.Quantity = item.Quantity;
                existingProduct.Product.Price = item.Product.Price;
            }

            return item;
        }

        // Delete product from inventory
        public Item? DeleteFromInventory(int productId)
        {
            try
            {
                var product = InventoryProducts.FirstOrDefault(p => p?.Id == productId);

                // If product is not found in inventory, throw an exception
                if (product == null)
                {
                    throw new InvalidOperationException("Product not found in inventory");
                }
                else
                {
                    InventoryProducts.Remove(product);
                }

                return product;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}\n");
                return null;
            }
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
                    CartProducts.Add(new Item { Id = productId, Quantity = quantity, Product = new Product { Name = inventoryProduct.Product.Name, Price = inventoryProduct.Product.Price }});
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
                    InventoryProducts.Add(new Item { Id = productId, Quantity = quantity, Product = new Product { Name = cartProduct.Product.Name, Price = cartProduct.Product.Price } });
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
            double total = CartProducts.Sum(p => p?.Quantity * p?.Product?.Price ?? 0); // Assuming each product costs 10 units
            double totalWithTax = total * (1 + salesTax);

            Console.WriteLine("\t\t\tReceipt\n\nProduct Name\t\tQuantity\t\tPrice");
            foreach (var product in CartProducts)
            {
                Console.WriteLine($"{product?.Product?.Name}\t\t\t{product?.Quantity}\t\t\t{(product?.Product?.Price * product?.Quantity):C}");
            }
            Console.WriteLine($"7% Sales Tax\t\t\t\t\t{(totalWithTax - total):C}\n\n\t\t\t\t\tTotal:\t{totalWithTax:C}");
        }
    }
}
