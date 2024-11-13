using System;

namespace LinesCodeCounter
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Application.Run();
        }
    }

    public static class Application
    {
        public static void Run()
        {
            Console.Title = "Lines code counter";

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter path");
                Console.ForegroundColor = ConsoleColor.Yellow;
                string? path = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter extensions (separate by spaces, e.g., .cs .txt):");
                string? languagesExtensions = Console.ReadLine();

                List<string> listExtensions = GetSeparated(languagesExtensions);
                Folder rootFolder = new Folder(path);
                rootFolder.Open();

                int totalLines = rootFolder.CountLines(listExtensions);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Total lines of code: {totalLines}\n");
            }
        }

        static List<string> GetSeparated(string? p)
        {
            List<string> r = new List<string>();

            if (string.IsNullOrEmpty(p))
                return r;

            string currentWord = "";

            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] == ' ')
                {
                    if (!string.IsNullOrEmpty(currentWord))
                    {
                        r.Add(currentWord);
                        currentWord = "";
                    }
                }
                else
                    currentWord += p[i];
            }

            if (!string.IsNullOrEmpty(currentWord))
                r.Add(currentWord);

            return r;
        }
    }
}
