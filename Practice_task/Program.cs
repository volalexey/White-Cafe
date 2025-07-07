using System.ComponentModel.Design;

namespace Practice_task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        private static void Menu()
        {
            int userChoice;
            string optionsList =
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

            do
            {
                Console.WriteLine(optionsList);

                Console.Write("Enter your chioce: ");
                userChoice = Convert.ToInt32(Console.ReadLine());//add error
            } while (userChoice != 0);
        }
    }
}
