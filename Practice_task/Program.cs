using System.ComponentModel.Design;

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

        static void Main(string[] args)
        {
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
                }
                else if (userChoice == 3)
                {
                    //add tip
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
    }
}
