using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal abstract class CommonFunctions
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
                Console.WriteLine("\t<<< nerastas aktualus failas >>>\n");
                return false;
            }
        }


        public void UpdateFile(string filePath, string temporaryfilePath)
        {
            if (IsFileAvailableToChange(filePath) && IsFileAvailableToChange(temporaryfilePath))
            {
                File.Delete(filePath);
                File.Move(temporaryfilePath, filePath);
            }
            else Console.WriteLine("\t Pakeitimai NEišsaugoti");

        }

        public CommonFunctions()
        {            
            currentDirectory = Directory.GetCurrentDirectory() + "\\myFiles";
        }

        public static string currentDirectory; // protected?
    }
}
