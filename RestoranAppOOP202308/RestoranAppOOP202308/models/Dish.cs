﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranAppOOP202308.models
{
    internal class Dish : Аssortiment
    {

        public Dish() { }

        public Dish(KindVariables kindVariables, string name, Dictionary<string, double> price) : base(name, price)
        {
            kindVariables = Аssortiment.KindVariables.Patiekalas;
        }
        


    }
}
