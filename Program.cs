
using System;
using System.Xml.Serialization;
using AmazonProject.Models;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool inventoryMode = true;
            bool modeChange = true;

            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("I: Inventory management");
            Console.WriteLine("S: Shopping cart management");

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

                inventoryMode = inventoryMode ? inventoryOperations(choice) : cartOperations(choice);

                if (oldMode != inventoryMode)
                {
                    Console.WriteLine("CHANGED MODES");
                    modeChange = true;
                }
                else
                {
                    modeChange = false;
                }

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
            }
        }

        static bool cartOperations(char cartOp)
        {
            bool inventoryMode = false;

            switch (char.ToUpper(cartOp))
            {
                case 'C':
                    Console.WriteLine("Adding item to car");
                    break;
                case 'R':
                    Console.WriteLine("Reading all items in cart");
                    break;
                case 'U':
                    Console.WriteLine("Updating number of items in cart");
                    break;
                case 'D':
                    Console.WriteLine("Removing item from cart");
                    break;
                case 'M':
                    Console.WriteLine("Switching to inventory mode");
                    inventoryMode = true;
                    break;
            }

            return inventoryMode;
        }

        static bool inventoryOperations(char inventoryOp)
        {
            bool inventoryMode = true;

            switch (char.ToUpper(inventoryOp))
            {
                case 'C':
                    Console.WriteLine("Creating new inventory item");
                    break;
                case 'R':
                    Console.WriteLine("Reading all inventory items");
                    break;
                case 'U':
                    Console.WriteLine("Updating an inventory item");
                    break;
                case 'D':
                    Console.WriteLine("Deleting an inventory item");
                    break;
                case 'M':
                    Console.WriteLine("Switching to shopping cart mode");
                    inventoryMode = false;
                    break;
            }

            return inventoryMode;
        }
    }
}
