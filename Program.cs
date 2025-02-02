
using System;
using System.Xml.Serialization;
using AmazonProject.Models;

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

            List<Product?> inventoryProducts = new List<Product?>();
            List<Product?> cartProducts = new List<Product?>();

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
                    Console.Write("Enter the item name >>> ");
                    string? name = Console.ReadLine() ?? "Unknown";
                    Console.Write("Enter the item quantity >>> ");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    inventoryProducts.Add(new Product { Name = name, Id = lastId++, Quantity = quantity });
                    break;

                case 'R':
                    Console.WriteLine("Reading all inventory items");
                    Console.WriteLine("ID\tName\tQuantity");
                    foreach (var product in inventoryProducts)
                    {
                        if (product?.Quantity > 0)
                        {
                            Console.WriteLine($"{product?.Id}\t{product?.Name}\t{product?.Quantity}");
                        }
                    }
                    break;

                case 'U':
                    Console.Write("Enter name of item to be updated >>> ");
                    name = Console.ReadLine();
                    foreach (var product in inventoryProducts)
                    {
                        if (product?.Name == name)
                        {
                            Console.Write("Enter new name >>> ");
                            product.Name = Console.ReadLine() ?? "Unknown";
                        }
                    }
                    break;

                case 'D':
                    Console.Write("Enter name of item to be deleted from inventory >>> ");
                    name = Console.ReadLine();
                    inventoryProducts.RemoveAll(product => product?.Name == name);
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
                    Console.Write("Enter item name >>> ");
                    string? name = Console.ReadLine() ?? "Unknown";
                    Console.Write("Enter item quantity >>> ");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    if (quantity <= 0)
                    {
                        Console.WriteLine("Invalid quantity");
                        break;
                    }

                    foreach (var product in inventoryProducts)
                    {
                        if (name == product?.Name && quantity <= product?.Quantity)
                        {
                            cartProducts.Add(new Product { Name = name, Id = product.Id, Quantity = quantity });
                            product.Quantity -= quantity;
                        }
                        else if (name == product?.Name && quantity > product?.Quantity)
                        {
                            Console.WriteLine("Not enough items in inventory");
                        }
                        else
                        {
                            Console.WriteLine("Item not found in inventory");
                        }
                    }
                    break;

                case 'R':
                    Console.WriteLine("Reading all inventory items");
                    Console.WriteLine("ID\tName\tQuantity");
                    foreach (var product in cartProducts)
                    {
                        if (product?.Quantity > 0)
                        {
                            Console.WriteLine($"{product?.Id}\t{product?.Name}\t{product?.Quantity}");
                        }
                    }
                    break;

                case 'U':
                    Console.Write("Enter item name >>> ");
                    name = Console.ReadLine();
                    Console.Write("Enter new quantity >>> ");
                    int desiredQuantity = Convert.ToInt32(Console.ReadLine());

                    int inventoryQuantity = 0;
                    int cartQuantity = 0;
                    int difference = 0;

                    foreach (var product in inventoryProducts)
                    {
                        if (name == product?.Name)
                        {
                            inventoryQuantity = product.Quantity;
                        }
                    }
                    foreach (var product in cartProducts)
                    {
                        if (name == product?.Name)
                        {
                            cartQuantity = product.Quantity;
                        }
                    }

                    // If user wants to add more of an item to cart
                    if (desiredQuantity >= cartQuantity)
                    {
                        difference = desiredQuantity - cartQuantity;
                        // If there are enough items in inventory
                        if (difference <= inventoryQuantity)
                        {
                            foreach (var product in inventoryProducts)
                            {
                                if (name == product?.Name)
                                {
                                    product.Quantity -= difference;
                                }
                            }
                            foreach (var product in cartProducts)
                            {
                                if (name == product?.Name)
                                {
                                    product.Quantity = desiredQuantity;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Not enough items in inventory");
                        }
                    }
                    // If user wants to remove some of an item from cart
                    else
                    {
                        difference = cartQuantity - desiredQuantity;
                        // If there are enough items in cart
                        if (difference <= cartQuantity)
                        {
                            foreach (var product in inventoryProducts) {
                                if (name == product?.Name)
                                {
                                    product.Quantity += difference;
                                }
                            }
                            foreach (var product in cartProducts)
                            {
                                if (name == product?.Name)
                                {
                                    product.Quantity = desiredQuantity;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Not enough items in cart");
                        }
                    }

                    break;

                case 'D':
                    Console.Write("Enter item name to be removed from cart >>> ");
                    name = Console.ReadLine();

                    foreach (var product in cartProducts)
                    {
                        if (name == product?.Name)
                        {
                            inventoryProducts.Add(product);
                        }
                    }
                    cartProducts.RemoveAll(p => p.Name == name);
                    break;

                case 'M':
                    Console.WriteLine("Switching to inventory mode");
                    inventoryMode = true;
                    break;
            }
            return inventoryMode;
        }
    }
}
