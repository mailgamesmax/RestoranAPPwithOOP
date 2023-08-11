using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RestoranAppOOP202308.models.Аssortiment;

namespace RestoranAppOOP202308.models
{
    internal abstract class Аssortiment
    {
        public enum KindVariables 
        {
            Patiekalas,
            Gėrimas
        }
        public Аssortiment() { }

        public Аssortiment(string name, Dictionary<string, double> nameAndPrice)
        {
            
            Name = name;
            NameAndPrice = nameAndPrice;
            double inputPrice = 0;
            nameAndPrice.Add(name, inputPrice);
        }
        public Аssortiment(string name, Dictionary<string, double> price, string description) : this( name, price)
        {
            Description = description;
        }

        public Аssortiment CreateAssortiment(int 1) { }

            Jei nori įvesti gėrimą: 
ControWriteLine(Įvesk pavadinimą)
ControWriteLine(Įvesk kainą)
switch
 {
	case: 1
case: 2
	

        public string Name { get; set; }
        public Dictionary<string, double> NameAndPrice { get; set; } //name, price
        public string Description { get; set; }
        public KindVariables Kind { get; set; }
        public static List<Аssortiment> FullAssortiment { get; set; } = new List<Аssortiment>();
    }
}

