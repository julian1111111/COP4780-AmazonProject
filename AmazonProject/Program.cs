
using System;
using System.Xml.Serialization;
using AmazonProject.Models;
using Library.eCommerce.Services;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int lastId = 1;
            bool inventoryMode = true;
            bool modeChange = true;

            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("I: Inventory management");
            Console.WriteLine("S: Shopping cart management");
            Console.Write(">>> ");

            List<Product?> inventoryProducts = ProductServiceProxy.Current.InventoryProducts;
            List<Product?> cartProducts = ProductServiceProxy.Current.CartProducts;

            string? input = Console.ReadLine();
            char choice = input[0];

            inventoryMode = char.ToUpper(choice) == 'I' ? true :
                            char.ToUpper(choice) == 'S' ? false :
                            inventoryMode;

            if (char.ToUpper(choice) != 'I' && char.ToUpper(choice) != 'S')
            {
                Console.WriteLine("Invalid input");
            }

            do
            {
                bool oldMode = inventoryMode;
                printMenu(inventoryMode, modeChange);

                input = Console.ReadLine();
                choice = input[0];

                inventoryMode = inventoryMode ? inventoryOperations(choice, ref inventoryProducts, ref lastId)
                                              : cartOperations(choice, ref inventoryProducts, ref cartProducts);
                modeChange = oldMode != inventoryMode ? true : false;

            } while (char.ToUpper(choice) != 'Q');

            ProductServiceProxy.Current.Checkout();
        }

        static void printMenu(bool inventoryMode, bool modeChange)
        {
            if (inventoryMode)
            {
                Console.WriteLine("\n\tInventory mode");
                if (modeChange)
                {
                    Console.WriteLine("C: Create new inventory item");
                    Console.WriteLine("R: Read all inventory items");
                    Console.WriteLine("U: Update an inventory item");
                    Console.WriteLine("D: Delete an inventory item");
                    Console.WriteLine("M: Switch to shopping cart mode");
                    Console.WriteLine("Q: Checkout");
                }
                Console.Write(">>> ");
            }
            else
            {
                Console.WriteLine("\n\tShopping cart mode");
                if (modeChange)
                {
                    Console.WriteLine("C: Add item to shopping cart");
                    Console.WriteLine("R: Read all items in shopping cart");
                    Console.WriteLine("U: Update number of items in shopping cart");
                    Console.WriteLine("D: Remove an item from shopping cart");
                    Console.WriteLine("M: Switch to inventory mode");
                    Console.WriteLine("Q: Checkout");
                }
                Console.Write(">>> ");
            }
        }

        static bool inventoryOperations(char inventoryOp, ref List<Product?> inventoryProducts, ref int lastId)
        {
            bool inventoryMode = true;
            switch (char.ToUpper(inventoryOp))
            {
                case 'C':
                    CreateItem(inventoryMode, ref inventoryProducts, ref inventoryProducts);
                    break;

                case 'R':
                    ReadItems(inventoryMode, ref inventoryProducts, ref inventoryProducts);
                    break;

                case 'U':
                    UpdateItem(inventoryMode, ref inventoryProducts, ref inventoryProducts);
                    break;

                case 'D':
                    DeleteItem(inventoryMode, ref inventoryProducts, ref inventoryProducts);
                    break;

                case 'M':
                    Console.WriteLine("Switching to shopping cart mode");
                    inventoryMode = false;
                    break;
            }
            return inventoryMode;
        }

        static bool cartOperations(char cartOp, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            bool inventoryMode = false;
            switch (char.ToUpper(cartOp))
            {
                case 'C':
                    CreateItem(inventoryMode, ref inventoryProducts, ref cartProducts);
                    break;

                case 'R':
                    ReadItems(inventoryMode, ref inventoryProducts, ref cartProducts);
                    break;

                case 'U':
                    UpdateItem(inventoryMode, ref inventoryProducts, ref cartProducts);
                    break;

                case 'D':
                    DeleteItem(inventoryMode, ref inventoryProducts, ref cartProducts);
                    break;

                case 'M':
                    Console.WriteLine("Switching to inventory mode");
                    inventoryMode = true;
                    break;
            }
            return inventoryMode;
        }

        static void CreateItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            if (inventoryMode)
            {
                string name = GetValidString("Enter product name >>> ");
                int quantity = GetValidInteger("Enter product quantity >>> ");
                ProductServiceProxy.Current.AddOrUpdateInventory(new Product { Name = name, Quantity = quantity });
            }
            else
            {
                ReadItems(inventoryMode : true, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to add to cart >>> ");
                int quantity = GetValidInteger("Enter product quantity >>> ");
                ProductServiceProxy.Current.AddToCart(id, quantity);
            }
        }

        static void ReadItems(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?>cartProducts)
        {
            if (inventoryMode)
            {
                Console.WriteLine("\tInventory\nID\tName\t\t\tQuantity\n--------------------------------");
                inventoryProducts.ForEach(Console.WriteLine);
            }

            else
            {
                Console.WriteLine("\tCart\nID\tName\t\t\tQuantity\n--------------------------------");
                cartProducts.ForEach(Console.WriteLine);
            }
        }

        static void UpdateItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            if (inventoryMode)
            {
                ReadItems(inventoryMode : true, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to update >>> ");
                string name = GetValidString("Enter new product name >>> ");
                int quantity = GetValidInteger("Enter new product quantity >>> ");

                ProductServiceProxy.Current.AddOrUpdateInventory(new Product { Id = id, Name = name, Quantity = quantity });
                Console.WriteLine("Product updated in inventory");
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
            }

            else
            {
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                Console.WriteLine();
                ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to update >>> ");
                int quantity = GetValidInteger("Enter new product quantity >>> ");

                Product? targetCartProduct = cartProducts.FirstOrDefault(p => p?.Id == id);
                Product? targetInventoryProduct = inventoryProducts.FirstOrDefault(p => p?.Id == id);

                // want less of item in cart 
                if (quantity <= targetCartProduct?.Quantity)
                {
                    ProductServiceProxy.Current.RemoveFromCart(id, targetCartProduct.Quantity - quantity);
                }
                // want more of item in cart 
                else
                {
                    ProductServiceProxy.Current.AddToCart(id, quantity - targetCartProduct.Quantity);
                }
                Console.WriteLine("Product updated in cart and inventory");
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                Console.WriteLine();
                ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
            }
        }

        static void DeleteItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            if (inventoryMode)
            {
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to delete from inventory >>> ");
                var product = ProductServiceProxy.Current.InventoryProducts.FirstOrDefault(p => p?.Id == id);
                if (product != null)
                {
                    ProductServiceProxy.Current.InventoryProducts.Remove(product);
                    Console.WriteLine("Product removed from inventory. Use 'R' to view inventory");
                }
                else
                {
                    Console.WriteLine("ERROR: Product not found in inventory.");
                }
            }
            else
            {
                ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to remove from cart >>>");
                var product = ProductServiceProxy.Current.CartProducts.FirstOrDefault(p => p?.Id == id);
                if (product != null)
                {
                    ProductServiceProxy.Current.RemoveFromCart(id, product.Quantity);
                    Console.WriteLine("Product removed from cart");
                    ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                    ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
                }
                else
                {
                    Console.WriteLine("ERROR: Product with id: " + id + " not found in cart.");
                }
            }
        }

        static int GetValidInteger(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out result) && result > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid input. Please enter a valid integer.");
                }
            }
            return result;
        }

        static string GetValidString(string prompt)
        {
            string? result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid input. Please enter a non-empty name.");
                }
            }
            return result;
        }
    }
}
