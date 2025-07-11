using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Practice_task
{
    internal class Program
    {
        private const string _menuList =
            " _________________________\n" +
            "|                         |\n" +
            "|  ---------------------  |\n" +
            "| | 1. Add Product \t| |\n" +
            "| | 2. Remove Product \t| |\n" +
            "| | 3. Add Tip \t\t| |\n" +
            "| | 4. Display Bill \t| |\n" +
            "| | 5. Clear All \t| |\n" +
            "| | 6. Save To File \t| |\n" +
            "| | 7. Load from File \t| |\n" +
            "| | 0. Exit \t\t| |\n" +
            "|  ---------------------  |\n" +
            "|                         |\n" +
            " _________________________\n";

        private static decimal[] PriceList = { };
        private static string[] DescriptionList = { };
        private static decimal TipPrice = 0;

        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            Menu();
        }

        private static void Menu()
        {
            int userChoice;
            Console.WriteLine(_menuList);

            do
            {
                Console.Write("\nEnter your choice: ");
                if (!int.TryParse(Console.ReadLine(), out userChoice))
                {
                    Console.WriteLine("Invalid input. Please enter a number from 0 to 7.");
                    userChoice = -1;
                    continue;
                }


                if (userChoice == 1)
                {
                    AddProductOption();
                }
                else if (userChoice == 2)
                {
                    DeleteProductOption();
                }
                else if (userChoice == 3)
                {
                    GetTipType();
                }
                else if (userChoice == 4)
                {
                    ShowProducts();
                }
                else if(userChoice == 5)
                {
                    ClearOrder();
                }
                else if(userChoice == 6)
                {
                    SaveToFile();
                }
                else if(userChoice == 7)
                {
                    LoadFromFile();
                }
                else if(userChoice == 0)
                {
                    Console.WriteLine("Have a nice day!");
                }
                else
                {
                    Console.WriteLine("Invalid option. Try again.");
                }

            } while (userChoice != 0);
        }

        private static void AddProduct(string description, decimal price)
        {
            Array.Resize(ref DescriptionList, DescriptionList.Length + 1);
            DescriptionList[DescriptionList.Length - 1] = description;

            Array.Resize(ref PriceList, PriceList.Length + 1);
            PriceList[PriceList.Length - 1] = price;

            Console.WriteLine($"Add item was successful.");
        }

        private static void AddProductOption()
        {
            if (DescriptionList.Length >= 5)
            {
                Console.WriteLine("Cannot add more than 5 products.");
                return;
            }

            Console.Write("Enter product description (3-20 chars): ");
            string desc = Console.ReadLine().Trim();
            if (desc.Length < 3 || desc.Length > 20)
            {
                Console.WriteLine("Description must be between 3 and 20 characters.");
                return;
            }

            decimal price = 0;
            while (true)
            {
                Console.Write("Enter product price (>0): ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
                {
                    break;
                }
                Console.WriteLine("Invalid price. Must be a positive number.");
            }

            AddProduct(desc, price);
        }

        private static void ShowProductsToDelete()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0,5} {1,-20} {2,10}", "No", "Description", "Price"));
            sb.AppendLine(new string('-', 37));

            for (int i = 0; i < DescriptionList.Length; i++)
            {
                sb.AppendLine(string.Format("{0,5} {1,-20} {2,10}",
                    i + 1,
                    DescriptionList[i],
                    PriceList[i].ToString("C")));
            }

            Console.WriteLine(sb.ToString());
        }

        private static bool DeleteProduct(int itemNo)
        {
            int index = itemNo - 1;
            if (index < 0 || index >= DescriptionList.Length)
            {
                Console.WriteLine("Invalid item number. Please try again.");
                return false;
            }

            var newDesc = new List<string>();
            var newPrices = new List<decimal>();

            for (int i = 0; i < DescriptionList.Length; i++)
            {
                if (i != index)
                {
                    newDesc.Add(DescriptionList[i]);
                    newPrices.Add(PriceList[i]);
                }
            }

            DescriptionList = newDesc.ToArray();
            PriceList = newPrices.ToArray();

            Console.WriteLine("Item removed successfully.");

            TipPrice = 0;
            Console.WriteLine("Tip cleared.");

            GetTipType();

            return true;
        }

        private static void DeleteProductOption()
        {
            if (DescriptionList.Length == 0)
            {
                Console.WriteLine("No products to delete.");
                return;
            }

            ShowProductsToDelete();

            while (true)
            {
                Console.Write("Enter the product number to remove or 0 to cancel: ");

                if (!int.TryParse(Console.ReadLine(), out int itemNo))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (itemNo == 0)
                {
                    Console.WriteLine("Returning to main menu.");
                    return;
                }

                if (DeleteProduct(itemNo))
                {
                    return;
                }
            }
        }

        private static decimal GetProductsPrice()
        {
            if (DescriptionList.Length == 0)
            {
                Console.WriteLine("There a no products in the bill to calculate total price");
                return -1;
            }

            decimal totalByProducts = 0;
            foreach (decimal price in PriceList)
            {
                totalByProducts += price;
            }
            return totalByProducts;
        }

        private static decimal GetGSTPrice()
        {
            return GetProductsPrice() * (decimal)0.05;
        }

        private static decimal GetTotalPrice()
        {
            return GetProductsPrice() + GetGSTPrice() + TipPrice;
        }

        private static void GetTipType()
        {
            if (DescriptionList.Length == 0)
            {
                Console.WriteLine("There are no products to add a tip for.");
                return;
            }

            Console.WriteLine($"\nNet Total: {GetProductsPrice():C}");
            Console.WriteLine("1 - Tip Percentage\n2 - Tip Amount\n3 - No Tip (clear)\n0 - Back");

            while (true)
            {
                Console.Write("\nEnter tip method: ");
                if (!int.TryParse(Console.ReadLine(), out int tipType))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (tipType == 0)
                {
                    Console.WriteLine("Returning to main menu.");
                    return;
                }

                if (tipType >= 1 && tipType <= 3)
                {
                    AddTip(tipType);
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid tip option.");
                }
            }
        }

        private static void AddTip(int tipType)
        {
            if (tipType == 1)
            {
                while (true)
                {
                    Console.Write("Enter Tip percent (>0): ");
                    if (!int.TryParse(Console.ReadLine(), out int perc))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    if (perc > 0)
                    {
                        TipPrice = GetProductsPrice() * perc / 100m;
                        Console.WriteLine($"Tip Added: {TipPrice:C} ({perc}%)");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Percentage must be greater than 0.");
                    }
                }
            }
            else if (tipType == 2)
            {
                while (true)
                {
                    Console.Write("Enter Tip amount (>=0): ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    if (amount >= 0)
                    {
                        TipPrice = amount;
                        Console.WriteLine($"Tip Added: {TipPrice:C}");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Tip amount must be >= 0.");
                    }
                }
            }
            else if (tipType == 3)
            {
                TipPrice = 0;
                Console.WriteLine("Tip cleared.");
            }
            else
            {
                Console.WriteLine("Invalid tip type.");
            }
        }

        private static void ShowProducts()
        {
            if (DescriptionList.Length == 0)
            {
                Console.WriteLine("No products to display.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{"Description",-40} {"Price",10}");
            sb.AppendLine(new string('-', 52));

            for (int i = 0; i < DescriptionList.Length; i++)
            {
                sb.AppendLine($"{DescriptionList[i],-40} {PriceList[i],10:C}");
            }

            sb.AppendLine(new string('-', 52));
            sb.AppendLine($"{"NetTotal:",-40} {GetProductsPrice(),10:C}");
            sb.AppendLine($"{"Tip Amount:",-40} {TipPrice,10:C}");
            sb.AppendLine($"{"GST Amount:",-40} {GetGSTPrice(),10:C}");
            sb.Append($"{"Total Amount:",-40} {GetTotalPrice(),10:C}");

            Console.WriteLine(sb.ToString());
        }

        private static void ClearOrder()
        {
            DescriptionList = [];
            PriceList = [];
            TipPrice = 0;

            Console.WriteLine("Order cleared.");
        }

        private static void SaveToFile()
        {
            if (DescriptionList.Length == 0)
            {
                Console.WriteLine("No products to save.");
                return;
            }

            string fileName;
            do
            {
                Console.Write("Enter filename (1-10 chars, without extension) or 0 to cancel: ");
                fileName = Console.ReadLine().Trim();

                if (fileName == "0")
                {
                    Console.WriteLine("Save cancelled.");
                    return;
                }

                if (string.IsNullOrEmpty(fileName) || fileName.Length > 10)
                {
                    Console.WriteLine("Filename must be between 1 and 10 characters.");
                    continue;
                }

                break;
            } while (true);

            string fullFileName = fileName + ".txt";

            if (File.Exists(fullFileName))
            {
                Console.Write($"File '{fullFileName}' already exists. Overwrite? (y/n): ");
                string overwrite = Console.ReadLine().Trim().ToLower();
                if (overwrite != "y")
                {
                    Console.WriteLine("Save cancelled.");
                    return;
                }
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(fullFileName))
                {
                    for (int i = 0; i < DescriptionList.Length; i++)
                    {
                        sw.WriteLine($"{DescriptionList[i]}|{PriceList[i]}");
                    }
                }
                Console.WriteLine($"Order saved to file '{fullFileName}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        private static void LoadFromFile()
        {
            string fileName;
            do
            {
                Console.Write("Enter filename to load (1-10 chars, without extension) or 0 to cancel: ");
                fileName = Console.ReadLine().Trim();

                if (fileName == "0")
                {
                    Console.WriteLine("Load cancelled.");
                    return;
                }

                if (string.IsNullOrEmpty(fileName) || fileName.Length > 10)
                {
                    Console.WriteLine("Filename must be between 1 and 10 characters.");
                    continue;
                }

                break;
            } while (true);

            string fullFileName = fileName + ".txt";

            if (!File.Exists(fullFileName))
            {
                Console.WriteLine($"File '{fullFileName}' does not exist.");
                return;
            }

            try
            {
                var lines = File.ReadAllLines(fullFileName);
                var desc = new List<string>();
                var prices = new List<decimal>();

                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length != 2 ||
                        parts[0].Length < 3 || parts[0].Length > 20 ||
                        !decimal.TryParse(parts[1], out decimal price) || price <= 0)
                    {
                        Console.WriteLine($"Invalid line skipped: {line}");
                        continue;
                    }

                    if (desc.Count >= 5)
                    {
                        Console.WriteLine("Cannot load more than 5 products. Some lines skipped.");
                        break;
                    }

                    desc.Add(parts[0]);
                    prices.Add(price);
                }

                DescriptionList = desc.ToArray();
                PriceList = prices.ToArray();
                TipPrice = 0;

                Console.WriteLine($"Order loaded from '{fullFileName}'. Tip cleared.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
    }
}
