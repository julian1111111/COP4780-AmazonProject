
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
            // Initial id for first product added to inventory
            int lastId = 1;

            // Keep track of mode to determine which operations to perform
            // modeChange is used to print menu only when mode changes
            bool inventoryMode = true;
            bool modeChange = true;

            char choice;

            // Initial menu at start of program
            Console.WriteLine("Welcome to Amazon!");
            Console.WriteLine("I: Inventory management");
            Console.WriteLine("S: Shopping cart management");
            Console.Write(">>> ");

            // Store separate lists of inventory and cart 
            List<Product?> inventoryProducts = ProductServiceProxy.Current.InventoryProducts;
            List<Product?> cartProducts = ProductServiceProxy.Current.CartProducts;

            // Get first choice of mode, loops until valid input is received
            do
            {
                string? input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    choice = input[0];
                    if (char.ToUpper(choice) == 'I' || char.ToUpper(choice) == 'S')
                    {
                        break;
                    }
                }
                Console.WriteLine("Invalid input. Use 'I' or 'S' to select mode");
                Console.Write(">>> ");
            } while (true);

            // Not super necessary for this console app, user will not be able 
            // to do anything if their first choice is to enter cart mode
            inventoryMode = char.ToUpper(choice) == 'I' ? true :
                            char.ToUpper(choice) == 'S' ? false :
                            inventoryMode;

            // Entire program loop
            do
            {
                // Used to determine if new menu should be printed.
                // oldMode is set at the start of loop, compared again at the end
                bool oldMode = inventoryMode;
                // Print appropriate menu based on mode and whether or not the user has changed mode
                printMenu(inventoryMode, modeChange);

                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }
                choice = input[0];

                // Perform inventory operations if in inventory mode, cart operations if not
                inventoryMode = inventoryMode ? inventoryOperations(choice, ref inventoryProducts, ref lastId)
                                              : cartOperations(choice, ref inventoryProducts, ref cartProducts);

                // If the mode has changed since the last loop iteration, 
                // modeChange is set accordingly 
                modeChange = oldMode != inventoryMode ? true : false;

            } while (char.ToUpper(choice) != 'Q');

            Console.WriteLine();
            ProductServiceProxy.Current.Checkout();
        }

        // Menu function to print appropriate menu based on mode and whether or not the user has changed mode
        static void printMenu(bool inventoryMode, bool modeChange)
        {
            // Inventory mode menu
            if (inventoryMode)
            {
                // Always print this when in inventory mode
                Console.WriteLine("\n\tInventory mode");
                // Print a reminder of the available operations if the user has changed into inventory mode
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
            // Cart mode menu
            else
            {
                // Always print this in cart mode
                Console.WriteLine("\n\tShopping cart mode");
                // Print a reminder of the available operations if the user has changed into cart mode
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

        // Inventory switch function 
        static bool inventoryOperations(char inventoryOp, ref List<Product?> inventoryProducts, ref int lastId)
        {
            // CRUD functions are dual-equipped for inventory and cart.
            // Since program made it to this function, obviously need to send 
            // those functions inventoryMode as true
            bool inventoryMode = true;
            switch (char.ToUpper(inventoryOp))
            {
                case 'C':
                    // CRUD functions for inventory mode will not edit cart list, but expect three parameters
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
                    // Return indicator that mode has changed
                    Console.WriteLine("Switching to shopping cart mode");
                    inventoryMode = false;
                    break;
            }
            return inventoryMode;
        }

        // Cart switch function 
        static bool cartOperations(char cartOp, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            // Inventory mode should be false if program made it to this function
            bool inventoryMode = false;
            switch (char.ToUpper(cartOp))
            {
                case 'C':
                    // Cart list is passed to CRUD functions when in cart mode, since they will
                    // need to edit both lists
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
                    // Return indicator that mode has changed 
                    Console.WriteLine("Switching to inventory mode");
                    inventoryMode = true;
                    break;
            }
            return inventoryMode;
        }

        // Dual-purpose create function for inventory and cart
        static void CreateItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            // Add product to inventory 
            if (inventoryMode)
            {
                string name = GetValidString("Enter product name >>> ");
                int quantity = GetValidInteger("Enter product quantity >>> ");
                double price = GetValidDouble("Enter product price >>> ");
                ProductServiceProxy.Current.AddOrUpdateInventory(new Product { Name = name, Quantity = quantity, Price = price });
            }

            // Add product to cart from inventory 
            else
            {
                // Print inventory for better UX
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to add to cart >>> ");
                int quantity = GetValidInteger("Enter product quantity >>> ");
                ProductServiceProxy.Current.AddToCart(id, quantity);
            }
        }

        // Dual-purpose read function for inventory and cart
        static void ReadItems(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            // Print inventory 
            if (inventoryMode)
            {
                Console.WriteLine("\tInventory\nID\tName\t\t\tQuantity\tPrice\n-----------------------------------------------------");
                inventoryProducts.ForEach(Console.WriteLine);
            }

            // Print cart 
            else
            {
                Console.WriteLine("\tCart\nID\tName\t\t\tQuantity\tPrice\n-----------------------------------------------------");
                cartProducts.ForEach(Console.WriteLine);
            }
        }

        // Dual-purpose update function for inventory and cart
        static void UpdateItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            // Update inventory product
            if (inventoryMode)
            {
                // Print inventory for better UX
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                // Ensure valid user input 
                int id = GetValidInteger("Enter product ID to update >>> ");
                string name = GetValidString("Enter new product name >>> ");
                int quantity = GetValidInteger("Enter new product quantity >>> ");
                double price = GetValidDouble("Enter new product price >>> ");

                ProductServiceProxy.Current.AddOrUpdateInventory(new Product { Id = id, Name = name, Quantity = quantity, Price = price });
                Console.WriteLine("Product updated in inventory");
                // Show updated inventory 
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
            }

            // Update cart product 
            else
            {
                // Print both inventory and cart for better UX
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                Console.WriteLine();
                ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
                // Ensure valid user input
                int id = GetValidInteger("Enter product ID to update >>> ");
                int quantity = GetValidInteger("Enter new product quantity >>> ");

                // Keep track of product in cart and inventory that matches input id
                Product? targetCartProduct = cartProducts.FirstOrDefault(p => p?.Id == id);
                Product? targetInventoryProduct = inventoryProducts.FirstOrDefault(p => p?.Id == id);

                if (targetCartProduct == null)
                {
                    Console.WriteLine("ERROR: Product with id: " + id + " not found in cart.");
                    return;
                }

                // User wants less of item in cart (sent a quantity less than that which is in cart)
                if (quantity <= targetCartProduct?.Quantity)
                {
                    ProductServiceProxy.Current.RemoveFromCart(id, targetCartProduct.Quantity - quantity);
                }
                // User wants more of item in cart (sent a quantity greater than that which is in cart)
                else
                {
                    ProductServiceProxy.Current.AddToCart(id, quantity - targetCartProduct.Quantity);
                }
                // Show both updated inventory and cart 
                Console.WriteLine("Product updated in cart and inventory");
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                Console.WriteLine();
                ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
            }
        }

        // Dual-purpose delete function for inventory and cart
        static void DeleteItem(bool inventoryMode, ref List<Product?> inventoryProducts, ref List<Product?> cartProducts)
        {
            // Delete inventory product 
            if (inventoryMode)
            {
                // Print inventory for better UX
                ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to delete from inventory >>> ");
                var product = ProductServiceProxy.Current.InventoryProducts.FirstOrDefault(p => p?.Id == id);
                // Remove product if matching id is found in inventory 
                if (product != null)
                {
                    ProductServiceProxy.Current.InventoryProducts.Remove(product);
                    Console.WriteLine("Product removed from inventory. Use 'R' to view inventory");
                }
                // Print an error if no product with matching id is found 
                else
                {
                    Console.WriteLine("ERROR: Product with id: " + id + " not found in inventory.");
                }
            }

            // Delete cart product 
            else
            {
                // Print cart for better UX
                ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
                int id = GetValidInteger("Enter product ID to remove from cart >>> ");
                var product = ProductServiceProxy.Current.CartProducts.FirstOrDefault(p => p?.Id == id);
                // Remove product if matching id is found in cart
                if (product != null)
                {
                    ProductServiceProxy.Current.RemoveFromCart(id, product.Quantity);
                    // Prints both updated inventory and cart to show that all cart products 
                    // are returned to inventory 
                    Console.WriteLine("Product removed from cart");
                    ReadItems(inventoryMode: true, ref inventoryProducts, ref cartProducts);
                    ReadItems(inventoryMode: false, ref inventoryProducts, ref cartProducts);
                }
                // Print an error if no product with matching id is found in cart
                else
                {
                    Console.WriteLine("ERROR: Product with id: " + id + " not found in cart.");
                }
            }
        }

        // Helper function to ensure valid user input
        // (gets in the way at the moment for updating item to 0 quantity)
        static int GetValidInteger(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                // Makes sure input is a positive integer 
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

        // Helper function to ensure valid user input
        static string GetValidString(string prompt)
        {
            string? result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                // Makes sure string is not null or empty 
                if (!string.IsNullOrWhiteSpace(result))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid input. Please enter a non-empty string.");
                }
            }
            return result;
        }

        // Helper function to ensure valid user input
        static double GetValidDouble(string prompt)
        {
            double result;
            while (true)
            {
                Console.Write(prompt);
                // Makes sure input is a positive decimal 
                if (double.TryParse(Console.ReadLine(), out result) && result > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid input. Please enter a valid decimal.");
                }
            }
            return result;
        }
    }
}
