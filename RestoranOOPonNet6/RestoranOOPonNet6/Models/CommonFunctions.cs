using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal class CommonFunctions
    {
        public bool IsFileAvailableToChange(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        return true;
                }
                catch (IOException error)
                {
                    Console.WriteLine("Failų apdorojimo klaida: " + error.Message);
                    return false;
                }
            }
            else
            {
                    Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\t<<< nerastas \n{filePath}\nPrograma sukurs trūkstamą failą. >>>\n");
                    Console.ResetColor();

                try
                {
                   using (FileStream fs = File.Create(filePath))
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("čiki puki");
                        Console.ResetColor();
                    return true;
                }
                catch (IOException error)
                {
                    Console.WriteLine("Failų apdorojimo klaida: " + error.Message);
                    return false;
                }

            }
        }

        public void PrintSomethingWrong() 
        {
            Console.WriteLine("\n\t<<< operacija nepavko >>>\n");
        }

        public void UpdateFile(string originFilePath, string temporaryfilePath)
        {
            if (IsFileAvailableToChange(originFilePath) && IsFileAvailableToChange(temporaryfilePath))
            {
                File.Delete(originFilePath);
                File.Move(temporaryfilePath, originFilePath);
            }
            else Console.WriteLine("\t Failų klaida - pakeitimai NEišsaugoti");

        }

        public double ConvertInputToDouble() 
        {
            Console.WriteLine("*norint nutraukti operaciją įveskite neigiamą skaičių\n");
            double result = -1;
            bool trying = false;
            while (!trying) 
            {                
                string userInput = Console.ReadLine();
                trying = double.TryParse(userInput, out result);
                if (result == 0) Console.WriteLine("\tDėmesio - įvesta reikšmė - 0\n");
                if (result < 0) break; //?
            }
            return result;
        }

        public CommonFunctions()
        {            
            currentDirectory = Directory.GetCurrentDirectory() + "\\myFiles";
        }

        public static string currentDirectory; // protected?


    }
}
