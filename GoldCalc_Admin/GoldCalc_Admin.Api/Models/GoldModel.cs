using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldCalc.Api.Models
{
    public class GoldModel
    {
        public double Rate { get; set; }
        public double Weight { get; set; }
        public double? Discount { get; set; }
    }
}
