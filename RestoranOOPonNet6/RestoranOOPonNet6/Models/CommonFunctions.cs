using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal abstract class CommonFunctions
    {
        
            
        public CommonFunctions()
        {            
            currentDirectory = Directory.GetCurrentDirectory() + "\\myFiles";
        }

        public static string currentDirectory; // protected?
    }
}
