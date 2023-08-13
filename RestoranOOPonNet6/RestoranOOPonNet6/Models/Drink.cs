using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal class Drink : Аssortiment
    {

        public Drink() { }

        public Drink(string name, double price) : base(name, price)
        {
            Kind = KindVariables.Gėrimas;
        }

        public Drink(string name, double price, string description) : base(name, price, description)
        {
            Kind = KindVariables.Gėrimas;
        }
    }
}
