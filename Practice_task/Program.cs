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
            "| | 6. Save TO File \t| |\n" +
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
                Console.Write("Enter your chioce: ");
                userChoice = Convert.ToInt32(Console.ReadLine());//add error

                if (userChoice == 1)
                {
                    //add product
                    Console.Write("Enter product description: ");
                    string desc = Console.ReadLine();

                    Console.Write("Enter product price: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        AddProduct(desc, price);
                    }
                    else
                    {
                        Console.WriteLine("Invalid price.");
                    }
                }
                else if (userChoice == 2)
                {
                    //remove product
                    ShowProductsToDeleate();

                    Console.Write("Enter the product number to remove ot 0 to cancel:  ");
                    userChoice = Convert.ToInt32(Console.ReadLine());//add error

                    if(userChoice != 0)
                    {
                        DeleteProduct(userChoice);
                    }
                    else
                    {
                        userChoice = 1;
                    }

                    continue;
                }
                else if (userChoice == 3)
                {
                    GetTipType();
                }
                else if (userChoice == 4)
                {
                    //show
                }
                else if(userChoice == 5)
                {
                    //clear
                }
                else if(userChoice == 6)
                {
                    //save
                }
                else if(userChoice == 7)
                {
                    //upload
                }
                else if(userChoice == 0)
                {
                    //exit
                    Console.WriteLine("Have a nice day!");
                }
                else
                {
                    //again
                    Console.WriteLine("Bad option!");
                }

            } while (userChoice != 0);
        }

        private static void AddProduct(string description, decimal price)
        {
            Array.Resize(ref DescriptionList, DescriptionList.Length + 1);
            DescriptionList[DescriptionList.Length - 1] = description;

            Array.Resize(ref PriceList, PriceList.Length + 1);
            PriceList[PriceList.Length - 1] = price;

            Console.WriteLine($"Added: {description} - {price}");
        }

        private static void ShowProductsToDeleate()
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

        private static void DeleteProduct(int itemNo)
        {
            int index = itemNo - 1;

            if (index < 0 || index >= DescriptionList.Length)
            {
                Console.WriteLine("Invalid item number.");
                return;
            }

            string[] newDescriptions = new string[DescriptionList.Length - 1];
            decimal[] newPrices = new decimal[PriceList.Length - 1];

            int newIdx = 0;
            for (int i = 0; i < DescriptionList.Length; i++)
            {
                if (i == index)
                    continue;

                newDescriptions[newIdx] = DescriptionList[i];
                newPrices[newIdx] = PriceList[i];
                newIdx++;
            }

            DescriptionList = newDescriptions;
            PriceList = newPrices;

            Console.WriteLine("Product removed successfully.");
        }

        private static decimal GetTotalPrice()
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

        private static void GetTipType()
        {
            int tipType = 0;

            if(DescriptionList.Length == 0)
            {
                Console.WriteLine("There a no products in the bill to add tip for");
                return;
            }
            string menuTips =
                "1 - Tip Percentage\n" +
                "2 - Tip Amount\n" +
                "3 - No Tip (delete tip)";

            Console.WriteLine("Net Total: $" + GetTotalPrice());
            Console.WriteLine(menuTips);

            tipType = Convert.ToInt32(Console.ReadLine());

            AddTip(tipType);
        }

        private static void AddTip(int tipType)
        {
            if (tipType == 1)
            {
                Console.Write("Enter Tip percent: ");
                int percentage = Convert.ToInt32(Console.ReadLine());//add error

                TipPrice = GetTotalPrice() * ((decimal)percentage / 100);

                Console.WriteLine($"Tip Added: {TipPrice} ({percentage}%)");
            }
            else if(tipType == 2)
            {
                Console.Write("Enter Tip amount: ");
                int amount = Convert.ToInt32(Console.ReadLine());//add error

                TipPrice = amount;

                Console.WriteLine($"Tip Added: {TipPrice}");
            }
            else if(tipType == 3)
            {
                Console.WriteLine("No tips (tip cleared)");
                TipPrice = 0;
            }
            else
            {
                Console.WriteLine("Invalide tip type!");
            }
        }

        private static void ShowProducts()
        {

        }
    }
}
