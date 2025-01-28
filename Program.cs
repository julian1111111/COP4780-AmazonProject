
using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("I: Inventory management");
            Console.WriteLine("S: Shopping cart management");

            Console.WriteLine("C: Create new inventory item");
            Console.WriteLine("R: Read all inventory items");
            Console.WriteLine("U: Update an inventory item");
            Console.WriteLine("D: Delete an inventory item");

            string? input = Console.ReadLine();
        }
    }
}
