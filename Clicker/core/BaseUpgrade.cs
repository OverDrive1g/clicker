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
        private string Name { get; set; }
        private readonly double _basePrice;
        private readonly double _multipiler;
        public int Count { get; set; }
        public int Power { get; private set; }
        
        public double Price
        {
            get => _basePrice * Math.Pow(_multipiler, Count);
            private set => throw new NotImplementedException();
        }

        public BaseUpgrade(String name, double basePrice, double multipiler, int count, int power)
        {
            Name = name;
            _basePrice = basePrice;
            _multipiler = multipiler;
            Count = count;
            Power = power;
        }

        public void IncPower()
        {
            Power++;
        }
    }
}
