
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
                    CreateItem(inventoryMode);
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
                    CreateItem(inventoryMode);
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

        static void CreateItem(bool inventoryMode)
        {

            if (inventoryMode)
            {
                Console.Write("Enter the item name >>> ");
                string? name = Console.ReadLine() ?? "Unknown";
                Console.Write("Enter the item quantity >>> ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                //inventoryProducts.Add(new Product { Name = name, Id = lastId++, Quantity = quantity });
                ProductServiceProxy.Current.AddOrUpdateInventory(new Product { Name = name, Quantity = quantity });
            }
            else
            {
                Console.Write("Enter product ID to add to cart >>> ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Enter quantity to add to cart >>> ");
                int quantity = int.Parse(Console.ReadLine() ?? "0");
                //cartProducts.Add(new Product { Name = name, Id = lastId++, Quantity = quantity });
                ProductServiceProxy.Current.AddToCart(id, quantity);
            }
        }

        static void ReadItems(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?>cartProducts)
        {
            if (inventoryMode)
            {
                Console.WriteLine("\tInventory\nID\tName\tQuantity\n--------------------------");
                inventoryProducts.ForEach(Console.WriteLine);
            }

            else
            {
                Console.WriteLine("\tCart\nID\tName\tQuantity\n--------------------------");
                cartProducts.ForEach(Console.WriteLine);
            }
        }

        static void UpdateItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            if (inventoryMode)
            {
                Console.Write("Enter product ID to update >>> ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Enter new product name >>> ");
                string? name = Console.ReadLine();
                Console.Write("Enter new product quantity >>> ");
                int quantity = int.Parse(Console.ReadLine() ?? "0");

                ProductServiceProxy.Current.AddOrUpdateInventory(new Product { Id = id, Name = name, Quantity = quantity });
            }

            else
            {
                Console.Write("Enter product ID to update >>> ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Enter new product quantity >>> ");
                int quantity = int.Parse(Console.ReadLine() ?? "0");

                Product? targetCartProduct = cartProducts.FirstOrDefault(p => p?.Id == id);
                Product? targetInventoryProduct = inventoryProducts.FirstOrDefault(p => p?.Id == id);

                // want less of item in cart 
                if (quantity <= targetCartProduct?.Quantity)
                {
                    ProductServiceProxy.Current.RemoveFromCart(id, quantity);
                }
                // want more of item in cart 
                else
                {
                    ProductServiceProxy.Current.AddToCart(id, quantity);
                }
            }
        }

        static void DeleteItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            if (inventoryMode)
            {
                Console.Write("Enter product ID to delete from inventory >>> ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                var product = ProductServiceProxy.Current.InventoryProducts.FirstOrDefault(p => p?.Id == id);
                if (product != null)
                {
                    ProductServiceProxy.Current.InventoryProducts.Remove(product);
                }
            }
            else
            {
                Console.Write("Enter product ID to remove from cart >>> ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                var product = ProductServiceProxy.Current.CartProducts.FirstOrDefault(p => p?.Id == id);
                ProductServiceProxy.Current.RemoveFromCart(id, product.Quantity);
            }
        }
    }
}
