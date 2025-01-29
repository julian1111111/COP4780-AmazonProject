
using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool inventoryMode = true;

            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("I: Inventory management");
            Console.WriteLine("S: Shopping cart management");

            while (true)
            {
                string? input = Console.ReadLine();

                if (input?.ToUpper() == "I")
                {
                    Console.WriteLine("\tInventory mode");
                    Console.WriteLine("C: Create new inventory item");
                    Console.WriteLine("R: Read all inventory items");
                    Console.WriteLine("U: Update an inventory item");
                    Console.WriteLine("D: Delete an inventory item");
                }

                else if (input?.ToUpper() == "S")
                {
                    inventoryMode = false;
                    Console.WriteLine("\tShopping cart mode");
                    Console.WriteLine("R: Read all items in shopping cart");
                    Console.WriteLine("U: Update number of items in shopping cart");
                    Console.WriteLine("D: Remove an item from shopping cart");
                }

                else if (input?.ToUpper() == "Q")
                {
                    break;
                }

                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }
    }
}
