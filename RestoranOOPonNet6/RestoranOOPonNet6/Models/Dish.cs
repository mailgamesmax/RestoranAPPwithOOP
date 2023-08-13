using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOOPonNet6.Models
{
    internal class Dish : Аssortiment
    {

        public Dish() { }

        public Dish(string name, double price) : base(name, price)
        {
            Kind = KindVariables.Patiekalas;
        }

        public Dish(string name, double price, string description) : base(name, price, description)
        {
            Kind = KindVariables.Patiekalas;
        }

        public Dish CreateDish(string newName, double newPrice, string newDescription)
        {
            var newDish = new Dish(newName, newPrice, newDescription);
            return newDish;
        }


    }
}

