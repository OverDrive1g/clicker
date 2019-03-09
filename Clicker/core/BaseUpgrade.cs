using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicker.core
{
    class BaseUpgrade
    {
        public String Name { get; set; }
        public double basePrice;
        public double multipiler;
        public int Count { get; set; }
        public int Power { get; private set; }
        
        public double Price { 
            get {
                return basePrice * Math.Pow(multipiler, Count);  
            } 
            private set {}
        }

        public BaseUpgrade(String name, double basePrice, double multipiler, int count, int power)
        {
            this.Name = name;
            this.basePrice = basePrice;
            this.multipiler = multipiler;
            this.Count = count;
            this.Power = power;
        }

        public void incPower()
        {
            Power++;
        }
    }
}
